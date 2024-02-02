using System;
using System.Collections.Generic;

namespace APIControlNet.Models
{
    public partial class NetgroupPromotionDetail
    {
        public int NetgroupPromotionDetailIdx { get; set; }
        public Guid NetgroupPromotionId { get; set; }
        public Guid ProductId { get; set; }
        public string Description { get; set; }
        public decimal? PointValue { get; set; }
        public DateTime? Date { get; set; }
        public DateTime? Updated { get; set; }
        public int? Active { get; set; }
        public int? Locked { get; set; }
        public int? Deleted { get; set; }
    }
}
