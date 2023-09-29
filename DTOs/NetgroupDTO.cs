namespace APIControlNet.DTOs
{
    public class NetgroupDTO
    {
        public int NetgroupIdx { get; set; }
        public Guid NetgroupId { get; set; }
        public int? NetgroupIdi { get; set; }
        public string Description { get; set; }
        public string ShortDescription { get; set; }
        public DateTime? Date { get; set; }
        public DateTime? Updated { get; set; }
        public bool? Active { get; set; }
        public bool? Locked { get; set; }
        public bool? Deleted { get; set; }
    }
}
