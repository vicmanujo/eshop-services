using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Catalog.API.Models.DatosIot.GetDatodIot
{
    public record GetDatosIotQuery() : IQuery<GetDatosIotResult>;
    public record GetDatosIotResult(IEnumerable<DatosIot> datosIot);
    internal class GetDatosIotQueryHandler
        (IDocumentSession session, ILogger<GetDatosIotQueryHandler>logger)
        : IQueryHandler<GetDatosIotQuery, GetDatosIotResult>
    {
        public async Task<GetDatosIotResult> Handle(GetDatosIotQuery query,
            CancellationToken cancellationToken)
        {
            logger.LogInformation("GetDatosIotHandler.Handle llamado co {@Query}",query);
            var datosIot = await session.Query<DatosIot>().ToListAsync(cancellationToken);
            return new GetDatosIotResult(datosIot);
        }
    }
}