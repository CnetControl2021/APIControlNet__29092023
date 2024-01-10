using System;
using System.Collections.Generic;

namespace APIControlNet.Models
{
    public partial class Supplier
    {
        public Supplier()
        {
            SupplierAddresses = new HashSet<SupplierAddress>();
            SupplierFuels = new HashSet<SupplierFuel>();
            SupplierTransports = new HashSet<SupplierTransport>();
        }

        public int SupplierIdx { get; set; }
        public Guid SupplierId { get; set; }
        public int SupplierNumber { get; set; }
        public string SatRegimenFiscalId { get; set; }
        public int? SupplierAddressIdi { get; set; }
        public string Name { get; set; }
        public string ContactPerson { get; set; }
        public string AbbreviatedName { get; set; }
        public string Lastname { get; set; }
        public string MotherLastname { get; set; }
        public string Curp { get; set; }
        public string Rfc { get; set; }
        public string Email { get; set; }
        public string Cellular { get; set; }
        public string Phone { get; set; }
        public bool? IsSupplierOfFuel { get; set; }
        public bool? IsSupplierOfTransport { get; set; }
        public DateTime? Date { get; set; }
        public DateTime? Updated { get; set; }
        public bool? Active { get; set; }
        public bool? Locked { get; set; }
        public bool? Deleted { get; set; }

        public virtual SatRegimenFiscal SatRegimenFiscal { get; set; }
        public virtual ICollection<SupplierAddress> SupplierAddresses { get; set; }
        public virtual ICollection<SupplierFuel> SupplierFuels { get; set; }
        public virtual ICollection<SupplierTransport> SupplierTransports { get; set; }
    }
}
