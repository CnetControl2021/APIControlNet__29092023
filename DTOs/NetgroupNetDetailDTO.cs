namespace APIControlNet.DTOs
{
    public class NetgroupNetDetailDTO
    {
        public int NetgroupNetDetailIdx { get; set; }
        public Guid NetgroupNetId { get; set; }
        public Guid StoreId { get; set; }
        public DateTime? Date { get; set; }
        public DateTime? Updated { get; set; }
        public bool? Active { get; set; }
        public bool? Locked { get; set; }
        public bool? Deleted { get; set; }
        public string StoreName { get; set; }
        public string NameNg { get; set; }
        public string NameNgn { get; set; }
    }
}
