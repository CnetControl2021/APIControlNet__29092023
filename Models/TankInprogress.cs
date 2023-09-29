using System;
using System.Collections.Generic;

namespace APIControlNet.Models
{
    public partial class TankInprogress
    {
        public int TankInprogressIdx { get; set; }
        public Guid StoreId { get; set; }
        public int TankIdi { get; set; }
        public int? IsInventoryInprogress { get; set; }
        public decimal? LastVolume { get; set; }
        public DateTime? LastDate { get; set; }
        public int Count { get; set; }
        public int CountFalse { get; set; }
        public DateTime? StartDate { get; set; }
        public decimal? Height { get; set; }
        public decimal? HeightWater { get; set; }
        public decimal? Volume { get; set; }
        public decimal? VolumeWater { get; set; }
        public decimal? Temperature { get; set; }
        public decimal? VolumeTc { get; set; }
        public decimal? Difference { get; set; }
        public DateTime? Date { get; set; }
        public DateTime? Updated { get; set; }
        public bool? Active { get; set; }
        public bool? Locked { get; set; }
        public bool? Deleted { get; set; }
    }
}
