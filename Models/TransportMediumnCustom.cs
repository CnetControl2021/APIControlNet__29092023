using System;
using System.Collections.Generic;

namespace APIControlNet.Models
{
    public partial class TransportMediumnCustom
    {
        public int TransportMediumnCustomsIdx { get; set; }
        public int TransportMediumnCustomsId { get; set; }
        public string TransportMediumn { get; set; }
        public DateTime? Date { get; set; }
        public DateTime? Updated { get; set; }
        public int? Active { get; set; }
        public int? Locked { get; set; }
        public int? Deleted { get; set; }
    }
}
