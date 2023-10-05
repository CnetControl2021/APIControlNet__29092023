using System;
using System.Collections.Generic;

namespace APIControlNet.Models
{
    public partial class PointSale
    {
        public int PointSaleIdx { get; set; }
        public Guid? StoreId { get; set; }
        public int? PointSaleIdi { get; set; }
        public int? PortIdi { get; set; }
        public string Name { get; set; }
        public int? Type { get; set; }
        public int? Subtype { get; set; }
        public int? Address { get; set; }
        public int? PrinterBaudRate { get; set; }
        public int? PrinterBrandId { get; set; }
        public int? PrinterIdi { get; set; }
        public int? TypeAuthorization { get; set; }
        public string PointSaleUnique { get; set; }
        public string InvoiceSerieId { get; set; }
        public int? StatusRes { get; set; }
        public int? CommPercentage { get; set; }
        public DateTime? Date { get; set; }
        public DateTime? Updated { get; set; }
        public bool? Active { get; set; }
        public bool? Locked { get; set; }
        public bool? Deleted { get; set; }
        public int? IsEnabledPrintToPrinterIdi { get; set; }

        public virtual Store Store { get; set; }
    }
}
