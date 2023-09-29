using System;
using System.Collections.Generic;

namespace APIControlNet.Models
{
    public partial class InvoiceComparison
    {
        public int InvoiceComparisonIdx { get; set; }
        public Guid? CompanyId { get; set; }
        public Guid? StoreId { get; set; }
        public int? IsIssued { get; set; }
        public string InvoiceSerieId { get; set; }
        public string Folio { get; set; }
        public DateTime? Date { get; set; }
        public Guid? InvoiceId { get; set; }
        public string Uuid { get; set; }
        public string CustomerName { get; set; }
        public string CustomerRfc { get; set; }
        public decimal? Subtotal { get; set; }
        public decimal? AmountTax { get; set; }
        public decimal? AmountIeps { get; set; }
        public decimal? AmountIsr { get; set; }
        public decimal? Amount { get; set; }
        public string InvoiceApplicationTypeId { get; set; }
        public bool? CnetIsCancelled { get; set; }
        public bool? CnetIsStamped { get; set; }
        public bool? CnetIsStampedCancelled { get; set; }
        public bool? CnetEnable { get; set; }
        public bool? SatIsCancelled { get; set; }
        public bool? SatIsStamped { get; set; }
        public bool? SatIsStampedCancelled { get; set; }
        public bool? SatEnable { get; set; }
        public string SatTipoComprobanteId { get; set; }
        public DateTime? Updated { get; set; }
        public bool? Active { get; set; }
        public bool? Locked { get; set; }
        public bool? Deleted { get; set; }
        public string CodigoStatus { get; set; }
        public string EsCancelable { get; set; }
        public string Estado { get; set; }
        public string ValidacionEfos { get; set; }
        public string EstatusCancelacion { get; set; }
        public DateTime? ValidateDate { get; set; }
        public int? EnableValidateSat { get; set; }
        public string Name { get; set; }
        public string Rfc { get; set; }
        public string SatCustomerRfc { get; set; }
        public int? Type { get; set; }
        public int? IsClosing { get; set; }
    }
}
