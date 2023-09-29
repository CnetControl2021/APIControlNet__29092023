using System;
using System.Collections.Generic;

namespace APIControlNet.Models
{
    public partial class Ducto
    {
        public int DuctoId { get; set; }
        public string NumeroEstacion { get; set; }
        public int NumeroDucto { get; set; }
        public string NumeroProducto { get; set; }
        public string TipoDucto { get; set; }
        public string Descripcion { get; set; }
        public decimal? Diametro { get; set; }
        public string UnidadMedida { get; set; }
        public decimal? Volumen { get; set; }
    }
}
