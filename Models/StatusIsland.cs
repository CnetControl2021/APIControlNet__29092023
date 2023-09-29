using System;
using System.Collections.Generic;

namespace APIControlNet.Models
{
    public partial class StatusIsland
    {
        public int StatusIslandIdx { get; set; }
        public int StatusRun { get; set; }
        public string Description { get; set; }
        public string LogoNameLow { get; set; }
        public string LogoNameHigh { get; set; }
        public string ColorStatus { get; set; }
        public DateTime? Date { get; set; }
        public DateTime? Updated { get; set; }
        public bool? Active { get; set; }
        public bool? Locked { get; set; }
        public bool? Deleted { get; set; }
    }
}
