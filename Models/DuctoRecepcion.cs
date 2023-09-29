using System;
using System.Collections.Generic;

namespace APIControlNet.Models
{
    public partial class DuctoRecepcion
    {
        public int RecepcionId { get; set; }
        public string NumeroEstacion { get; set; }
        public int NumeroRecepcion { get; set; }
        public int NumeroDucto { get; set; }
        public string TipoDistribucion { get; set; }
        public string NumeroProducto { get; set; }
        public string UnidadMedida { get; set; }
        public decimal VolumenPuntoEntrada { get; set; }
        public decimal VolumenRecepcion { get; set; }
        public decimal VolumenFinal { get; set; }
        public decimal Temperatura { get; set; }
        public decimal PresionAbsoluta { get; set; }
        public DateTime? FechaYhoraInicioRecepcion { get; set; }
        public DateTime? FechaYhoraFinalRecepcion { get; set; }
        public decimal? Precio { get; set; }
        public decimal? Importe { get; set; }
        public decimal? PoderCalorifico { get; set; }
    }
}
