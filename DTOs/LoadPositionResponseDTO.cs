using APIControlNet.Models;

namespace APIControlNet.DTOs
{
    public class LoadPositionResponseDTO
    {
        public int LoadPositionResponseIdx { get; set; }
        //public Guid? StoreId { get; set; }
        public int? LoadPositionIdi { get; set; }
        public int? HoseIdi { get; set; }
        //public string Name { get; set; }
        //public Guid? CustomerId { get; set; }
        //public Guid? EmployeeId { get; set; }
        //public Guid? ShiftId { get; set; }
        //public Guid? CustomerControlId { get; set; }
        //public string CardEmployeeId { get; set; }
        //public Guid? ProductId { get; set; }
        //public Guid? VehicleId { get; set; }
        public decimal? Quantity { get; set; }
        public decimal? Amount { get; set; }
        public decimal? Price { get; set; }
        //public decimal? TotalAmountElectronic { get; set; }
        //public decimal? TotalQuantityElectronic { get; set; }
        //public decimal? TotalAmount { get; set; }
        //public decimal? TotalQuantity { get; set; }
        //public int? CpuAddressHose { get; set; }
        //public int? SaleOrderNumberStart { get; set; }
        //public int? StatusRun { get; set; }
        //public int? PriceLevel { get; set; }
        //public DateTime? StartDate { get; set; }
        //public int? StatusCommResp { get; set; }
        public int StatusDispenserIdi { get; set; }
        //public int? StoreNumber { get; set; }
        //public int? PresetType { get; set; }
        //public decimal? PresetQuantity { get; set; }
        //public decimal? PresetValue { get; set; }
        //public byte? IsCash { get; set; }
        //public long? StatusChangeTimes { get; set; }
        //public string Message { get; set; }
        //public DateTime? Date { get; set; }
        //public DateTime? Updated { get; set; }
        //public bool? Active { get; set; }
        //public bool? Locked { get; set; }
        //public bool? Deleted { get; set; }
        //public int? CommPercentage { get; set; }
        //public int? TicketNumber { get; set; }
        //public int? CpuAddressHoseAuthorized { get; set; }
        public int? LastStatusDispenser { get; set; }

        //public virtual LoadPositionDTO LoadPosition { get; set; }

        public string ColorStatus { get; set; }  // agregre aqui
        public string Description { get; set; } // agregre aqui
        public string Productname { get; set; }  // agregre aqui

        //public virtual StatusDispenserDTO StatusDispenserIdiNavigation { get; set; }
    }
}
