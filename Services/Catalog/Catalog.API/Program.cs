var builder = WebApplication.CreateBuilder(args);


//se dan permisos al pueto de vue que es el 5173
builder.Services.AddCors(option =>
{
    option.AddPolicy("PermisoParaVue", policy =>
    {
       policy.WithOrigins("http://localhost:5173")
            .AllowAnyHeader()
            .AllowAnyMethod();
    });   
});

// Capturamos el ensamblado actual para reusarlo
var assembly = typeof(Program).Assembly;

// 1. Forzar a MediatR a usar el ensamblado correcto
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(assembly));

// Forzar a Carter a escanear este mismo ensamblado
builder.Services.AddCarter(configurator: config => 
{
    // Esto le dice a Carter que busque en todo tu proyecto de Catalog.API
    config.WithModule<Catalog.API.Models.Products.CreateProduct.CreateProductEndpoint>(); 
    config.WithModule<Catalog.API.Models.Products.GetProducts.GetProductsEndPoint>(); 
    config.WithModule<Catalog.API.Models.DatosIot.CreateDatosIot.CreateDatosIotEndpoint>(); 
    config.WithModule<Catalog.API.Models.DatosIot.GetDatodIot.GetDatosIotEndPoint>(); 

    config.WithModule<Catalog.API.Models.Products.UpdateProducts.UpdateProductEndPoint>();
    config.WithModule<Catalog.API.Models.Products.GetProductByName.GetProductByNameEndPoint>();
    // Asegúrate de que el namespace y el nombre de la clase de Delete coincidan con los tuyos
    config.WithModule<Catalog.API.Models.Products.DeleteProducts.DeleteProductEndPoint>();
});

// Configuración de Marten (Usa Postgres)
// Configuración de Marten leyendo desde appsettings.json
builder.Services.AddMarten(opts =>
{
    opts.Connection(builder.Configuration.GetConnectionString("CatalogDb")!);
}).UseLightweightSessions();

var app = builder.Build();
app.UseCors("PermisoParaVue");

// Mapeo de rutas
app.MapCarter();

app.MapGet("/", () => "¡Mi API de Ubuntu está viva y conectada!");
app.MapPost("/test", () => "¡Kestrel sí acepta POST y Docker compiló bien!");

app.Run();