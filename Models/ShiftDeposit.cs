using System;
using System.Collections.Generic;

namespace APIControlNet.Models
{
    public partial class ShiftDeposit
    {
        public int ShiftDepositIdx { get; set; }
        public Guid? StoreId { get; set; }
        public int? IslandIdi { get; set; }
        public int? ShiftDepositNumber { get; set; }
        public Guid? ShiftHeadId { get; set; }
        public Guid? EmployeeId { get; set; }
        public decimal? Amount { get; set; }
        public DateTime? Date { get; set; }
        public DateTime? Updated { get; set; }
        public byte? Active { get; set; }
        public byte? Locked { get; set; }
        public byte? Deleted { get; set; }
        public string Name { get; set; }
        public int? CapsuleNumber { get; set; }
    }
}
