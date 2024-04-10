using System.ComponentModel.DataAnnotations;

namespace APIControlNet.DTOs
{
    public class DispensaryDTO
    {
        //public DispensaryDTO()
        //{
        //    LoadPositions = new HashSet<LoadPositionDTO>();
        //}
        public int? DispensaryIdx { get; set; }
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
        public bool? Active { get; set; } = true;
        public bool? Locked { get; set; } = false;
        public bool? Deleted { get; set; } = false;
        public int? Subtype { get; set; }
        public string UniqueId { get; set; }
        public int? DefaultHoseIdi { get; set; }

        //public virtual DispensaryBrandDTO DispensaryBrand { get; set; }
        //public virtual StoreDTO Store { get; set; }
        //public virtual ICollection<LoadPositionDTO> LoadPositions { get; set; }
    }
}
