namespace APIControlNet.DTOs
{
    public class StoreHouseMovementDetailDTO
    {
        public int StoreHouseMovementDetailIdx { get; set; }
        public Guid StoreHouseMovementId { get; set; }
        public Guid ProductId { get; set; }
        public string Name { get; set; }
        public decimal? QuantityEntry { get; set; }
        public decimal? QuantityExit { get; set; }
        public decimal? QuantityTrasfer { get; set; }
        public string Location { get; set; }
        public DateTime? Date { get; set; } = DateTime.Now;
        public DateTime? Updated { get; set; } = DateTime.Now;
        public byte? Active { get; set; } = 1;
        public byte? Locked { get; set; }
        public byte? Deleted { get; set; }

        public List<ProductDTO> Products { get; set; } //agrege para traer el nombre del producto desde entidad producto.

        public Guid StoreHouseIdDestination { get; set; }
        public virtual ProductDTO Product { get; set; }  // uso include desde controller
        public virtual StoreHouseMovementDTO StoreHouseMovement { get; set; }

    }
}
