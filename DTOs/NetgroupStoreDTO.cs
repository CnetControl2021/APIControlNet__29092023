namespace APIControlNet.DTOs
{
    public class NetgroupStoreDTO
    {
        public int NetgroupStoreIdx { get; set; }
        public Guid NetgroupId { get; set; }
        public Guid StoreId { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public bool? IsPriceDiscountEnable { get; set; }
        public Guid? NetgroupPriceId { get; set; }
        public DateTime? Date { get; set; }
        public DateTime? Updated { get; set; }
        public bool? Active { get; set; }
        public bool? Locked { get; set; }
        public bool? Deleted { get; set; }
    }
}
