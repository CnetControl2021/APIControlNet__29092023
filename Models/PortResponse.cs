using System;
using System.Collections.Generic;

namespace APIControlNet.Models
{
    public partial class PortResponse
    {
        public int PortResponseIdx { get; set; }
        public Guid StoreId { get; set; }
        public int PortIdi { get; set; }
        public string DeviceName { get; set; }
        public string DeviceBrand { get; set; }
        public int? CpuAddress { get; set; }
        public int? CpuNumberLoop { get; set; }
        public int? CommPercentage { get; set; }
        public string Response { get; set; }
        public DateTime? Date { get; set; }
        public DateTime? Updated { get; set; }
        public bool? Active { get; set; }
        public bool? Locked { get; set; }
        public bool? Deleted { get; set; }
        public string Response2 { get; set; }
    }
}
