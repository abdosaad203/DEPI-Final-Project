using eShop.WebApp.Components;
using eShop.ServiceDefaults;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

builder.Services.AddRazorComponents().AddInteractiveServerComponents();

builder.AddApplicationServices();

var app = builder.Build();

app.MapDefaultEndpoints();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseAntiforgery();

if (!app.Configuration.GetValue("DisableHttpsRedirection", false))
{
    app.UseHttpsRedirection();
}

app.UseStaticFiles();

app.MapRazorComponents<App>().AddInteractiveServerRenderMode();

var catalogForwarder = app.Configuration["ServiceUrls:catalog-api-forwarder"]
    ?? (string.Equals(Environment.GetEnvironmentVariable("DOTNET_RUNNING_IN_CONTAINER"), "true", StringComparison.OrdinalIgnoreCase)
        ? "http://catalog-api:8080"
        : "https+http://catalog-api");
app.MapForwarder("/product-images/{id}", catalogForwarder, "/api/catalog/items/{id}/pic");

app.Run();
