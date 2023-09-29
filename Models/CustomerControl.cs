using System;
using System.Collections.Generic;

namespace APIControlNet.Models
{
    public partial class CustomerControl
    {
        public int CustomerControlIdx { get; set; }
        public Guid StoreId { get; set; }
        public Guid CustomerControlId { get; set; }
        public string Name { get; set; }
        public Guid? CustomerId { get; set; }
        public Guid? VehicleId { get; set; }
        public Guid? Operator1Id { get; set; }
        public Guid? Operator2Id { get; set; }
        public Guid? OdrId { get; set; }
        public int? OdometerStart { get; set; }
        public int? OdometerEnd { get; set; }
        public string CardVehicleId { get; set; }
        public string CardOperator1Id { get; set; }
        public string CardOperator2Id { get; set; }
        public DateTime? Date { get; set; }
        public DateTime? Updated { get; set; }
        public byte? Active { get; set; }
        public byte? Locked { get; set; }
        public byte? Deleted { get; set; }
    }
}
