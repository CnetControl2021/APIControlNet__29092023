using System;
using System.Collections.Generic;

namespace APIControlNet.Models
{
    public partial class SaleSuborder
    {
        public int SaleSuborderIdx { get; set; }
        public Guid SaleOrderId { get; set; }
        public int? SaleSuborderIdi { get; set; }
        public string Name { get; set; }
        public Guid ProductId { get; set; }
        public decimal? Quantity { get; set; }
        public decimal? Amount { get; set; }
        public decimal? Price { get; set; }
        public decimal? Discount { get; set; }
        public decimal? Subtotal { get; set; }
        public decimal? TotalAmount { get; set; }
        public decimal? TotalQuantity { get; set; }
        public decimal? TotalAmountElectronic { get; set; }
        public decimal? TotalQuantityElectronic { get; set; }
        public int? PresetType { get; set; }
        public decimal? PresetQuantity { get; set; }
        public decimal? PresetValue { get; set; }
        public DateTime? Date { get; set; }
        public DateTime? Updated { get; set; }
        public bool? Active { get; set; }
        public bool? Locked { get; set; }
        public bool? Deleted { get; set; }
        public int? DeviceIdi { get; set; }
        public decimal? Tax { get; set; }
        public decimal? Ieps { get; set; }
        public decimal? Temperature { get; set; }
        public decimal? QuantityTc { get; set; }
        public decimal? AbsolutePressure { get; set; }
        public decimal? CalorificPower { get; set; }
        public int? ProductCompositionId { get; set; }
        public decimal? StartQuantity { get; set; }
        public decimal? EndQuantity { get; set; }

        public virtual SaleOrder SaleOrder { get; set; }
    }
}
