using APIControlNet.Models;

namespace APIControlNet.DTOs
{
    public class DailySummaryDTO
    {
        public int DailySummaryIdx { get; set; }
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
        public string ProductColor { get; set; }
        public string ProductName { get; set; }

        public virtual ProductDTO Product { get; set; }
    }
}
