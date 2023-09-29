using APIControlNet.Models;
using System.ComponentModel.DataAnnotations;

namespace APIControlNet.DTOs
{
    public class SupplierDTO
    {
        public SupplierDTO()
        {
            SupplierAddresses = new HashSet<SupplierAddressDTO>();
        }

        public int SupplierIdx { get; set; }
        public Guid SupplierId { get; set; } = Guid.NewGuid();
        public int? SupplierNumber { get; set; }
        public string SatRegimenFiscalId { get; set; }
        public int? SupplierAddressIdi { get; set; }
        public string Name { get; set; }
        public string ContactPerson { get; set; }
        public string AbbreviatedName { get; set; }
        public string Lastname { get; set; }
        public string MotherLastname { get; set; }
        public string Curp { get; set; }

        [Required(ErrorMessage = "El RFC es obligatorio")]
        [RegularExpression(@"^[A-Za-zñÑ&]{3,4}\d{6}\w{3}$", ErrorMessage = "El RFC no es válido")]
        public string Rfc { get; set; }
        public string Email { get; set; }
        public string Cellular { get; set; }
        public string Phone { get; set; }
        public bool? IsSupplierOfFuel { get; set; }
        public bool? IsSupplierOfTransport { get; set; }
        public DateTime? Date { get; set; } = DateTime.Now;
        public DateTime? Updated { get; set; } = DateTime.Now;
        public bool? Active { get; set; } = true;
        public bool? Locked { get; set; } = false;
        public bool? Deleted { get; set; }=false;

        public virtual SatRegimenFiscalDTO SatRegimenFiscal { get; set; }
        public virtual ICollection<SupplierAddressDTO> SupplierAddresses { get; set; }

    }
}
