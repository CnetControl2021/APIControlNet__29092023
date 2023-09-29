using System;
using System.Collections.Generic;

namespace APIControlNet.Models
{
    public partial class BuyDetail
    {
        public int BuyDetailIdx { get; set; }
        public Guid? BuyId { get; set; }
        public Guid? CustomerId { get; set; }
        public string Description { get; set; }
        public Guid? ProductId { get; set; }
        public decimal? Quantity { get; set; }
        public decimal? Amount { get; set; }
        public decimal? Price { get; set; }
        public decimal? Discount { get; set; }
        public decimal? Subtotal { get; set; }
        public DateTime? Date { get; set; }
        public DateTime? Updated { get; set; }
        public byte? Active { get; set; }
        public byte? Locked { get; set; }
        public byte? Deleted { get; set; }
    }
}
