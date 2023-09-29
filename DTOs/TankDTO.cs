using APIControlNet.Models;
using System.ComponentModel.DataAnnotations;

namespace APIControlNet.DTOs
{
    public class TankDTO
    {
        [Required(ErrorMessage = "El campo {0} es requerio")]
        public int TankIdx { get; set; }
        public Guid StoreId { get; set; }
        [Required(ErrorMessage = "El campo {0} es requerio")]
        public int? TankIdi { get; set; }
        public Guid ProductId { get; set; }
        public int? TankCpuAddress { get; set; }
        public int? PortIdi { get; set; }
        public int? TankBrandId { get; set; }
        [Required(ErrorMessage = "El campo {0} es requerio")]
        public string Name { get; set; }
        public int? CapacityTotal { get; set; }
        public int? CapacityOperational { get; set; }
        public int? CapacityMinimumOperating { get; set; }
        public int? CapacityUseful { get; set; }
        public int? Fondage { get; set; }
        public DateTime? SatDateCalibration { get; set; }
        public string SatTypeMeasurement { get; set; }
        public string SatTankType { get; set; }
        public string SatTypeMediumStorage { get; set; }
        public string SatDescriptionMeasurement { get; set; }
        public int? SatPercentageUncertaintyMeasurement { get; set; }
        public byte? EnableGetInventory { get; set; }
        public DateTime? Date { get; set; } = DateTime.Now;
        public DateTime? Updated { get; set; }= DateTime.Now;
        public bool? Active { get; set; }=true;
        public bool? Locked { get; set; }=false;
        public bool? Deleted { get; set; }=false;
        public string StatusRes { get; set; }
        public int? CommPercentage { get; set; }

        public virtual PortDTO Port { get; set; }
        public virtual StoreDTO Store { get; set; }
        public virtual TankBrandDTO TankBrand { get; set; }
    }
}
