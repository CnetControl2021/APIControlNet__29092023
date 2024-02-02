using System;
using System.Collections.Generic;

namespace APIControlNet.Models
{
    public partial class NetgroupRewardProduct
    {
        public int NetgroupRewardProductIdx { get; set; }
        public Guid NetgroupId { get; set; }
        public Guid ProductId { get; set; }
        public int? PresetType { get; set; }
        public decimal? PointValue { get; set; }
        public string Description { get; set; }
        public DateTime? Date { get; set; }
        public DateTime? Updated { get; set; }
        public int? Active { get; set; }
        public int? Locked { get; set; }
        public int? Deleted { get; set; }
    }
}
