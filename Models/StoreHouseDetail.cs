using System;
using System.Collections.Generic;

namespace APIControlNet.Models
{
    public partial class StoreHouseDetail
    {
        public int StoreHouseDetailsIdx { get; set; }
        public string StoreHouseIdDestination { get; set; }
        public string ProductId { get; set; }
        public decimal? QuantityEntry { get; set; }
        public decimal? QuantityExit { get; set; }
        public decimal? Quantity { get; set; }
        public DateTime? PeriodStart { get; set; }
        public DateTime? PeriodEnd { get; set; }
        public DateTime? Date { get; set; }
        public DateTime? Updated { get; set; }
        public bool? Active { get; set; }
        public bool? Locked { get; set; }
        public bool? Deleted { get; set; }
    }
}
