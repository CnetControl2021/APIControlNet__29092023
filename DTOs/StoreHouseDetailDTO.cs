namespace APIControlNet.DTOs
{
    public class StoreHouseDetailDTO
    {
        public int StoreHouseDetailsIdx { get; set; }
        public Guid StoreHouseId { get; set; }
        public Guid StoreHouseIdDestination { get; set; }
        public Guid ProductId { get; set; }
        public decimal? QuantityEntry { get; set; }
        public decimal? QuantityExit { get; set; }
        public decimal? Quantity { get; set; }
        public string Location { get; set; }
        public DateTime? Date { get; set; }
        public DateTime? Updated { get; set; }
        public byte? Active { get; set; }
        public byte? Locked { get; set; }
        public byte? Deleted { get; set; }
    }
}
