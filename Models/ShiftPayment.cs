using System;
using System.Collections.Generic;

namespace APIControlNet.Models
{
    public partial class ShiftPayment
    {
        public int ShiftPaymentIdx { get; set; }
        public Guid ShiftIslandId { get; set; }
        public Guid? ShiftPaymentId { get; set; }
        public int ShiftPaymentTypeIdi { get; set; }
        public decimal? Amount { get; set; }
        public int? IsCaptured { get; set; }
        public DateTime? Date { get; set; }
        public DateTime? Updated { get; set; }
        public bool? Active { get; set; }
        public bool? Locked { get; set; }
        public bool? Deleted { get; set; }
    }
}
