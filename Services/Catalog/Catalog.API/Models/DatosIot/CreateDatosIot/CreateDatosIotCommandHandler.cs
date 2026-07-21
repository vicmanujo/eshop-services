using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Catalog.API.Models.DatosIot.CreateDatosIot
{
    public record CreateDatosIotCommand(float Temperatura, float Humedad, DateTime FechaRegistro) 
    : ICommand<CreateDatosIotResult>;

    public record CreateDatosIotResult(Guid Id);
    internal class CreateDatosIotCommandHandler(IDocumentSession documentSession) : ICommandHandler<CreateDatosIotCommand, CreateDatosIotResult>
    {
        public async Task<CreateDatosIotResult> Handle(CreateDatosIotCommand request,
            CancellationToken cancellationToken)
        {
            DatosIot datosIot = new DatosIot
            {
                Temperatura = request.Temperatura,
                Humedad = request.Humedad,
                FechaRegistro = DateTime.UtcNow
            };
            documentSession.Store(datosIot);
            await documentSession.SaveChangesAsync(cancellationToken);
            return new CreateDatosIotResult(Guid.NewGuid());
        }
        
    }
}