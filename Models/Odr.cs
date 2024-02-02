using System;
using System.Collections.Generic;

namespace APIControlNet.Models
{
    public partial class Odr
    {
        public int OdrIdx { get; set; }
        public Guid? OdrId { get; set; }
        public string CardOdr { get; set; }
        public Guid? OdrStoreId { get; set; }
        public Guid? CustomerId { get; set; }
        public Guid? VehicleId { get; set; }
        public int? OdrNumber { get; set; }
        public Guid? ProductId { get; set; }
        public string ProductCode { get; set; }
        public Guid? OperatorId1 { get; set; }
        public Guid? OperatorId2 { get; set; }
        public int? PresetType { get; set; }
        public decimal? PresetQuantity { get; set; }
        public Guid? SaleOrderId { get; set; }
        public DateTime? Date { get; set; }
        public DateTime? EndDate { get; set; }
        public DateTime? Updated { get; set; }
        public bool? Active { get; set; }
        public bool? Locked { get; set; }
        public bool? Deleted { get; set; }
    }
}
