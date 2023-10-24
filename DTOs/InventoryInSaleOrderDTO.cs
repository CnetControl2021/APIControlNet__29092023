namespace APIControlNet.DTOs
{
    public class InventoryInSaleOrderDTO
    {
        public int InventoryInSaleOrderIdx { get; set; }
        public Guid InventoryInSaleOrderId { get; set; }
        public Guid StoreId { get; set; }
        public DateTime? Date { get; set; }
        public int? TankIdi { get; set; }
        public Guid? ProductId { get; set; }
        public decimal? StartVolume { get; set; }
        public DateTime? StartDate { get; set; }
        public decimal? StartTemperature { get; set; }
        public decimal? EndVolume { get; set; }
        public decimal? EndTemperature { get; set; }
        public DateTime? EndDate { get; set; }
        public decimal? Volume { get; set; }
        public decimal? CalorificPower { get; set; }
        public decimal? AbsolutePressure { get; set; }
        public DateTime? Updated { get; set; } = DateTime.UtcNow;
        public bool? Active { get; set; } = true;
        public bool? Locked { get; set; } = false;
        public bool? Deleted { get; set; }=false;
    }
}
