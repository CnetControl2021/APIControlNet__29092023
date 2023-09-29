using System;
using System.Collections.Generic;

namespace APIControlNet.Models
{
    public partial class DuctoEntregaCfdi
    {
        public int RegistroId { get; set; }
        public string NumeroEstacion { get; set; }
        public int NumeroEntrega { get; set; }
        public string ClienteRfc { get; set; }
        public string NombreCliente { get; set; }
        public string NumeroPermisoCre { get; set; }
        public string Serie { get; set; }
        public int NumeroFactura { get; set; }
        public string TipoCfdi { get; set; }
        public string Cfdi { get; set; }
        public decimal Precio { get; set; }
        public decimal PrecioVtaPublico { get; set; }
        public DateTime? FechaHora { get; set; }
        public decimal Volumen { get; set; }
        public string UnidadMedida { get; set; }
    }
}
