using System;
using System.Collections.Generic;

namespace APIControlNet.Models
{
    public partial class ProductPrice
    {
        public int ProductPriceIdx { get; set; }
        public Guid StoreId { get; set; }
        public Guid ProductId { get; set; }
        public DateTime? Date { get; set; }
        public DateTime? ProgrammingDate { get; set; }
        public decimal? Ieps { get; set; }
        public decimal? Price { get; set; }
        public bool? IsImmediate { get; set; }
        public DateTime? ApplicationDate { get; set; }
        public bool? IsCancelled { get; set; }
        public bool? IsApplied { get; set; }
        public string UserId { get; set; }
        public DateTime? Updated { get; set; }
        public bool? Active { get; set; }
        public bool? Locked { get; set; }
        public bool? Deleted { get; set; }
    }
}
