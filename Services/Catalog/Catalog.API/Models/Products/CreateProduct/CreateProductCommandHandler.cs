using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Mapster;
using Marten;
using BuildingBlocks.CQRS;


namespace Catalog.API.Models.Products.CreateProduct
{

    public record CreateProductCommand(string Name, string Description,List<string> Category,string ImageFiles,decimal Price)
        : ICommand<CreateProductResult>;

    public record CreateProductResult(Guid Id);

    internal class CreatedProductCommandHandler(IDocumentSession documentSession): ICommandHandler<CreateProductCommand, CreateProductResult>
    {
        public async Task<CreateProductResult> Handle(CreateProductCommand request,
            CancellationToken cancellationToken)
        {
            Product product = new Product
            {
                Name = request.Name,
                Description = request.Description,
                Category = request.Category,
                ImageFiles = request.ImageFiles,
                Price = request.Price
            };

            //save to database tenemos que poner otro patron que se encarge de la accion 
            // otro patron por buenas practicas para que se encage el aplicativo de hacer su proceso cancellation
            documentSession.Store(product);
            await documentSession.SaveChangesAsync(cancellationToken);
            return new CreateProductResult(product.Id);
        }
        
    }
}