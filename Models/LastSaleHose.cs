using System;
using System.Collections.Generic;

namespace APIControlNet.Models
{
    public partial class LastSaleHose
    {
        public int LastSaleHoseIdx { get; set; }
        public Guid? StoreId { get; set; }
        public int? HoseIdi { get; set; }
        public int? LoadPositionIdi { get; set; }
        public Guid? CustomerControlId { get; set; }
        public string Name { get; set; }
        public Guid? VehicleId { get; set; }
        public string CardEmployeeId { get; set; }
        public Guid? SaleOrderId { get; set; }
        public int? SaleOrderNumber { get; set; }
        public Guid? ProductId { get; set; }
        public Guid? ShiftId { get; set; }
        public Guid? EmployeeId { get; set; }
        public Guid? CustomerId { get; set; }
        public decimal? Quantity { get; set; }
        public decimal? Amount { get; set; }
        public decimal? Price { get; set; }
        public decimal? TotalAmount { get; set; }
        public decimal? TotalQuantity { get; set; }
        public decimal? TotalAmountElectronic { get; set; }
        public decimal? TotalQuantityElectronic { get; set; }
        public int? StatusRun { get; set; }
        public int? CpuAddressHose { get; set; }
        public int? SaleOrderNumberStart { get; set; }
        public int? TicketNumber { get; set; }
        public DateTime? StartDate { get; set; }
        public int? PresetType { get; set; }
        public decimal? PresetQuantity { get; set; }
        public decimal? PresetValue { get; set; }
        public DateTime? Date { get; set; }
        public DateTime? Updated { get; set; }
        public byte? Active { get; set; }
        public byte? Locked { get; set; }
        public byte? Deleted { get; set; }
        public int? InventoryIsRead { get; set; }
    }
}
