namespace APIControlNet.DTOs
{
    public class ValidateTypeDTO
    {
        public int ValidateTypeIdx { get; set; }
        public int ValidateTypeId { get; set; }
        public string Name { get; set; }
        public DateTime? Date { get; set; }
        public DateTime? Updated { get; set; }
        public bool? Active { get; set; }
        public bool? Locked { get; set; }
        public bool? Deleted { get; set; }
    }
}
