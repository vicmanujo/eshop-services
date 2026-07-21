using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Catalog.API.Models.Products.GetProductByName
{

    public record GetProductByNameQuery(string Name) : IQuery<GetProductByNameResult>;
    public record GetProductByNameResult(IEnumerable<Product> Products);
    internal class GetProductByNameQueryHandler(IDocumentSession session, ILogger<GetProductByNameQueryHandler> logger)
        : IQueryHandler<GetProductByNameQuery, GetProductByNameResult>
    {
        public async Task<GetProductByNameResult> Handle(GetProductByNameQuery query, CancellationToken cancellationToken)
        {
            logger.LogInformation("GetProductByNameQueryHandler.Handle llamado con {@Query}",query);
            var products = await session.Query<Product>()
                .Where(p => p.Name.Contains(query.Name))
                .ToListAsync(cancellationToken);
            return new GetProductByNameResult(products);
        }
    }
}
