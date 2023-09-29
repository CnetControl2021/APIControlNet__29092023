using System;
using System.Collections.Generic;

namespace APIControlNet.Models
{
    public partial class ShiftHead
    {
        public int ShiftHeadIdx { get; set; }
        public Guid? ShiftHeadId { get; set; }
        public Guid? StoreId { get; set; }
        public DateTime? ShiftDate { get; set; }
        public int? ShiftNumber { get; set; }
        public int? ShiftDay { get; set; }
        public DateTime? Date { get; set; }
        public DateTime? Updated { get; set; }
        public byte? Active { get; set; }
        public byte? Locked { get; set; }
        public byte? Deleted { get; set; }
        public string Name { get; set; }
    }
}
