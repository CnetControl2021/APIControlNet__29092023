namespace APIControlNet.DTOs
{
    public class DispensaryBrandDTO
    {

        public int DispensaryBrandIdx { get; set; }
        public int DispensaryBrandId { get; set; }
        public string Name { get; set; }
        public DateTime Date { get; set; }
        public DateTime Updated { get; set; }
        public byte? Active { get; set; }
        public byte? Locked { get; set; }
        public byte? Deleted { get; set; }
    }
}
