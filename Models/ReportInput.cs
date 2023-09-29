using System;
using System.Collections.Generic;

namespace APIControlNet.Models
{
    public partial class ReportInput
    {
        public int ReportInputIdx { get; set; }
        public Guid ReportId { get; set; }
        public int ReportInputIdi { get; set; }
        public string ReportCodeId { get; set; }
        public string Description { get; set; }
        public int? OrderNumber { get; set; }
        public DateTime? Date { get; set; }
        public DateTime? Updated { get; set; }
        public bool? Active { get; set; }
        public bool? Locked { get; set; }
        public bool? Deleted { get; set; }
    }
}
