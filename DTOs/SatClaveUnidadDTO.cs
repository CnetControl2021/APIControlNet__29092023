using APIControlNet.Models;

namespace APIControlNet.DTOs
{
    public class SatClaveUnidadDTO
    {
        public SatClaveUnidadDTO()
        {
            Products = new HashSet<ProductDTO>();
        }

        public int SatClaveUnidadIdx { get; set; }
        public string SatClaveUnidadId { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public DateTime? Date { get; set; }
        public DateTime? Updated { get; set; }
        public bool? Active { get; set; }
        public bool? Locked { get; set; }
        public bool? Deleted { get; set; }

        public virtual ICollection<ProductDTO> Products { get; set; }
    }
}
