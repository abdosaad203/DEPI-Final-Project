using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace eShop.ServiceDefaults;

/// <summary>
/// Test-only helpers to run OIDC over HTTP (not for production).
/// </summary>
public static class InsecureHttpAuthentication
{
    public static bool IsAllowed(IConfiguration configuration, IHostEnvironment environment) =>
        environment.IsDevelopment()
        || configuration.GetValue("AllowInsecureAuthentication", false)
        || configuration.GetValue("DisableHttpsRedirection", false);

    // public static void ConfigureOpenIdConnect(OpenIdConnectOptions options)
    // {
    //     options.RequireHttpsMetadata = false;
    //     options.CorrelationCookie.SecurePolicy = CookieSecurePolicy.None;
    //     options.CorrelationCookie.SameSite = SameSiteMode.Lax;
    //     options.NonceCookie.SecurePolicy = CookieSecurePolicy.None;
    //     options.NonceCookie.SameSite = SameSiteMode.None;
    // }

    public static void ConfigureCookie(CookieAuthenticationOptions options)
    {
        options.Cookie.SecurePolicy = CookieSecurePolicy.None;
        options.Cookie.SameSite = SameSiteMode.Lax;
    }
}