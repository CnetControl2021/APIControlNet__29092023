using System;
using System.Collections.Generic;

namespace APIControlNet.Models
{
    public partial class Port
    {
        public Port()
        {
            LoadPositions = new HashSet<LoadPosition>();
            Tanks = new HashSet<Tank>();
        }

        public int PortIdx { get; set; }
        public Guid StoreId { get; set; }
        public int PortIdi { get; set; }
        public string Name { get; set; }
        public string NameLinux { get; set; }
        public int PortTypeIdi { get; set; }
        public int? BaudRate { get; set; }
        public DateTime? Date { get; set; }
        public int? IsShowTxrx { get; set; }
        public bool? IsWithEcho { get; set; }
        public DateTime? Updated { get; set; }
        public string Description { get; set; }
        public bool? Active { get; set; }
        public bool? Locked { get; set; }
        public bool? Deleted { get; set; }
        public int? Bits { get; set; }
        public int? StopBit { get; set; }
        public string Parity { get; set; }
        public DateTime? MonitorDate { get; set; }
        public int? IsFound { get; set; }

        public virtual PortType PortTypeIdiNavigation { get; set; }
        public virtual Store Store { get; set; }
        public virtual ICollection<LoadPosition> LoadPositions { get; set; }
        public virtual ICollection<Tank> Tanks { get; set; }
    }
}
