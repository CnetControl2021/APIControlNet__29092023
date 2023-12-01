using System;
using System.Collections.Generic;

namespace APIControlNet.Models
{
    public partial class NetgroupBalance
    {
        public int NetgroupBalanceIdx { get; set; }
        public Guid NetgroupId { get; set; }
        public Guid NetgroupBalanceId { get; set; }
        public decimal? RewardPoints { get; set; }
        public int? ItsAReward { get; set; }
        public Guid? NetgroupRewardId { get; set; }
        public DateTime? Date { get; set; }
        public DateTime? EndDate { get; set; }
        public DateTime? Updated { get; set; }
        public int? Active { get; set; }
        public int? Locked { get; set; }
        public int? Deleted { get; set; }
    }
}
