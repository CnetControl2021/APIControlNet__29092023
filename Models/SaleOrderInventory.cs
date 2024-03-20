using System;
using System.Collections.Generic;

namespace APIControlNet.Models
{
    public partial class SaleOrderInventory
    {
        public int SaleOrderInventoryIdx { get; set; }
        public Guid SaleOrderId { get; set; }
        public int? TankIdi { get; set; }
        public float? Height { get; set; }
        public float? Volume { get; set; }
        public DateTime? Date { get; set; }
        public DateTime? Updated { get; set; }
        public bool? Active { get; set; }
        public bool? Locked { get; set; }
        public bool? Deleted { get; set; }
    }
}
