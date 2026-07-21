using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Catalog.API.Models.DatosIot
{
    public class DatosIot
    {
        public Guid Id {get; set;}
        public float Temperatura {get; set;}
        public float Humedad {get; set;}

        public DateTime FechaRegistro {get; set;}
    }
}