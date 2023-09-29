using System;
using System.Collections.Generic;

namespace APIControlNet.Models
{
    public partial class DuctoRecepcionTransporte
    {
        public int TransporteId { get; set; }
        public string NumeroEstacion { get; set; }
        public int NumeroRecepcion { get; set; }
        public string PermisoTrasporte { get; set; }
        public string ClaveVehiculo { get; set; }
        public decimal TarifaTransporte { get; set; }
        public decimal CargoCapacidadTrans { get; set; }
        public decimal CargoUsoTrans { get; set; }
        public decimal CargoVolumenTrans { get; set; }
        public decimal? TarifaSumnistro { get; set; }
        public decimal? ContraPrestacion { get; set; }
        public decimal? Descuento { get; set; }
    }
}
