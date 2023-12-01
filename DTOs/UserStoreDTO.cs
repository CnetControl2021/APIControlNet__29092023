namespace APIControlNet.DTOs
{
    public class UserStoreDTO
    {
        public int UserStoreIdx { get; set; }
        public string UserId { get; set; }
        public Guid StoreId { get; set; }
        public DateTime? Date { get; set; }
        public DateTime? Updated { get; set; }
        public bool? Active { get; set; }
        public bool? Locked { get; set; }
        public bool? Deleted { get; set; }
    }
}
