using TestTask_SimpleProxy;
using TestTask_SimpleProxy.Helpers;
using TestTask_SimpleProxy.Middlewares;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<IUriProvider, UriProvider>();
builder.Services.AddScoped<IHtmlContentManager, HtmlContentManager>();
builder.Services.AddHttpClient();

var app = builder.Build();

app.UseHttpsRedirection();

app.UseMiddleware<ProxyMiddleware>();

app.Run();
