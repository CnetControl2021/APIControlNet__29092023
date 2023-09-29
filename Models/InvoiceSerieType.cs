using System;
using System.Collections.Generic;

namespace APIControlNet.Models
{
    public partial class InvoiceSerieType
    {
        public int InvoiceSerieTypeIdx { get; set; }
        public int InvoiceSerieTypeId { get; set; }
        public string Description { get; set; }
        public string SatTipoComprobanteId { get; set; }
        public string InvoiceApplicationTypeId { get; set; }
        public int? FactorForClosing { get; set; }
        public DateTime? Date { get; set; }
        public DateTime? Updated { get; set; }
        public bool? Active { get; set; }
        public bool? Locked { get; set; }
        public bool? Deleted { get; set; }
    }
}
