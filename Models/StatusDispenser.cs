using System;
using System.Collections.Generic;

namespace APIControlNet.Models
{
    public partial class StatusDispenser
    {
        public StatusDispenser()
        {
            LoadPositionResponses = new HashSet<LoadPositionResponse>();
        }

        public int StatusDispenserIdx { get; set; }
        public int StatusDispenserIdi { get; set; }
        public string Description { get; set; }
        public DateTime? Date { get; set; }
        public DateTime? Updated { get; set; }
        public bool? Active { get; set; }
        public bool? Locked { get; set; }
        public bool? Deleted { get; set; }
        public string ColorStatus { get; set; }
        public string Logo { get; set; }
        public string LogoName { get; set; }
        public string LogoNameLow { get; set; }
        public string LogoNameHigh { get; set; }

        public virtual ICollection<LoadPositionResponse> LoadPositionResponses { get; set; }
    }
}
