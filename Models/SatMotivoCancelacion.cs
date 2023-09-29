using System;
using System.Collections.Generic;

namespace APIControlNet.Models
{
    public partial class SatMotivoCancelacion
    {
        public int SatMotivoCancelacionIdx { get; set; }
        public string SatMotivoCancelacionId { get; set; }
        public string Descripcion { get; set; }
        public DateTime? FechaInicioVigencia { get; set; }
        public DateTime? FechaFinVigencia { get; set; }
        public DateTime? Updated { get; set; }
        public bool? Active { get; set; }
        public bool? Locked { get; set; }
        public bool? Deleted { get; set; }
    }
}
