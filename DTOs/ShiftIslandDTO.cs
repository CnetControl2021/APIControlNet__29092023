using System;
using System.Collections.Generic;

namespace APIControlNet.DTOs
{
    public class ShiftIslandDTO
    {
        public int ShiftIslandIdx { get; set; }
        public Guid? StoreId { get; set; }
        public Guid ShiftIslandId { get; set; }
        public int? IslandIdi { get; set; }
        public decimal? Amount { get; set; }
        public decimal? AmountPayment { get; set; }
        public decimal? Difference { get; set; }
    }
}
