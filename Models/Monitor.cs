using System;
using System.Collections.Generic;

namespace APIControlNet.Models
{
    public partial class Monitor
    {
        public int MonitorIdx { get; set; }
        public Guid? StoreId { get; set; }
        public int? PortTypeIdi { get; set; }
        public string Address { get; set; }
        public string Request { get; set; }
        public string Response { get; set; }
        public string Status { get; set; }
        public DateTime? Date { get; set; }
        public DateTime? Updated { get; set; }
        public byte? Active { get; set; }
        public byte? Locked { get; set; }
        public byte? Deleted { get; set; }
    }
}
