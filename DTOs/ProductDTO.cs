using APIControlNet.Models;
using System.Text.Json.Serialization;
///okko
namespace APIControlNet.DTOs
{
    public class ProductDTO
    {
        public ProductDTO()
        {
            DailySummaries = new HashSet<DailySummaryDTO>();
            MonthlySummaries = new HashSet<MonthlySummaryDTO>();
            ProductSats = new HashSet<ProductSatDTO>();
            ProductStores = new HashSet<ProductStoreDTO>();
        }

        public int ProductIdx { get; set; }
        public Guid ProductId { get; set; } = Guid.NewGuid();
        public string ProductCode { get; set; }
        public string SatClaveProductoServicioId { get; set; }
        public string SatClaveUnidadId { get; set; }
        public bool? IsFuel { get; set; }
        public string Name { get; set; }
        public Guid ProductCategoryId { get; set; }
        public string Barcode { get; set; }
        public DateTime? Date { get; set; }= DateTime.Now;
        public DateTime? Updated { get; set; }=DateTime.Now;
        public bool? Active { get; set; } = true;
        public bool? Locked { get; set; } = false;
        public bool? Deleted { get; set; }=false;
        public string AppName { get; set; }
        public string Color { get; set; }

        public virtual ProductCategoryDTO ProductCategory { get; set; }
        public virtual SatClaveProductoServicioDTO SatClaveProductoServicio { get; set; }
        public virtual SatClaveUnidadDTO SatClaveUnidad { get; set; }
        public virtual ICollection<DailySummaryDTO> DailySummaries { get; set; }
        public virtual ICollection<MonthlySummaryDTO> MonthlySummaries { get; set; }
        public virtual ICollection<ProductSatDTO> ProductSats { get; set; }
        public virtual ICollection<ProductStoreDTO> ProductStores { get; set; }
    }
}
