using System;
using System.Collections.Generic;

namespace APIControlNet.Models
{
    public partial class Netgroup
    {
        public int NetgroupIdx { get; set; }
        public Guid NetgroupId { get; set; }
        public int? NetgroupIdi { get; set; }
        public string NetgroupName { get; set; }
        public string ShortDescription { get; set; }
        public string Logo { get; set; }
        public string NetgroupPhone { get; set; }
        public DateTime? Date { get; set; }
        public DateTime? Updated { get; set; }
        public bool? Active { get; set; }
        public bool? Locked { get; set; }
        public bool? Deleted { get; set; }
        public int? IsEnableRaffle { get; set; }
        public Guid? NetgroupRaffleId { get; set; }
        public int? IsEnableReward { get; set; }
        public string VersionApp { get; set; }
    }
}
