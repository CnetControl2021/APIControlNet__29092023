using System;
using System.Collections.Generic;

namespace APIControlNet.Models
{
    public partial class Report
    {
        public int ReportIdx { get; set; }
        public Guid ReportId { get; set; }
        public string Description { get; set; }
        public string Head { get; set; }
        public string Query { get; set; }
        public DateTime? Date { get; set; }
        public DateTime? Updated { get; set; }
        public bool? Active { get; set; }
        public bool? Locked { get; set; }
        public bool? Deleted { get; set; }
        public int? NumberOfColumnsForSubtotal { get; set; }
        public string NameOfColumnsForSubtotal { get; set; }
        public string ShortName { get; set; }
    }
}
