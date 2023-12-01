using System;
using System.Collections.Generic;

namespace APIControlNet.Models
{
    public partial class Dispensary
    {
        public Dispensary()
        {
            LoadPositions = new HashSet<LoadPosition>();
        }

        public int DispensaryIdx { get; set; }
        public int DispensaryIdi { get; set; }
        public Guid StoreId { get; set; }
        public string Name { get; set; }
        public string SatMeasurementType { get; set; }
        public string SatMeasurementDescription { get; set; }
        public decimal? SatMeasurementPercentageUncertainty { get; set; }
        public DateTime? SatCalibrationDate { get; set; }
        public int? DispensaryBrandId { get; set; }
        public string Description { get; set; }
        public DateTime? Date { get; set; }
        public DateTime? Updated { get; set; }
        public bool? Active { get; set; }
        public bool? Locked { get; set; }
        public bool? Deleted { get; set; }
        public int? Subtype { get; set; }
        public string UniqueId { get; set; }

        public virtual DispensaryBrand DispensaryBrand { get; set; }
        public virtual Store Store { get; set; }
        public virtual ICollection<LoadPosition> LoadPositions { get; set; }
    }
}
