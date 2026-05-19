using eShop.Basket.API.Grpc;
using eShop.WebApp.Services.OrderStatus.IntegrationEvents;
using eShop.WebAppComponents.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Server;
using Microsoft.Extensions.AI;

public static class Extensions
{
    public static void AddApplicationServices(this IHostApplicationBuilder builder)
    {
        builder.AddAuthenticationServices();

        builder.AddRabbitMqEventBus("EventBus")
               .AddEventBusSubscriptions();

        builder.Services.AddHttpForwarderWithServiceDiscovery();

        // Application services
        builder.Services.AddScoped<BasketState>();
        builder.Services.AddScoped<LogOutService>();
        builder.Services.AddSingleton<BasketService>();
        builder.Services.AddSingleton<OrderStatusNotificationService>();
        builder.Services.AddSingleton<IProductImageUrlProvider, ProductImageUrlProvider>();

        builder.AddAIServices();

        // HTTP and GRPC client registrations
        var configuration = builder.Configuration;

        builder.Services.AddGrpcClient<Basket.BasketClient>(o =>
                o.Address = GetServiceAddress(configuration, "basket-api"))
            .AddAuthToken();

        builder.Services.AddHttpClient<CatalogService>(o =>
                o.BaseAddress = GetServiceAddress(configuration, "catalog-api"))
            .AddApiVersion(2.0)
            .AddAuthToken();

        builder.Services.AddHttpClient<OrderingService>(o =>
                o.BaseAddress = GetServiceAddress(configuration, "ordering-api"))
            .AddApiVersion(1.0)
            .AddAuthToken();
    }

    private static Uri GetServiceAddress(IConfiguration configuration, string serviceName)
    {
        var configured = configuration[$"ServiceUrls:{serviceName}"];

        if (!string.IsNullOrWhiteSpace(configured))
        {
            return new Uri(configured);
        }

        if (string.Equals(Environment.GetEnvironmentVariable("DOTNET_RUNNING_IN_CONTAINER"), "true", StringComparison.OrdinalIgnoreCase))
        {
            return new Uri($"http://{serviceName}:8080");
        }

        return serviceName switch
        {
            "catalog-api" => new Uri("http://catalog-api:8080"),
            "ordering-api" => new Uri("http://ordering-api:8080"),
            _ => new Uri($"http://{serviceName}:8080")
        };
    }

    public static void AddEventBusSubscriptions(this IEventBusBuilder eventBus)
    {
        eventBus.AddSubscription<OrderStatusChangedToAwaitingValidationIntegrationEvent, OrderStatusChangedToAwaitingValidationIntegrationEventHandler>();
        eventBus.AddSubscription<OrderStatusChangedToPaidIntegrationEvent, OrderStatusChangedToPaidIntegrationEventHandler>();
        eventBus.AddSubscription<OrderStatusChangedToStockConfirmedIntegrationEvent, OrderStatusChangedToStockConfirmedIntegrationEventHandler>();
        eventBus.AddSubscription<OrderStatusChangedToShippedIntegrationEvent, OrderStatusChangedToShippedIntegrationEventHandler>();
        eventBus.AddSubscription<OrderStatusChangedToCancelledIntegrationEvent, OrderStatusChangedToCancelledIntegrationEventHandler>();
        eventBus.AddSubscription<OrderStatusChangedToSubmittedIntegrationEvent, OrderStatusChangedToSubmittedIntegrationEventHandler>();
    }

    public static void AddAuthenticationServices(this IHostApplicationBuilder builder)
    {
        var services = builder.Services;
        var configuration = builder.Configuration;

        var identityUrl = configuration["IdentityUrl"]
            ?? configuration["PUBLIC_IDENTITY_URL"]
            ?? "http://localhost:8081";

        services.AddAuthorization();

        services.AddAuthentication(options =>
        {
            options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = "oidc";
        })
        .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, options =>
        {
            options.Cookie.Name = "eshopauth";

            options.Cookie.SameSite = SameSiteMode.Lax;

            options.Cookie.SecurePolicy = CookieSecurePolicy.None;

            options.LoginPath = "/";
        })
        .AddOpenIdConnect("oidc", options =>
        {
            options.Authority = identityUrl;

            options.RequireHttpsMetadata = false;

            options.ClientId = "webapp";

            options.ClientSecret = "secret";

            options.ResponseType = "code";

            options.SaveTokens = true;

            options.GetClaimsFromUserInfoEndpoint = true;

            options.Scope.Add("openid");
            options.Scope.Add("profile");
            options.Scope.Add("offline_access");
        });

        services.AddScoped<AuthenticationStateProvider, ServerAuthenticationStateProvider>();

        services.AddCascadingAuthenticationState();
    }

    private static void AddAIServices(this IHostApplicationBuilder builder)
    {
        ChatClientBuilder? chatClientBuilder = null;

        if (builder.Configuration["OllamaEnabled"] is string ollamaEnabled && bool.Parse(ollamaEnabled))
        {
            chatClientBuilder = builder.AddOllamaApiClient("chat")
                .AddChatClient();
        }
        else if (!string.IsNullOrWhiteSpace(builder.Configuration.GetConnectionString("chatModel")))
        {
            chatClientBuilder = builder.AddOpenAIClientFromConfiguration("chatModel")
                .AddChatClient();
        }

        chatClientBuilder?.UseFunctionInvocation();
    }

    public static async Task<string?> GetBuyerIdAsync(this AuthenticationStateProvider authenticationStateProvider)
    {
        return "demo-user";
    }

    public static async Task<string?> GetUserNameAsync(this AuthenticationStateProvider authenticationStateProvider)
    {
        return "Demo User";
    }
}