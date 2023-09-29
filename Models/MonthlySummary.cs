using System;
using System.Collections.Generic;

namespace APIControlNet.Models
{
    public partial class MonthlySummary
    {
        public int MonthlySummaryIdx { get; set; }
        public Guid StoreId { get; set; }
        public DateTime Date { get; set; }
        public Guid ProductId { get; set; }
        public decimal Price { get; set; }
        public decimal StartInventoryQuantity { get; set; }
        public decimal InventoryInQuantity { get; set; }
        public decimal SaleQuantity { get; set; }
        public decimal SaleAmount { get; set; }
        public decimal TheoryInventoryQuantity { get; set; }
        public decimal EndInventoryQuantity { get; set; }
        public decimal InventoryDifference { get; set; }
        public decimal InventoryDifferencePercentage { get; set; }
        public decimal SaleSample { get; set; }
        public DateTime? Updated { get; set; }
        public bool? Active { get; set; }
        public bool? Locked { get; set; }
        public bool? Deleted { get; set; }

        public virtual Product Product { get; set; }
    }
}
