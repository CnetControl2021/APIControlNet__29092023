using System;
using System.Collections.Generic;

namespace APIControlNet.Models
{
    public partial class DuctoEntregaPedimento
    {
        public int PedimentoId { get; set; }
        public string NumeroEstacion { get; set; }
        public int NumeroEntrega { get; set; }
        public string ClavePermisoImportOexport { get; set; }
        public int PuntoInternacionOextracccion { get; set; }
        public string Pais { get; set; }
        public int MedioIngresoOsalida { get; set; }
        public string ClavePedimento { get; set; }
        public string Incoterms { get; set; }
        public decimal PrecioDeImportOexport { get; set; }
        public decimal Volumen { get; set; }
        public string UnidadMedida { get; set; }
    }
}
