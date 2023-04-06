using Microsoft.AspNetCore.HttpOverrides;

using FiraServer.api.Middlewares.Authorization;
using FiraServer.DI;

var builder = WebApplication.CreateBuilder(args);

DIService.RegisterDependencies(builder);

var app = builder.Build();

// Configure the HTTP request pipeline.
ForwardedHeadersOptions forwardedHeadersOptions = new ForwardedHeadersOptions
{
    ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
};

app.UseForwardedHeaders(forwardedHeadersOptions);

app.MapControllers();

app.UseOauth2Middleware();

app.Run();
