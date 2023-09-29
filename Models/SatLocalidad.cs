using System;
using System.Collections.Generic;

namespace APIControlNet.Models
{
    public partial class SatLocalidad
    {
        public int SatLocalidadIdx { get; set; }
        public string SatLocalidadClave { get; set; }
        public string SatEstadoId { get; set; }
        public string SatLocalidadId { get; set; }
        public string Descripcion { get; set; }
        public DateTime? Date { get; set; }
        public DateTime? Updated { get; set; }
        public bool? Active { get; set; }
        public bool? Locked { get; set; }
        public bool? Deleted { get; set; }
    }
}
