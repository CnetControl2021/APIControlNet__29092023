using System;
using System.Collections.Generic;

namespace APIControlNet.Models
{
    public partial class Company
    {
        public Company()
        {
            CompanyAddresses = new HashSet<CompanyAddress>();
            Stores = new HashSet<Store>();
        }

        public int CompanyIdx { get; set; }
        public Guid CompanyId { get; set; }
        public string Name { get; set; }
        public string TradeName { get; set; }
        public string Rfc { get; set; }
        public string SatRegimenFiscalId { get; set; }
        public DateTime? Date { get; set; }
        public DateTime? Updated { get; set; }
        public bool? Active { get; set; }
        public bool? Locked { get; set; }
        public bool? Deleted { get; set; }

        public virtual ICollection<CompanyAddress> CompanyAddresses { get; set; }
        public virtual ICollection<Store> Stores { get; set; }
    }
}
