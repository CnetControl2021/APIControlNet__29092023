using System;
using System.Collections.Generic;

namespace APIControlNet.Models
{
    public partial class NetgroupNetDetail
    {
        public int NetgroupNetDetailIdx { get; set; }
        public Guid NetgroupNetId { get; set; }
        public Guid StoreId { get; set; }
        public DateTime? Date { get; set; }
        public DateTime? Updated { get; set; }
        public bool? Active { get; set; }
        public bool? Locked { get; set; }
        public bool? Deleted { get; set; }
    }
}
