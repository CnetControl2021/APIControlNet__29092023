namespace APIControlNet.DTOs
{
    public class NetgroupUserDTO
    {
        public int NetgroupUserIdx { get; set; }
        public Guid NetgroupId { get; set; }
        public string UserId { get; set; }
        public Guid? NetgroupUserId { get; set; } 
        public string Name { get; set; }
        public string Description { get; set; }
        public bool MakeInvoice { get; set; }
        public DateTime? Date { get; set; } = DateTime.Now;
        public DateTime? Updated { get; set; }=DateTime.Now;
        public bool? Active { get; set; } = true;
        public bool? Locked { get; set; } = false;
        public bool? Deleted { get; set; } = false;
    }
}
