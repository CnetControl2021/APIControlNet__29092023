using System;
using System.Collections.Generic;

namespace APIControlNet.Models
{
    public partial class TankCalibrationPoint
    {
        public int TankCalibrationPointsIdx { get; set; }
        public Guid StoreId { get; set; }
        public int TankIdi { get; set; }
        public int PointsId { get; set; }
        public decimal? Height { get; set; }
        public decimal? Quantity { get; set; }
        public string Name { get; set; }
        public DateTime? Date { get; set; }
        public DateTime? Updated { get; set; }
        public bool Active { get; set; }
        public bool? Locked { get; set; }
        public bool? Deleted { get; set; }
    }
}
