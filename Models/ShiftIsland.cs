using System;
using System.Collections.Generic;

namespace APIControlNet.Models
{
    public partial class ShiftIsland
    {
        public int ShiftIslandIdx { get; set; }
        public Guid? StoreId { get; set; }
        public Guid ShiftIslandId { get; set; }
        public int? IslandIdi { get; set; }
        public decimal? Amount { get; set; }
        public decimal? AmountPayment { get; set; }
        public decimal? Difference { get; set; }
        public DateTime? Date { get; set; }
        public DateTime? Updated { get; set; }
        public bool? Active { get; set; }
        public bool? Locked { get; set; }
        public bool? Deleted { get; set; }
    }
}
