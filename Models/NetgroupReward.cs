using System;
using System.Collections.Generic;

namespace APIControlNet.Models
{
    public partial class NetgroupReward
    {
        public int NetgroupRewardIdx { get; set; }
        public Guid NetgroupId { get; set; }
        public Guid NetgroupRewardId { get; set; }
        public decimal? RewardPoints { get; set; }
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
