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
        public DateTime? Date { get; set; }
        public DateTime? Updated { get; set; }
        public bool? Active { get; set; }
        public bool? Locked { get; set; }
        public bool? Deleted { get; set; }
        public string Password { get; set; }
        public string PasswordAspnet { get; set; }
        public string DeviceCode { get; set; }
        public string Phone { get; set; }
        public int? NetgroupUserTypeId { get; set; }
        public Guid? CustomerId { get; set; }
    }
}
