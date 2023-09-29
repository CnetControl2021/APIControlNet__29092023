using System;
using System.Collections.Generic;

namespace APIControlNet.Models
{
    public partial class LastInventoryIn
    {
        public int LastInventoryInIdx { get; set; }
        public Guid StoreId { get; set; }
        public int TankIdi { get; set; }
        public int? InventoryInNumber { get; set; }
        public Guid? InventoryInId { get; set; }
        public Guid? ProductId { get; set; }
        public int? ProductCodeVeeder { get; set; }
        public decimal? StartVolume { get; set; }
        public decimal? StartVolumeTc { get; set; }
        public decimal? StartVolumeWater { get; set; }
        public decimal? StartTemperature { get; set; }
        public decimal? StartHeight { get; set; }
        public DateTime? StartDate { get; set; }
        public decimal? EndVolume { get; set; }
        public decimal? EndVolumeTc { get; set; }
        public decimal? EndVolumeWater { get; set; }
        public decimal? EndTemperature { get; set; }
        public decimal? EndHeight { get; set; }
        public DateTime? EndDate { get; set; }
        public decimal? Volume { get; set; }
        public decimal? VolumeTc { get; set; }
        public decimal? VolumeWater { get; set; }
        public string DocumentType { get; set; }
        public int? DocumentNumber { get; set; }
        public string DocumentVehicle { get; set; }
        public decimal? DocumentVolume { get; set; }
        public decimal? UnitaryPrice { get; set; }
        public int? FuelSupplierId { get; set; }
        public int? TransportationId { get; set; }
        public DateTime? Date { get; set; }
        public DateTime? Updated { get; set; }
        public int? StatusRx { get; set; }
        public byte? Active { get; set; }
        public byte? Locked { get; set; }
        public byte? Deleted { get; set; }
        public string Name { get; set; }
    }
}
