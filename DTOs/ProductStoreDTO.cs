using APIControlNet.Models;


namespace APIControlNet.DTOs
{
    public class ProductStoreDTO
    {
        public ProductStoreDTO()
        {
            Hoses = new HashSet<HoseDTO>();
        }
        public int ProductStoreIdx { get; set; }
        public Guid StoreId { get; set; }
        public Guid ProductId { get; set; }
        public string ProductPemex { get; set; }
        public string ProductUce { get; set; }
        public int? SystemGradeId { get; set; } = 1;
        public decimal? Cost { get; set; }
        public decimal? Existence { get; set; }
        public decimal? Tax { get; set; }
        public decimal? Ieps { get; set; }
        public decimal? Price { get; set; }
        public DateTime? Date { get; set; }=DateTime.Now;
        public DateTime? Updated { get; set; } = DateTime.Now;
        public bool? Active { get; set; }=true;
        public bool? Locked { get; set; } = false;
        public bool? Deleted { get; set; } = false;
        public string Color { get; set; }

        public virtual ProductDTO Product { get; set; }
        public virtual StoreDTO Store { get; set; }
        public virtual ICollection<HoseDTO> Hoses { get; set; }
    }
}
