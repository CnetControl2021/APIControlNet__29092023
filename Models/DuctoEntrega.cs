using System;
using System.Collections.Generic;

namespace APIControlNet.Models
{
    public partial class DuctoEntrega
    {
        public int EntregaId { get; set; }
        public string NumeroEstacion { get; set; }
        public int NumeroEntrega { get; set; }
        public int NumeroDucto { get; set; }
        public string TipoDistribucion { get; set; }
        public string NumeroProducto { get; set; }
        public string UnidadMedida { get; set; }
        public decimal VolumenPuntoSalida { get; set; }
        public decimal VolumenEntregado { get; set; }
        public decimal VolumenFinal { get; set; }
        public decimal Temperatura { get; set; }
        public decimal PresionAbsoluta { get; set; }
        public DateTime? FechaYhoraInicialEntrega { get; set; }
        public DateTime? FechaYhoraFinalEntrega { get; set; }
        public decimal? Precio { get; set; }
        public decimal? Importe { get; set; }
        public decimal? PoderCalorifico { get; set; }
    }
}
