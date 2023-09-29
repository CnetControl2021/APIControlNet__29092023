using APIControlNet.Models;

namespace APIControlNet.DTOs
{
    public class SatPaiDTO
    {
        public SatPaiDTO()
        {
            CompanyAddresses = new HashSet<CompanyAddressDTO>();
        }

        public int SatPaisIdx { get; set; }
        public string SatPaisId { get; set; }
        public string Descripcion { get; set; }
        public DateTime? Date { get; set; }
        public DateTime? Updated { get; set; }
        public bool? Active { get; set; }
        public bool? Locked { get; set; }
        public bool? Deleted { get; set; }

        public virtual ICollection<CompanyAddressDTO> CompanyAddresses { get; set; }
    }
}
