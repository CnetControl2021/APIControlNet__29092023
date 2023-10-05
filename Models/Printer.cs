using System;
using System.Collections.Generic;

namespace APIControlNet.Models
{
    public partial class Printer
    {
        public int PrinterIdx { get; set; }
        public int? PrinterIdi { get; set; }
        public Guid? StoreId { get; set; }
        public string PrinterIpAddress { get; set; }
        public int? PrinterIpPort { get; set; }
        public string PrinterName { get; set; }
        public int? PrinterBrandId { get; set; }
        public int? PrinterBaudRate { get; set; }
        public DateTime? Date { get; set; }
        public DateTime? Updated { get; set; }
        public byte? Active { get; set; }
        public byte? Locked { get; set; }
        public byte? Deleted { get; set; }
        public string Description { get; set; }
        public int? EnablePrintToPrinterName { get; set; }
    }
}
