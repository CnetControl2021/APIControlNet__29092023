using System;
using System.Collections.Generic;

namespace APIControlNet.Models
{
    public partial class InvoiceDetail
    {
        public int InvoiceDetailIdx { get; set; }
        public Guid? InvoiceId { get; set; }
        public int? InvoiceDetailIdi { get; set; }
        public Guid? ProductId { get; set; }
        public decimal? Quantity { get; set; }
        public decimal? Price { get; set; }
        public decimal? Subtotal { get; set; }
        public decimal? Tax { get; set; }
        public decimal? Ieps { get; set; }
        public decimal? Isr { get; set; }
        public decimal? AmountTax { get; set; }
        public decimal? AmountIeps { get; set; }
        public decimal? Amount { get; set; }
        public DateTime? Date { get; set; }
        public DateTime? Updated { get; set; }
        public byte? Active { get; set; }
        public byte? Locked { get; set; }
        public byte? Deleted { get; set; }
        public string Name { get; set; }
    }
}
