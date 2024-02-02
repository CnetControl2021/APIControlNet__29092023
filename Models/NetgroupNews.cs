using System;
using System.Collections.Generic;

namespace APIControlNet.Models
{
    public partial class NetgroupNews
    {
        public int NetgroupNewsIdx { get; set; }
        public Guid NetgroupId { get; set; }
        public string Description { get; set; }
        public string Photo { get; set; }
        public DateTime? Date { get; set; }
        public DateTime? EndDate { get; set; }
        public DateTime? Updated { get; set; }
        public int? Active { get; set; }
        public int? Locked { get; set; }
        public int? Deleted { get; set; }
    }
}
