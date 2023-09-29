using System;
using System.Collections.Generic;

namespace APIControlNet.Models
{
    public partial class InventoryInResp
    {
        public int InventoryInIdx { get; set; }
        public Guid? InventoryInId { get; set; }
        public Guid? InventoryId { get; set; }
        public int? HoseIdi { get; set; }
        public int? IdProvComb { get; set; }
        public int? IdProvTran { get; set; }
        public Guid? ProductId { get; set; }
        public decimal? StartVolume { get; set; }
        public decimal? StartVolumeTc { get; set; }
        public decimal? StartVolumeWater { get; set; }
        public decimal? StartVolumeDate { get; set; }
        public decimal? EndVolume { get; set; }
        public decimal? EndVolumeTc { get; set; }
        public decimal? EndVolumeWater { get; set; }
        public decimal? StartTemperature { get; set; }
        public decimal? EndTemperature { get; set; }
        public decimal? StartHeight { get; set; }
        public decimal? EndHeight { get; set; }
        public decimal? DocumentVolume { get; set; }
        public string DocumentType { get; set; }
        public string DocumentFolio { get; set; }
        public string DocumentVehicle { get; set; }
        public decimal? UnitaryPrice { get; set; }
        public string Status { get; set; }
        public string StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string Date { get; set; }
        public long? Updated { get; set; }
        public byte? Active { get; set; }
        public byte? Locked { get; set; }
        public byte? Deleted { get; set; }
        public string Name { get; set; }
    }
}
