using System;
using System.Collections.Generic;

namespace APIControlNet.Models
{
    public partial class MovementType
    {
        public int MovementTypeIdx { get; set; }
        public string MovementTypeId { get; set; }
        public string CompanyId { get; set; }
        public string MovementName { get; set; }
        public int? MovementCounter { get; set; }
        public int? MotionEffect { get; set; }
        public DateTime? Date { get; set; }
        public DateTime? Updated { get; set; }
        public bool? Active { get; set; }
        public bool? Locked { get; set; }
        public bool? Deleted { get; set; }
    }
}
