namespace APIControlNet.DTOs
{
    public class StoreHouseDTO
    {
        public int StoreHouseIdx { get; set; }
        public Guid StoreHouseId { get; set; } = Guid.NewGuid();
        public Guid StoreId { get; set; }
        public string Name { get; set; }
        public DateTime? Date { get; set; } = DateTime.Now;
        public DateTime? Updated { get; set; } = DateTime.Now;
        public byte? Active { get; set; } = 1;
        public byte? Locked { get; set; } 
        public byte? Deleted { get; set; }

        public virtual ICollection<StoreHouseMovementDTO> StoreHouseMovements { get; set; }
    }
}
