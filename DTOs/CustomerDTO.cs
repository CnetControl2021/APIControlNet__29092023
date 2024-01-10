using APIControlNet.Models;
using System.ComponentModel.DataAnnotations;

namespace APIControlNet.DTOs
{
    public class CustomerDTO
    {
        public CustomerDTO()
        {
            Vehicles = new HashSet<VehicleDTO>();
        }

        public int CustomerIdx { get; set; }
        public Guid CustomerId { get; set; }=Guid.NewGuid();
        public int? CustomerNumber { get; set; }
        public int? CustomerTypeId { get; set; }
        public string SatFormaPagoId { get; set; }
        public string SatRegimenFiscalId { get; set; }
        public string SatUsoCfdiId { get; set; }
        public int? CustomerAddressIdi { get; set; }
        public string Name { get; set; }
        public string AbbreviatedName { get; set; }
        public string Lastname { get; set; }
        public string MotherLastname { get; set; }
        public string Curp { get; set; }
        public string Rfc { get; set; }
        public string Email { get; set; }
        public string Cellular { get; set; }
        public string Phone { get; set; }
        public DateTime? Date { get; set; } = DateTime.Now;
        public DateTime? Updated { get; set; } = DateTime.Now;
        public bool? Active { get; set; } = true;
        public bool? Locked { get; set; } = false;
        public bool? Deleted { get; set; } = false;
        public string ContactPerson { get; set; }
        public string SatConsignmentSale { get; set; }

        public virtual CustomerTypeDTO CustomerType { get; set; }
        public virtual SatFormaPagoDTO SatFormaPago { get; set; }
        public virtual SatRegimenFiscalDTO SatRegimenFiscal { get; set; }
        public virtual SatUsoCfdiDTO SatUsoCfdi { get; set; }
        public virtual ICollection<VehicleDTO> Vehicles { get; set; }
    }
}

