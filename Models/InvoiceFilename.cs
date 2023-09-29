using System;
using System.Collections.Generic;

namespace APIControlNet.Models
{
    public partial class InvoiceFilename
    {
        public int InvoiceFilenameIdx { get; set; }
        public Guid? CompanyId { get; set; }
        public int? Type { get; set; }
        public Guid UuidRequest { get; set; }
        public string PackageId { get; set; }
        public string FileName { get; set; }
        public string Response { get; set; }
        public bool? IsProcessed { get; set; }
        public DateTime? Date { get; set; }
        public DateTime? Updated { get; set; }
        public bool? Active { get; set; }
        public bool? Locked { get; set; }
        public bool? Deleted { get; set; }
        public int? NumberOfCfdi { get; set; }
    }
}
