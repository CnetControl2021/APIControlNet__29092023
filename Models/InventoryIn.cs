using System;
using System.Collections.Generic;

namespace APIControlNet.Models
{
    public partial class InventoryIn
    {
        public int InventoryInIdx { get; set; }
        public Guid InventoryInId { get; set; }
        public Guid StoreId { get; set; }
        public int? InventoryInNumber { get; set; }
        public DateTime? Date { get; set; }
        public int TankIdi { get; set; }
        public Guid? ProductId { get; set; }
        public Guid? ShiftHeadId { get; set; }
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
        public int? StatusRx { get; set; }
        public DateTime? Updated { get; set; }
        public bool? Active { get; set; }
        public bool? Locked { get; set; }
        public bool? Deleted { get; set; }
        public string Name { get; set; }
        public string JsonTipoDistribucionId { get; set; }
        public decimal? AbsolutePressure { get; set; }
        public decimal? CalorificPower { get; set; }
        public int? ProductCompositionId { get; set; }
        public int? ImportPermissionId { get; set; }
        public int? TransportPermissionId { get; set; }

        public virtual Store Store { get; set; }
    }
}
