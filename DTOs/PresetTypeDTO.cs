namespace APIControlNet.DTOs
{
    public class PresetTypeDTO
    {
        public int PresetTypeIdx { get; set; }
        public int PresetType1 { get; set; }
        public string Description { get; set; }
        public DateTime? Date { get; set; }
        public DateTime? Updated { get; set; }
        public bool? Active { get; set; }
        public bool? Locked { get; set; }
        public bool? Deleted { get; set; }
    }
}
