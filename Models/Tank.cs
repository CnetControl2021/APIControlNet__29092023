using System;
using System.Collections.Generic;

namespace APIControlNet.Models
{
    public partial class Tank
    {
        public int TankIdx { get; set; }
        public Guid StoreId { get; set; }
        public int TankIdi { get; set; }
        public Guid ProductId { get; set; }
        public int? TankCpuAddress { get; set; }
        public int? PortIdi { get; set; }
        public int? TankBrandId { get; set; }
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
        public DateTime? Date { get; set; }
        public DateTime? Updated { get; set; }
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

        public virtual Port Port { get; set; }
        public virtual Store Store { get; set; }
        public virtual TankBrand TankBrand { get; set; }
    }
}
