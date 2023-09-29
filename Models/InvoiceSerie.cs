using System;
using System.Collections.Generic;

namespace APIControlNet.Models
{
    public partial class InvoiceSerie
    {
        public int InvoiceSerieIdx { get; set; }
        public Guid? CompanyId { get; set; }
        public Guid? StoreId { get; set; }
        public string InvoiceSerieId { get; set; }
        public string Description { get; set; }
        public int Folio { get; set; }
        public DateTime? Date { get; set; }
        public DateTime? Updated { get; set; }
        public byte? Active { get; set; }
        public byte? Locked { get; set; }
        public byte? Deleted { get; set; }
        public int? InvoiceSerieTypeId { get; set; }
    }
}
