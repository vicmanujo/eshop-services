using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Catalog.API.Models.DatosIot.GetDatodIot
{
    public record GetDatosIotResponse(IEnumerable<DatosIot> DatosIot);

    public class GetDatosIotEndPoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet("/datosIot", async (ISender sender) =>
            {
                var result = await sender.Send(new GetDatosIotQuery());
                var response = result.Adapt<GetDatosIotResponse>();
                return Results.Ok(response);
            })
            .WithName("GetDatos iot")
            .Produces<GetDatosIotResponse>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .WithSummary("Get Resumen")
            .WithDescription("Get Resumen");
        }
    }
}