using System;
using System.Collections.Generic;

namespace APIControlNet.Models
{
    public partial class ShiftPaymentDetail
    {
        public int ShiftPaymentDetailIdx { get; set; }
        public Guid ShiftPaymentId { get; set; }
        public int ShiftPaymentTypeIdi { get; set; }
        public int ShiftPaymentNumberReference { get; set; }
        public decimal? Amount { get; set; }
        public Guid? SaleOrderId { get; set; }
        public int? IsCaptured { get; set; }
        public DateTime? Date { get; set; }
        public DateTime? Updated { get; set; }
        public bool? Active { get; set; }
        public bool? Locked { get; set; }
        public bool? Deleted { get; set; }
    }
}
