using eShop.WebApp.Components;
using eShop.WebApp.Extensions;
using eShop.ServiceDefaults;
using Microsoft.AspNetCore.Http;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.AddApplicationServices();

builder.Services.ConfigureApplicationCookie(options =>
{
    options.Cookie.SameSite = SameSiteMode.Lax;
    options.Cookie.SecurePolicy = CookieSecurePolicy.None;
});

var app = builder.Build();

app.MapDefaultEndpoints();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

// تعطيل HTTPS مؤقتًا علشان OIDC
// app.UseHttpsRedirection();

app.UseCookiePolicy(new CookiePolicyOptions
{
    MinimumSameSitePolicy = SameSiteMode.Lax,
    Secure = CookieSecurePolicy.None
});

app.UseStaticFiles();

app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

var catalogForwarder =
    app.Configuration["ServiceUrls:catalog-api-forwarder"]
    ?? (string.Equals(
        Environment.GetEnvironmentVariable("DOTNET_RUNNING_IN_CONTAINER"),
        "true",
        StringComparison.OrdinalIgnoreCase)
        ? "http://catalog-api:8080"
        : "https+http://catalog-api");

app.MapForwarder(
    "/product-images/{id}",
    catalogForwarder,
    "/api/catalog/items/{id}/pic");

app.Run();