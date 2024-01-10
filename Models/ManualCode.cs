using System;
using System.Collections.Generic;

namespace APIControlNet.Models
{
    public partial class ManualCode
    {
        public int ManualCodeIdx { get; set; }
        public string ManualCodeId { get; set; }
        public string Description { get; set; }
        public DateTime? Date { get; set; }
        public DateTime? Updated { get; set; }
        public bool? Active { get; set; }
        public bool? Locked { get; set; }
        public bool? Deleted { get; set; }
    }
}
