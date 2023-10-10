using System;
using System.Collections.Generic;

namespace APIControlNet.Models
{
    public partial class Voucher
    {
        public int VoucherIdx { get; set; }
        public Guid? StoreNetworkId { get; set; }
        public Guid? VoucherId { get; set; }
        public int VoucherNumber { get; set; }
        public decimal? Amount { get; set; }
        public decimal? AmountSpent { get; set; }
        public Guid? InvoiceId { get; set; }
        public Guid? SaleOrderId { get; set; }
        public int? IsConsumed { get; set; }
        public string UserName { get; set; }
        public DateTime? DateConsumed { get; set; }
        public DateTime? DateExpiration { get; set; }
        public DateTime? Date { get; set; }
        public DateTime? Updated { get; set; }
        public bool? Active { get; set; }
        public bool? Locked { get; set; }
        public bool? Deleted { get; set; }
    }
}
