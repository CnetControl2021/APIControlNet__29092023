using System;
using System.Collections.Generic;

namespace APIControlNet.Models
{
    public partial class HoseCalibration
    {
        public int HoseCalibrationIdx { get; set; }
        public Guid? HoseCalibrationId { get; set; }
        public Guid? StoreId { get; set; }
        public string Name { get; set; }
        public Guid? HoseId { get; set; }
        public Guid? SupplierId { get; set; }
        public long? Date { get; set; }
        public long? Updated { get; set; }
        public bool? Active { get; set; }
        public bool? Locked { get; set; }
        public bool? Deleted { get; set; }
    }
}
