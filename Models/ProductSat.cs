using System;
using System.Collections.Generic;

namespace APIControlNet.Models
{
    public partial class ProductSat
    {
        public int ProductSatIdx { get; set; }
        public Guid StoreId { get; set; }
        public Guid ProductId { get; set; }
        public int? ClaveProducto { get; set; }
        public int? ClaveSubProducto { get; set; }
        public int? ComposOctanajeGasolina { get; set; }
        public string GasolinaConCombustibleNoFosil { get; set; }
        public int? ComposDeCombustibleNoFosilEnGasolina { get; set; }
        public string DieselConCombustibleNoFosil { get; set; }
        public int? ComposDeCombustibleNoFosilEnDiesel { get; set; }
        public string TurbosinaConCombustibleNoFosil { get; set; }
        public int? ComposDeCombustibleNoFosilEnTurbosina { get; set; }
        public decimal? ComposDePropanoEnGasLp { get; set; }
        public decimal? ComposDeButanoEnGasLp { get; set; }
        public decimal? DensidadDePetroleo { get; set; }
        public decimal? ComposDeAzufreEnPetroleo { get; set; }
        public string Otros { get; set; }
        public int? MarcaComercial { get; set; }
        public int? Marcaje { get; set; }
        public int? ConcentracionSustanciaMarcaje { get; set; }
        public string GasNaturalOcondensados { get; set; }
        public string TipoCompuesto { get; set; }
        public decimal? FraccionMolar { get; set; }
        public decimal? PoderCalorifico { get; set; }
        public DateTime? Date { get; set; }
        public DateTime? Updated { get; set; }
        public bool? Active { get; set; }
        public bool? Locked { get; set; }
        public bool? Deleted { get; set; }
        public string SatProductKey { get; set; }
        public string SatProductSubkey { get; set; }
        public int? SatWithFossil { get; set; }
        public int? SatPercentageWithFossil { get; set; }

        public virtual Product Product { get; set; }
    }
}
