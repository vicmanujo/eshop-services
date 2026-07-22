using System.Data.Common;
using BuildingBlocks.Behaviors;
using BuildingBlocks.Exceptions.Handler;
using HealthChecks.UI.Client;
using Marten;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Carter;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddCors(option =>
{
    option.AddPolicy("PermisoParaVue", policy =>
    {
       policy.WithOrigins("http://localhost:5173", "https://*.onrender.com")
            .AllowAnyHeader()
            .AllowAnyMethod();
    });   
});

var assembly = typeof(Program).Assembly;

// Carter detecta automáticamente los módulos en el ensamblado actual
builder.Services.AddCarter(configurator: config => 
{
    config.WithModule<Basket.API.Basket.StoreBasket.StoreBasketEndPoint>();
    config.WithModule<Basket.API.Basket.GetBasket.GetBasketEndPoints>();
    config.WithModule<Basket.API.Basket.DeleteBasket.DeleteBasketEndPoints>();

});

builder.Services.AddMediatR(config =>
{
    config.RegisterServicesFromAssembly(assembly);
    config.AddOpenBehavior(typeof(ValidationBehaviors<,>));
    config.AddOpenBehavior(typeof(LogginBehavior<,>));
});

builder.Services.AddMarten(opt =>
{
    opt.Connection(builder.Configuration.GetConnectionString("Database")!);
    opt.Schema.For<ShoppingCart>().Identity(x => x.UserName); 
}).UseLightweightSessions();

builder.Services.AddScoped<IBasketRepository, BasketRepository>();
builder.Services.Decorate<IBasketRepository, CachedBasketRepository>();
builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = builder.Configuration.GetConnectionString("Redis");
});

builder.Services.AddExceptionHandler<CustomExceptionHandler>();

builder.Services.AddHealthChecks()
    .AddNpgSql(builder.Configuration.GetConnectionString("Database")!)
    .AddRedis(builder.Configuration.GetConnectionString("Redis")!);


builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()    // Permite peticiones desde cualquier origen (como tu Netlify)
              .AllowAnyHeader()    // Permite cualquier tipo de encabezado
              .AllowAnyMethod();   // Permite GET, POST, PUT, DELETE, etc.
    });
});
var app = builder.Build();
app.UseCors("AllowAll");

app.MapCarter();

app.UseExceptionHandler(options => {});

app.UseHealthChecks("/health", new HealthCheckOptions
{
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
});

// Agrega esto justo antes de app.Run(); para ver en los logs de Render qué rutas se mapearon
foreach (var endpoint in app.Urls)
{
    Console.WriteLine($"URL disponible: {endpoint}");
}
app.MapGet("/test-basket", () => "¡El microservicio de Basket está vivo y enrutando!");

app.Run();