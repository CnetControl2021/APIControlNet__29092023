using System;
using System.Collections.Generic;

namespace APIControlNet.Models
{
    public partial class SatErrorCancelacion
    {
        public int SatErrorCancelacionId { get; set; }
        public string Description { get; set; }
        public string Message { get; set; }
        public DateTime? Updated { get; set; }
        public bool? Active { get; set; }
        public bool? Locked { get; set; }
        public bool? Deleted { get; set; }
    }
}
