using APIControlNet.Models;

namespace APIControlNet.DTOs
{
    public class SatRegimenFiscalDTO
    {
        public SatRegimenFiscalDTO()
        {
            Customers = new HashSet<CustomerDTO>();
            SatRegimenfiscalUsocfdis = new HashSet<SatRegimenfiscalUsocfdiDTO>();
            Suppliers = new HashSet<SupplierDTO>();
        }

        public int SatRegimenFiscalIdx { get; set; }
        public string SatRegimenFiscalId { get; set; }
        public string Descripcion { get; set; }
        public string Fisica { get; set; }
        public string Moral { get; set; }
        public DateTime? FechaInicioVigencia { get; set; }
        public DateTime? FechaFinVigencia { get; set; }
        public bool? Active { get; set; }
        public bool? Locked { get; set; }
        public bool? Deleted { get; set; }

        public virtual ICollection<CustomerDTO> Customers { get; set; }
        public virtual ICollection<SatRegimenfiscalUsocfdiDTO> SatRegimenfiscalUsocfdis { get; set; }
        public virtual ICollection<SupplierDTO> Suppliers { get; set; }
    }
}
