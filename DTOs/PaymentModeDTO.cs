namespace APIControlNet.DTOs
{
    public class PaymentModeDTO
    {
        public int PaymentModeIdx { get; set; }
        public Guid? PaymentModeId { get; set; }
        public int? PaymentModeNumber { get; set; }
        public string Name { get; set; }
        public DateTime? Date { get; set; }
        public DateTime? Updated { get; set; }
        public byte? Active { get; set; }
        public byte? Locked { get; set; }
        public byte? Deleted { get; set; }

    }
}
