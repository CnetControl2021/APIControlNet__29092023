using System;
using System.Collections.Generic;

namespace APIControlNet.Models
{
    public partial class AuthorizationSet
    {
        public int AuthorizationSetIdx { get; set; }
        public Guid? AuthorizationSetId { get; set; }
        public Guid? StoreId { get; set; }
        public Guid? HoseId { get; set; }
        public Guid? CustomerId { get; set; }
        public Guid? VehicleId { get; set; }
        public Guid? OperatorId { get; set; }
        public Guid? EmployeeId { get; set; }
        public int? Position { get; set; }
        public int? Network { get; set; }
        public int? Definition { get; set; }
        public int? TagUse { get; set; }
        public int? Type { get; set; }
        public decimal? Quantity { get; set; }
        public decimal? Price { get; set; }
        public long? Date { get; set; }
        public long? Updated { get; set; }
        public byte? Active { get; set; }
        public byte? Locked { get; set; }
        public byte? Deleted { get; set; }
        public string Name { get; set; }
    }
}
