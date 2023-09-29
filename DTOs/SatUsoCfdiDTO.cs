using APIControlNet.Models;

namespace APIControlNet.DTOs
{
    public class SatUsoCfdiDTO
    {
        public SatUsoCfdiDTO()
        {
            Customers = new HashSet<CustomerDTO>();
            SatRegimenfiscalUsocfdis = new HashSet<SatRegimenfiscalUsocfdiDTO>();
        }

        public int SatUsoCdfiIdx { get; set; }
        public string SatUsoCfdiId { get; set; }
        public string Decripcion { get; set; }
        public string Fisica { get; set; }
        public string Moral { get; set; }
        public DateTime? FechaInicioVigencia { get; set; }
        public DateTime? FechaFinVigencia { get; set; }
        public string RegimenFiscalReceptor { get; set; }
        public bool? Active { get; set; }
        public bool? Locked { get; set; }
        public bool? Deleted { get; set; }

        public virtual ICollection<CustomerDTO> Customers { get; set; }
        public virtual ICollection<SatRegimenfiscalUsocfdiDTO> SatRegimenfiscalUsocfdis { get; set; }
    }
}
