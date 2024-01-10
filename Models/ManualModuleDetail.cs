using System;
using System.Collections.Generic;

namespace APIControlNet.Models
{
    public partial class ManualModuleDetail
    {
        public int ManualModuleDetailIdx { get; set; }
        public int ManualModuleIdi { get; set; }
        public string ManualName { get; set; }
        public DateTime? Date { get; set; }
        public DateTime? Updated { get; set; }
        public bool? Active { get; set; }
        public bool? Locked { get; set; }
        public bool? Deleted { get; set; }
    }
}
