using APIControlNet.Models;

namespace APIControlNet.DTOs
{
    public class SatLocalidadDTO
    {
        public SatLocalidadDTO()
        {
            CompanyAddresses = new HashSet<CompanyAddressDTO>();
        }

        public int SatLocalidadIdx { get; set; }
        public string SatLocalidadClave { get; set; }
        public string SatEstadoId { get; set; }
        public string SatLocalidadId { get; set; }
        public string Descripción { get; set; }
        public DateTime? Date { get; set; }
        public DateTime? Updated { get; set; }
        public bool? Active { get; set; }
        public bool? Locked { get; set; }
        public bool? Deleted { get; set; }

        public virtual ICollection<CompanyAddressDTO> CompanyAddresses { get; set; }
    }
}
