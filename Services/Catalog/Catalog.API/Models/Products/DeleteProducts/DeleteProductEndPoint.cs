using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Catalog.API.Models.Products.DeleteProducts
{
    public record DeleteProductResponse(bool IsSuccess);
    public class DeleteProductEndPoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapDelete("/products/{id:guid}", async(Guid id,ISender sender) =>
            {
                var result = await sender.Send(new DeleteProductCommand(id));
                var response = result.Adapt<DeleteProductResponse>();
                return Results.Ok(response);
            })
            .WithName("DeleteProduct")
            .Produces<DeleteProductResponse>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .WithSummary("Eliminar producto")
            .WithDescription("Elimina un producto existente usando su Id único");
            
        }  
    }
}