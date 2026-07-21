using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Catalog.API.Models.Products.GetProductByName
{
    public record GetProductByNameResponse(IEnumerable<Product> Products);

    public class GetProductByNameEndPoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet("/products/name/{name}",async (string name, ISender sender) =>
            {
                var result = await sender.Send(new GetProductByNameQuery(name));
                var response = result.Adapt<GetProductByNameResponse>();
                return Results.Ok(response);
            })
            .WithName("GetProductByName")
            .Produces<GetProductByNameResponse>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .WithSummary("Buscar Producto por Nombre")
            .WithDescription("Obtiene una lista de productos cuyo nombre coincida con el texto buscado.");
        }
    }
}