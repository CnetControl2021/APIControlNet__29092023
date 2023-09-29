using System;
using System.Collections.Generic;

namespace APIControlNet.Models
{
    public partial class TankType
    {
        public int TankTypeIdx { get; set; }
        public string TankTypeId { get; set; }
        public string Description { get; set; }
        public DateTime? Date { get; set; }
        public DateTime? Updated { get; set; }
        public bool? Active { get; set; }
        public bool? Locked { get; set; }
        public bool? Deleted { get; set; }
    }
}
