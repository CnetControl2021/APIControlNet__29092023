using System;
using System.Collections.Generic;

namespace APIControlNet.Models
{
    public partial class LastShift
    {
        public int LastShiftIdx { get; set; }
        public Guid? StoreId { get; set; }
        public int? HoseIdi { get; set; }
        public Guid? ShiftId { get; set; }
        public Guid? ShiftHeadId { get; set; }
        public Guid? ShiftHeadIdNew { get; set; }
        public Guid? EmployeeId { get; set; }
        public string CardEmployeeId { get; set; }
        public int? StatusRun { get; set; }
        public decimal? StartQuantity { get; set; }
        public decimal? StartAmount { get; set; }
        public decimal? EndQuantity { get; set; }
        public decimal? EndAmount { get; set; }
        public decimal? StartAmountElectronic { get; set; }
        public decimal? StartQuantityElectronic { get; set; }
        public decimal? EndAmountElectronic { get; set; }
        public decimal? EndQuantityElectronic { get; set; }
        public decimal? TotalQuantity { get; set; }
        public decimal? TotalAmount { get; set; }
        public decimal? Price { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public DateTime? Date { get; set; }
        public DateTime? Updated { get; set; }
        public byte? Active { get; set; }
        public byte? Locked { get; set; }
        public byte? Deleted { get; set; }
        public string Name { get; set; }
        public int? Pause { get; set; }
    }
}
