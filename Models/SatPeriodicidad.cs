using System;
using System.Collections.Generic;

namespace APIControlNet.Models
{
    public partial class SatPeriodicidad
    {
        public int SatPeriodicidadIdx { get; set; }
        public string SatPeriodicidadId { get; set; }
        public string Descripcion { get; set; }
        public DateTime? Date { get; set; }
        public DateTime? Updated { get; set; }
        public bool? Active { get; set; }
        public bool? Locked { get; set; }
        public bool? Deleted { get; set; }
    }
}
