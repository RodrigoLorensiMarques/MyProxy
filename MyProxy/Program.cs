using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "MyProxy API",
        Version = "v1",
        Description = "Server Proxy Manager"
    });
});


builder.Services.AddHostedService<ProxyListener>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "MyProxy API V1");
    });
}

app.UseHttpsRedirection();

app.MapGet("/", () => "Live")
   .WithName("Home")
   .WithOpenApi(); 

app.Run();
