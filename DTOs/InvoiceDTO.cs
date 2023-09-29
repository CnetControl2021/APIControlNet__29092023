using APIControlNet.Models;
using APIControlNet.Utilidades;
using System.ComponentModel.DataAnnotations;

namespace APIControlNet.DTOs
{
    public class InvoiceDTO
    {
        public int InvoiceIdx { get; set; }
        public Guid InvoiceId { get; set; }
        public Guid StoreId { get; set; }
        public string InvoiceSerieId { get; set; }
        public string Folio { get; set; }
        public DateTime? Date { get; set; }
        public Guid? CustomerId { get; set; }
        public decimal? Subtotal { get; set; }
        public decimal? AmountTax { get; set; }
        public decimal? AmountIeps { get; set; }
        public decimal? AmountIsr { get; set; }
        public decimal? Amount { get; set; }
        public string Uuid { get; set; }
        public string InvoiceApplicationTypeId { get; set; }
        public bool? IsCancelled { get; set; }
        public bool? IsStamped { get; set; }
        public bool? IsStampedCancelled { get; set; }
        public bool? IsClosing { get; set; }
        public bool? IsRelated { get; set; }
        public string SatFormaPagoId { get; set; }
        public string SatUsoCfdiId { get; set; }
        public string SatMetodoPagoId { get; set; }
        public string SatTipoComprobanteId { get; set; }
        public string SatMotivoCancelacionId { get; set; }
        public string SatTipoRelacionId { get; set; }
        public string SatPeriodicidadId { get; set; }
        public string SatMesesId { get; set; }
        public int? ClosingYear { get; set; }
        public int? PacId { get; set; }
        public DateTime? Updated { get; set; } = DateTime.Now;
        public bool? Active { get; set; } = true;
        public bool? Locked { get; set; } = false;
        public bool? Deleted { get; set; }=false;
        public int? IsIssued { get; set; }

        public virtual Store Store { get; set; }
    }
}
