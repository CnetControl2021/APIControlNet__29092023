namespace APIControlNet.DTOs
{
    public class NetgroupNetDTO
    {
        public int NetgroupNetIdx { get; set; }
        public Guid NetgroupNetId { get; set; }
        public string NetgroupNetName { get; set; }
        public Guid NetgroupId { get; set; }
        public DateTime? Date { get; set; }
        public DateTime? Updated { get; set; }
        public bool? Active { get; set; }
        public bool? Locked { get; set; }
        public bool? Deleted { get; set; }
    }
}
