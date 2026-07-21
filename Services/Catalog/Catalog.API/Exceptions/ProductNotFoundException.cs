using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Catalog.API.Exceptions
{
    public class ProductNotFoundException: Exception
    {
        public ProductNotFoundException():base("Producto no encontrado")
        {
            
        }     
    }
}