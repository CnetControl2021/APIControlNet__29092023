using System;
using System.Collections.Generic;

namespace APIControlNet.Models
{
    public partial class PortType
    {
        public PortType()
        {
            Ports = new HashSet<Port>();
        }

        public int PortTypeIdx { get; set; }
        public int PortTypeIdi { get; set; }
        public string Name { get; set; }
        public DateTime? Date { get; set; }
        public DateTime? Updated { get; set; }
        public bool? Active { get; set; }
        public bool? Locked { get; set; }
        public bool? Deleted { get; set; }

        public virtual ICollection<Port> Ports { get; set; }
    }
}
