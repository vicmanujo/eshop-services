using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Catalog.API.Models.DatosIot.CreateDatosIot
{

    public record CreateDatosIotRequest(float Temperatura, float Humedad, DateTime FechaRegistro);
    public record CreateDatosIotResponse(Guid Id);
    public class CreateDatosIotEndpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPost("/datosIot",async (CreateDatosIotRequest request, ISender sender) =>
            {
                var command = request.Adapt<CreateDatosIotCommand>();
                var result = await sender.Send(command);
                var response = result.Adapt<CreateDatosIotResponse>();
                return Results.Created($"/datosIot/{response.Id}", response);
            })
            .WithName("CrearDatosIot")
            .Produces<CreateDatosIotResponse>(StatusCodes.Status201Created)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .WithSummary("Crear un nuevo Dato Iot")
            .WithDescription("Crea un nuevo registro de datos iot y se retorna el identificador de la identidad");
        }
    }
}