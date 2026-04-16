using ApiTokenAzureStorage.Services;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddTransient<ServiceSaSToken>();

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.MapOpenApi();
app.MapScalarApiReference();

app.MapGet("/", context =>
{
    context.Response.Redirect("/scalar");
    return Task.CompletedTask;
});

app.UseHttpsRedirection();

//NECESIAMOS MAPEAR NUESTRO METODO TOKEN DENTRO DE UN ENDPOINT
//app.MapGet("/vacio", () =>
//{
//    return "Metodo vacio";
//}).WithName("MetodoVacio");

app.MapGet("/token/{curso}", (string curso, ServiceSaSToken service) =>
{
    string token = service.GenerateToken(curso);
    return new { token = token };
}).WithName("TokenCurso");

app.Run();

internal record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
