using System;
using System.Collections.Generic;

namespace APIControlNet.Models
{
    public partial class CompraUltimo
    {
        public int LastInventoryInIdx { get; set; }
        public Guid? LastInventoryInId { get; set; }
        public Guid? StoreId { get; set; }
        public Guid? TankId { get; set; }
        public Guid? ProductId { get; set; }
        public Guid? IdProvComb { get; set; }
        public Guid? IdProvTran { get; set; }
        public decimal? StartVolume { get; set; }
        public decimal? StartVolumeTc { get; set; }
        public decimal? StartVolumeWater { get; set; }
        public decimal? StartTemperature { get; set; }
        public decimal? StartHeigh { get; set; }
        public decimal? EndVolumen { get; set; }
        public decimal? EndVolumeTc { get; set; }
        public decimal? EndVolumeWater { get; set; }
        public decimal? EndTemperature { get; set; }
        public decimal? EndHeight { get; set; }
        public decimal? Volume { get; set; }
        public decimal? VolumenTc { get; set; }
        public decimal? VolumeWater { get; set; }
        public int? DocumentVolume { get; set; }
        public string DocumentTipo { get; set; }
        public string DocumentFolio { get; set; }
        public string DocumentVehicle { get; set; }
        public string Description { get; set; }
        public decimal? Price { get; set; }
        public string Status { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public long? Date { get; set; }
        public long? Updated { get; set; }
        public byte? Active { get; set; }
        public byte? Locked { get; set; }
        public byte? Deleted { get; set; }
        public string Name { get; set; }
    }
}
