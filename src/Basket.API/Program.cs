var builder = WebApplication.CreateBuilder(args);
builder.WebHost.ConfigureKestrel(options =>
{
    options.ListenAnyIP(8080, listenOptions =>
    {
        listenOptions.Protocols =
            Microsoft.AspNetCore.Server.Kestrel.Core.HttpProtocols.Http2;
    });
});
builder.AddBasicServiceDefaults();
builder.AddApplicationServices();

builder.Services.AddGrpc();

var app = builder.Build();

app.MapDefaultEndpoints();

app.MapGrpcService<BasketService>();

app.Run();
