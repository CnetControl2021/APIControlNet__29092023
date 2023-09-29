using APIControlNet.Models;
using System.ComponentModel.DataAnnotations;

namespace APIControlNet.DTOs
{
    public class SupplierTransportDTO
    {
        public SupplierTransportDTO()
        {
            InventoryInDocuments = new HashSet<InventoryInDocumentDTO>();
        }

        public int SupplierTransportIdx { get; set; }
        public Guid SupplierId { get; set; }
        public Guid StoreId { get; set; }
        public int SupplierTransportIdi { get; set; }
        public string Name { get; set; }
        public string BrandName { get; set; }
        [Required(ErrorMessage = "El RFC es obligatorio")]
        [RegularExpression(@"^[A-Za-zñÑ&]{3,4}\d{6}\w{3}$", ErrorMessage = "El RFC no es válido")]
        public string Rfc { get; set; }
        public string TransportPermission { get; set; }
        public DateTime? Date { get; set; } = DateTime.Now;
        public DateTime? Updated { get; set; } = DateTime.Now;
        public bool? Active { get; set; } = true;
        public bool? Locked { get; set; }=false;
        public bool? Deleted { get; set; } = false;

        public virtual SupplierDTO Supplier { get; set; }
        public virtual ICollection<InventoryInDocumentDTO> InventoryInDocuments { get; set; }

    }
}
