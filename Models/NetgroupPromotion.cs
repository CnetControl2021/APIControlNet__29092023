using System;
using System.Collections.Generic;

namespace APIControlNet.Models
{
    public partial class NetgroupPromotion
    {
        public int NetgroupPromotionIdx { get; set; }
        public Guid NetgroupPromotionId { get; set; }
        public Guid NetgroupId { get; set; }
        public string Description { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public bool? EnableDay1 { get; set; }
        public bool? EnableDay2 { get; set; }
        public bool? EnableDay3 { get; set; }
        public bool? EnableDay4 { get; set; }
        public bool? EnableDay5 { get; set; }
        public bool? EnableDay6 { get; set; }
        public bool? EnableDay7 { get; set; }
        public DateTime? StartHour1 { get; set; }
        public DateTime? StartHour2 { get; set; }
        public DateTime? EndHour1 { get; set; }
        public DateTime? EndHour2 { get; set; }
        public bool? EnableHour1 { get; set; }
        public bool? EnableHour2 { get; set; }
        public DateTime? Date { get; set; }
        public DateTime? Updated { get; set; }
        public int? Active { get; set; }
        public int? Locked { get; set; }
        public int? Deleted { get; set; }
    }
}
