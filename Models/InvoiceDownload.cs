using System;
using System.Collections.Generic;

namespace APIControlNet.Models
{
    public partial class InvoiceDownload
    {
        public int InvoiceDownloadIdx { get; set; }
        public Guid CompanyId { get; set; }
        public DateTime? Date { get; set; }
        public int? PeriodTypeId { get; set; }
        public int? Type { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public bool? IsChecked { get; set; }
        public bool? IsStarted { get; set; }
        public bool? IsFinished { get; set; }
        public int? RequestStatus { get; set; }
        public int? DownloadStatus { get; set; }
        public int? NumberOfCfdis { get; set; }
        public string PackagesId { get; set; }
        public Guid? UuidRequest { get; set; }
        public string Response { get; set; }
        public DateTime? Updated { get; set; }
        public bool? Active { get; set; }
        public bool? Locked { get; set; }
        public bool? Deleted { get; set; }
        public int? InvoiceDownloadIdi { get; set; }
        public int? IsDownloadFromInvoice { get; set; }
        public int? NumberOfAttempts { get; set; }
        public int? IsFinishedVerify { get; set; }
    }
}
