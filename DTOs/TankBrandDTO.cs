namespace APIControlNet.DTOs
{
    public class TankBrandDTO
    {
        public TankBrandDTO()
        {
            Tanks = new HashSet<TankDTO>();
        }

        public int TankBrandIdx { get; set; }
        public int TankBrandId { get; set; }
        public string Name { get; set; }
        public DateTime? Date { get; set; }
        public DateTime? Updated { get; set; }
        public byte? Active { get; set; }
        public byte? Locked { get; set; }
        public byte? Deleted { get; set; }

        public virtual ICollection<TankDTO> Tanks { get; set; }
    }
}
