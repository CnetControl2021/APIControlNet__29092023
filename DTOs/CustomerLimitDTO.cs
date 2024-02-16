namespace APIControlNet.DTOs
{
    public class CustomerLimitDTO
    {
        public int CustomerLimitIdx { get; set; }
        public Guid CustomerId { get; set; }
        public decimal? AmountCreditLimit { get; set; }
        public int? CreditDays { get; set; }
        public bool? InvoiceDay1 { get; set; }
        public bool? InvoiceDay2 { get; set; }
        public bool? InvoiceDay3 { get; set; }
        public bool? InvoiceDay4 { get; set; }
        public bool? InvoiceDay5 { get; set; }
        public bool? InvoiceDay6 { get; set; }
        public bool? InvoiceDay7 { get; set; }
        public bool? PaymentDay1 { get; set; }
        public bool? PaymentDay2 { get; set; }
        public bool? PaymentDay3 { get; set; }
        public bool? PaymentDay4 { get; set; }
        public bool? PaymentDay5 { get; set; }
        public bool? PaymentDay6 { get; set; }
        public bool? PaymentDay7 { get; set; }
        public bool? AllowDay1 { get; set; }
        public bool? AllowDay2 { get; set; }
        public bool? AllowDay3 { get; set; }
        public bool? AllowDay4 { get; set; }
        public bool? AllowDay5 { get; set; }
        public bool? AllowDay6 { get; set; }
        public bool? AllowDay7 { get; set; }
        public DateTime? Date { get; set; }
        public DateTime? Updated { get; set; } 
        public bool? Active { get; set; }
        public bool? Locked { get; set; }
        public bool? Deleted { get; set; }
        public int? FolioOdrNumber { get; set; }
    }
}
