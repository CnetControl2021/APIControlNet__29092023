using APIControlNet.Models;

namespace APIControlNet.DTOs
{
    public class SatFormaPagoDTO
    {
        public SatFormaPagoDTO()
        {
            Customers = new HashSet<CustomerDTO>();
        }

        public int SatFormaPagoIdx { get; set; }
        public string SatFormaPagoId { get; set; }
        public string Descripcion { get; set; }
        public DateTime? FechaInicioVigencia { get; set; }
        public DateTime? FechaFinVigencia { get; set; }
        public bool? ActiveInvoiceOnFinalCustomer { get; set; }
        public DateTime? Date { get; set; }
        public DateTime? Updated { get; set; }
        public bool? Active { get; set; }
        public bool? Locked { get; set; }
        public bool? Deleted { get; set; }

        public virtual ICollection<CustomerDTO> Customers { get; set; }
    }
}
