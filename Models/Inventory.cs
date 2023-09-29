using System;
using System.Collections.Generic;

namespace APIControlNet.Models
{
    public partial class Inventory
    {
        public int InventoryIdx { get; set; }
        public Guid? InventoryId { get; set; }
        public int? InventoryNumber { get; set; }
        public string Name { get; set; }
        public int? TankIdi { get; set; }
        public Guid? StoreId { get; set; }
        public Guid? ProductId { get; set; }
        public Guid? ShiftHeadId { get; set; }
        public int? ProductCodeVeeder { get; set; }
        public decimal? Volume { get; set; }
        public decimal? VolumeTc { get; set; }
        public decimal? VolumeWater { get; set; }
        public decimal? Temperature { get; set; }
        public decimal? Height { get; set; }
        public decimal? HeightWater { get; set; }
        public decimal? ToFill { get; set; }
        public int? StatusRx { get; set; }
        public byte? IsFromShift { get; set; }
        public DateTime? Date { get; set; }
        public DateTime? Updated { get; set; }
        public bool? Active { get; set; }
        public bool? Locked { get; set; }
        public bool? Deleted { get; set; }
        public decimal? Pressure { get; set; }

        public virtual Store Store { get; set; }
    }
}
