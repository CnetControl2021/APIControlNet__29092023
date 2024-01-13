using APIControlNet.Models;
using System.ComponentModel.DataAnnotations;

namespace APIControlNet.DTOs
{
    public class SupplierTransportDTO
    {
        public int SupplierTransportIdx { get; set; }
        public Guid SupplierId { get; set; }
        public Guid StoreId { get; set; }
        public int SupplierTransportIdi { get; set; }
        public string Name { get; set; }
        public string BrandName { get; set; }
        public string Rfc { get; set; }
        public string TransportPermission { get; set; }
        public DateTime? Date { get; set; }
        public DateTime? Updated { get; set; }
        public bool? Active { get; set; }
        public bool? Locked { get; set; }
        public bool? Deleted { get; set; }

        public virtual SupplierDTO Supplier { get; set; }

    }
}
