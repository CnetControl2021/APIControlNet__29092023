using System;
using System.Collections.Generic;

namespace APIControlNet.Models
{
    public partial class ManualModule
    {
        public int ManualModuleIdx { get; set; }
        public int ManualModuleIdi { get; set; }
        public string ManualModuleName { get; set; }
        public DateTime? Date { get; set; }
        public DateTime? Updated { get; set; }
        public bool? Active { get; set; }
        public bool? Locked { get; set; }
        public bool? Deleted { get; set; }
    }
}
