using System;
using System.Collections.Generic;

namespace APIControlNet.Models
{
    public partial class ComposicionGasNaturalOcondensado
    {
        public int ComposicionId { get; set; }
        public string NumeroEstacion { get; set; }
        public string NumeroProducto { get; set; }
        public string TipoCompuesto { get; set; }
        public decimal FraccionMolar { get; set; }
        public decimal PoderCalorifico { get; set; }
    }
}
