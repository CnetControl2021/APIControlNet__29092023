using System;
using System.Collections.Generic;

namespace APIControlNet.Models
{
    public partial class StoreInvoiceSerie
    {
        public int StoreInvoiceSerieIdx { get; set; }
        public Guid? StoreId { get; set; }
        public string InvoiceSerieId { get; set; }
        public string Description { get; set; }
        public DateTime? Date { get; set; }
        public DateTime? Updated { get; set; }
        public byte? Active { get; set; }
        public byte? Locked { get; set; }
        public byte? Deleted { get; set; }
    }
}
