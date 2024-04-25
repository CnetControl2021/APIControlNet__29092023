using APIControlNet.Models;
using System.ComponentModel.DataAnnotations;

namespace APIControlNet.DTOs
{
    public class TankDTO
    {
        public int? TankIdx { get; set; }
        public Guid StoreId { get; set; }
        public int TankIdi { get; set; }
        public Guid ProductId { get; set; }
        public int? TankCpuAddress { get; set; }
        public int? PortIdi { get; set; }
        public int? TankBrandId { get; set; }
        public string Name { get; set; }
        public decimal? CapacityTotal { get; set; }
        public decimal? CapacityOperational { get; set; }
        public decimal? CapacityMinimumOperating { get; set; }
        public decimal? CapacityUseful { get; set; }
        public decimal? Fondage { get; set; }
        public DateTime? SatDateCalibration { get; set; }
        public string SatTypeMeasurement { get; set; }
        public string SatTankType { get; set; }
        public string SatTypeMediumStorage { get; set; }
        public string SatDescriptionMeasurement { get; set; }
        public int? SatPercentageUncertaintyMeasurement { get; set; }
        public byte? EnableGetInventory { get; set; }
        public DateTime? Date { get; set; } = DateTime.Now;
        public DateTime? Updated { get; set; }=DateTime.Now;
        public bool? Active { get; set; }
        public bool? Locked { get; set; }
        public bool? Deleted { get; set; }
        public string StatusRes { get; set; }
        public int? CommPercentage { get; set; }
        public string Nuevocampo { get; set; }
        public int? UtilityPercentaje { get; set; }
        public string TankShapeId { get; set; }
        public string TankTypeId { get; set; }
        public decimal? DiameterOrWidth { get; set; }
        public decimal? Length { get; set; }
        public decimal? Height { get; set; }
        public decimal? HeightStart { get; set; }
        public decimal? HeightNotFuel { get; set; }
        public decimal? MultiplicationFactor { get; set; }
        public int? CalculateQuantityWithTable { get; set; }
        public int? TankCpuAddressNew { get; set; }
        public decimal? CapacityGastalon { get; set; }
        public string ResponseInventoryIn { get; set; }

        public virtual PortDTO Port { get; set; }
        public virtual StoreDTO Store { get; set; }
        public virtual TankBrandDTO TankBrand { get; set; }

    }
}
