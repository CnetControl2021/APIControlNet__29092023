namespace APIControlNet.DTOs
{
    public class OperatorDTO
    {
        public int OperatorIdx { get; set; }
        public Guid OperatorId { get; set; }
        public Guid CustomerId { get; set; }
        public long OperatorNumber { get; set; }
        public string Name { get; set; }
        public string Lastname { get; set; }
        public string MotherLastname { get; set; }
        public DateTime? Date { get; set; }
        public DateTime? Updated { get; set; }
        public bool? Active { get; set; }
        public bool? Locked { get; set; }
        public bool? Deleted { get; set; }
    }
}
