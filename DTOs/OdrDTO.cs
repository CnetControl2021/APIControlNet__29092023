namespace APIControlNet.DTOs
{
    public class OdrDTO
    {
        public int OdrIdx { get; set; }
        public Guid? OdrId { get; set; } = Guid.NewGuid();
        public string CardOdr { get; set; }
        public Guid? OdrStoreId { get; set; }
        public Guid? CustomerId { get; set; }
        public Guid? VehicleId { get; set; }
        public int? OdrNumber { get; set; }
        public Guid? ProductId { get; set; }
        public string ProductCode { get; set; }
        public Guid? OperatorId1 { get; set; }
        public Guid? OperatorId2 { get; set; }
        public int? PresetType { get; set; }
        public decimal? PresetQuantity { get; set; }
        public Guid? SaleOrderId { get; set; }
        public DateTime? Date { get; set; }=DateTime.Now;
        public DateTime? EndDate { get; set; } = DateTime.Now;
        public DateTime? Updated { get; set; } = DateTime.Now;
        public bool? Active { get; set; } = true;
        public bool? Locked { get; set; }=false;
        public bool? Deleted { get; set; } = false;
    }
}
