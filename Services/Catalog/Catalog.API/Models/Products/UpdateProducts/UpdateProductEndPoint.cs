using Carter;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Mvc; // <-- Agregamos esta librería para forzar la lectura del Body

namespace Catalog.API.Models.Products.UpdateProducts
{
    public record UpdateProductRequest(Guid Id, string Name, List<string> Category, 
        string Description, string ImageFiles, decimal Price);

    public record UpdateProductResponse(bool IsSuccess);

    public class UpdateProductEndPoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            // 🕵️‍♂️ RUTA DE DIAGNÓSTICO: Si esto responde, Carter SÍ está leyendo el archivo.
            app.MapGet("/products/test-update", () => 
                Results.Ok(new { message = "¡El archivo de Actualización está vivo y funcionando!" }));

            // RUTA PUT ORIGINAL (Con atributos explícitos para evitar confusiones de .NET)
            app.MapPut("/products", async ([FromBody] UpdateProductRequest request, [FromServices] ISender sender) =>
            {
                var command = request.Adapt<UpdateProductCommand>();
                var result = await sender.Send(command);
                var response = result.Adapt<UpdateProductResponse>();
                return Results.Ok(response);
            })
            .WithName("UpdateProduct")
            .Produces<UpdateProductResponse>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .WithSummary("Update producto")
            .WithDescription("Actualizacion del producto");
        }
    }
}