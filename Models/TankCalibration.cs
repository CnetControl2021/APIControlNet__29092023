using System;
using System.Collections.Generic;

namespace APIControlNet.Models
{
    public partial class TankCalibration
    {
        public int TankCalibrationIdx { get; set; }
        public Guid TankCalibrationId { get; set; }
        public Guid TankId { get; set; }
        public Guid SupplierId { get; set; }
        public string Name { get; set; }
        public DateTime? Date { get; set; }
        public bool Active { get; set; }
        public bool? Locked { get; set; }
        public bool? Deleted { get; set; }
        public DateTime? Updated { get; set; }
    }
}
