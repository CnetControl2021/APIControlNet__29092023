using System;
using System.Collections.Generic;

namespace APIControlNet.Models
{
    public partial class LastInventory
    {
        public int LastInventoryIdx { get; set; }
        public Guid? StoreId { get; set; }
        public int? TankIdi { get; set; }
        public Guid? InventoryId { get; set; }
        public int? InventoryNumber { get; set; }
        public Guid? ProductId { get; set; }
        public int? ProductCodeVeeder { get; set; }
        public decimal? Volume { get; set; }
        public decimal? VolumeTc { get; set; }
        public decimal? VolumeWater { get; set; }
        public decimal? Temperature { get; set; }
        public decimal? Height { get; set; }
        public decimal? HeightWater { get; set; }
        public decimal? ToFill { get; set; }
        public int? StatusRx { get; set; }
        public DateTime? Date { get; set; }
        public DateTime? Updated { get; set; }
        public byte? Active { get; set; }
        public byte? Locked { get; set; }
        public byte? Deleted { get; set; }
        public string Name { get; set; }
        public DateTime? LastDate { get; set; }
        public string StatusResponse { get; set; }
        public decimal? Pressure { get; set; }
    }
}
