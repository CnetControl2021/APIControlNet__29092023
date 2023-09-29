using APIControlNet.Models;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace APIControlNet.DTOs
{
    public class CompanyDTO
    {
        public CompanyDTO()
        {
            CompanyAddresses = new HashSet<CompanyAddressDTO>();
            Stores = new HashSet<StoreDTO>();
        }

        public int CompanyIdx { get; set; }
        public Guid CompanyId { get; set; }= Guid.NewGuid();
        public string Name { get; set; }
        public string TradeName { get; set; }
        public string Rfc { get; set; }
        public string SatRegimenFiscalId { get; set; }
        public DateTime? Date { get; set; } = DateTime.Now;
        public DateTime? Updated { get; set; } = DateTime.Now;
        public bool? Active { get; set; } = true;
        public bool? Locked { get; set; } = false;
        public bool? Deleted { get; set; }=false;

        public virtual ICollection<CompanyAddressDTO> CompanyAddresses { get; set; }
        public virtual ICollection<StoreDTO> Stores { get; set; }
    }
}
