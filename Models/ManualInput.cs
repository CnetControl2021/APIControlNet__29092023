using System;
using System.Collections.Generic;

namespace APIControlNet.Models
{
    public partial class ManualInput
    {
        public int ManualInputIdx { get; set; }
        public Guid StoreId { get; set; }
        public int ManualInputIdi { get; set; }
        public string ManualCodeId { get; set; }
        public string TextToReplace { get; set; }
        public DateTime? Date { get; set; }
        public DateTime? Updated { get; set; }
        public bool? Active { get; set; }
        public bool? Locked { get; set; }
        public bool? Deleted { get; set; }
    }
}
