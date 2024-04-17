using APIControlNet.Models;
using System.ComponentModel.DataAnnotations;

namespace APIControlNet.DTOs
{
    public class LoadPositionDTO
    {
        //public LoadPosition()
        //{
        //    Hoses = new HashSet<Hose>();
        //    LoadPositionResponses = new HashSet<LoadPositionResponse>();
        //}

        public int? LoadPositionIdx { get; set; }
        public Guid StoreId { get; set; }
        public int LoadPositionIdi { get; set; }
        public string Name { get; set; }
        public int DispensaryIdi { get; set; }
        public int PortIdi { get; set; }
        public int? IslandIdi { get; set; }
        public int? CpuAddress { get; set; }
        public int? CpuNumberLoop { get; set; }
        public decimal? FactorSetQuantity { get; set; }
        public decimal? FactorSetCurrency { get; set; }
        public decimal? FactorSetPrice { get; set; }
        public decimal? FactorGetQuantity { get; set; }
        public decimal? FactorGetCurrency { get; set; }
        public decimal? FactorGetPrice { get; set; }
        public decimal? FactorGetTotalQuantity { get; set; }
        public decimal? FactorGetTotalCurrency { get; set; }
        public decimal? FactorPulseWayne { get; set; }
        public int? DispensingMode { get; set; }
        public int? IsSecurityEnabled { get; set; }
        public int? PriceLevel { get; set; }
        public int? MaximumQuantity { get; set; }
        public int? MaximumAmount { get; set; }
        public int? DefaultQuantity { get; set; }
        public int? MinimumQuantity { get; set; }
        public int? MinimumAmount { get; set; }
        public int? QuantityPrefix { get; set; }
        public int? IsStopCancelled { get; set; }
        public DateTime? Date { get; set; }
        public DateTime? Updated { get; set; }
        public bool? Active { get; set; }
        public bool? Locked { get; set; }
        public bool? Deleted { get; set; }
        public int? IsEnableSaveToZero { get; set; }
        public int? AutomaticPrintingIsEnabled { get; set; }
        public int? PointSaleIdi { get; set; }

        //public virtual Dispensary Dispensary { get; set; }
        //public virtual Port Port { get; set; }
        //public virtual Store Store { get; set; }
        //public virtual ICollection<Hose> Hoses { get; set; }
        //public virtual ICollection<LoadPositionResponse> LoadPositionResponses { get; set; }

    }
}
