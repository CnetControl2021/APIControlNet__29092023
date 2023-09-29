using System;
using System.Collections.Generic;

namespace APIControlNet.Models
{
    public partial class NetgroupPrice
    {
        public int NetgroupPriceIdx { get; set; }
        public Guid NetgroupId { get; set; }
        public Guid NetgroupPriceId { get; set; }
        public decimal? Discount { get; set; }
        public string Description { get; set; }
        public DateTime? Date { get; set; }
        public DateTime? Updated { get; set; }
        public bool? Active { get; set; }
        public bool? Locked { get; set; }
        public bool? Deleted { get; set; }
    }
}
