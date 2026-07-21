using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Catalog.API.Models.Products
{
    public class Product
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; } = default;
        public List<string> Category { get; set; } = new ();
        public string ImageFiles { get; set; } = default;
        public decimal Price { get; set; }
        
    }
}