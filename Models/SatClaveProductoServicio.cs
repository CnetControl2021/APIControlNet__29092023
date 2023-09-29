using System;
using System.Collections.Generic;

namespace APIControlNet.Models
{
    public partial class SatClaveProductoServicio
    {
        public int SatClaveProductoServicioIdx { get; set; }
        public string SatClaveProductoServicioId { get; set; }
        public string Descripcion { get; set; }
        public string IncluirIvaTrasladado { get; set; }
        public string IncluirIepsTrasladado { get; set; }
        public string IncluirComplemento { get; set; }
        public DateTime? FechaInicioVigencia { get; set; }
        public DateTime? FechaFinVigencia { get; set; }
        public string PalabrasSimilares { get; set; }
        public DateTime? Updated { get; set; }
        public bool? Active { get; set; }
        public bool? Locked { get; set; }
        public bool? Deleted { get; set; }
    }
}
