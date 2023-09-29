using System;
using System.Collections.Generic;

namespace APIControlNet.Models
{
    public partial class ApplicationType
    {
        public int InvoiceApplicationTypeIdx { get; set; }
        public string InvoiceApplicationTypeId { get; set; }
        public string Descripcion { get; set; }
        public DateTime? Date { get; set; }
        public DateTime? Updated { get; set; }
        public bool? Active { get; set; }
        public bool? Locked { get; set; }
        public bool? Deleted { get; set; }
    }
}
