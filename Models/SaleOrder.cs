using System;
using System.Collections.Generic;

namespace APIControlNet.Models
{
    public partial class SaleOrder
    {
        public int SaleOrderIdx { get; set; }
        public Guid SaleOrderId { get; set; }
        public Guid StoreId { get; set; }
        public int? SaleOrderNumber { get; set; }
        public string Name { get; set; }
        public Guid? EmployeeId { get; set; }
        public int? SaleOrderNumberStart { get; set; }
        public Guid? CustomerControlId { get; set; }
        public int? HoseIdi { get; set; }
        public Guid? ShiftId { get; set; }
        public Guid? CustomerId { get; set; }
        public Guid? VehicleId { get; set; }
        public string CardEmployeeId { get; set; }
        public decimal? Amount { get; set; }
        public string Status { get; set; }
        public DateTime? StartDate { get; set; }
        public int? TicketNumber { get; set; }
        public DateTime? Date { get; set; }
        public int? TankIdi { get; set; }
        public DateTime? Updated { get; set; }
        public Guid? InventoryInSaleOrderId { get; set; }
        public bool? Active { get; set; }
        public bool? Locked { get; set; }
        public bool? Deleted { get; set; }

        public virtual Store Store { get; set; }
    }
}
