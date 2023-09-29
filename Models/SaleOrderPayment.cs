using System;
using System.Collections.Generic;

namespace APIControlNet.Models
{
    public partial class SaleOrderPayment
    {
        public int SaleOrderPaymentIdx { get; set; }
        public Guid SaleOrderId { get; set; }
        public int? SaleOrderPaymentIdi { get; set; }
        public string SatFormaPagoId { get; set; }
        public int? SatFormaSubpagoId { get; set; }
        public Guid? BankCardId { get; set; }
        public string Name { get; set; }
        public decimal? Paid { get; set; }
        public decimal? Received { get; set; }
        public decimal? Reimburse { get; set; }
        public string Reference { get; set; }
        public DateTime? Date { get; set; }
        public DateTime? Updated { get; set; }
        public byte? Active { get; set; }
        public byte? Locked { get; set; }
        public byte? Deleted { get; set; }
        public int? PaymentSubModeIdi { get; set; }
    }
}
