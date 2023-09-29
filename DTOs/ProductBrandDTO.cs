using System.ComponentModel.DataAnnotations;

namespace APIControlNet.DTOs
{
    public class ProductBrandDTO
    {
        public ProductBrandDTO()
        {
            Products = new HashSet<ProductDTO>();
        }

        public int ProductBrandIdx { get; set; }
        public Guid ProductBrandId { get; set; } = Guid.NewGuid();
        [Required(ErrorMessage = "El campo {0} es requerio")]
        public string Name { get; set; }
        public DateTime? Date { get; set; }
        public DateTime? Updated { get; set; }
        public bool? Active { get; set; } = true;
        public bool? Locked { get; set; }=false;
        public bool? Deleted { get; set; }=false ;

        public virtual ICollection<ProductDTO> Products { get; set; }
    }
}
