using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Catalog.API.Models.Products.GetProducts
{
    public record GetProductsRequets(int? PageNumber =1, int? PageSize = 10);
    public record GetProductsResponse(IEnumerable<Product> Products);
    public class GetProductsEndPoint :ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet("/products", async ([AsParameters] GetProductsRequets requets ,ISender sender) =>
            {
                var query = new GetProductsQuery(requets.PageNumber ?? 1, requets.PageSize??10);
                var result = await sender.Send(query);
                var response = result.Adapt<GetProductsResponse>();
                return Results.Ok(response);
            })
            .WithName("GetProducts")
            .Produces<GetProductsResponse>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .WithSummary("Get Resumen")
            .WithDescription("Get Resumen");
        }
        
    }
}