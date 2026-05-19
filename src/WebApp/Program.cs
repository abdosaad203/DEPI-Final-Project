﻿using eShop.WebApp.Components;
using eShop.ServiceDefaults;
using Microsoft.AspNetCore.HttpOverrides;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

builder.Services.AddRazorComponents().AddInteractiveServerComponents();

builder.AddApplicationServices();

var app = builder.Build();

app.MapDefaultEndpoints();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

// مهم جدًا
app.UseForwardedHeaders(new ForwardedHeadersOptions
{
    ForwardedHeaders =
        ForwardedHeaders.XForwardedFor |
        ForwardedHeaders.XForwardedProto
});

if (!app.Configuration.GetValue("DisableHttpsRedirection", false)
    && !app.Configuration.GetValue("AllowInsecureAuthentication", false))
{
    app.UseHttpsRedirection();
}

app.UseStaticFiles();

app.UseRouting();

// مهم جدًا
app.UseCookiePolicy(new CookiePolicyOptions
{
    MinimumSameSitePolicy = SameSiteMode.Unspecified
});

app.UseAuthentication();
app.UseAuthorization();

app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

var catalogForwarder = app.Configuration["ServiceUrls:catalog-api-forwarder"]
    ?? (string.Equals(Environment.GetEnvironmentVariable("DOTNET_RUNNING_IN_CONTAINER"), "true", StringComparison.OrdinalIgnoreCase)
        ? "http://catalog-api:8080"
        : "https+http://catalog-api");

app.MapForwarder("/product-images/{id}", catalogForwarder, "/api/catalog/items/{id}/pic");

app.Run();