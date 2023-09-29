using System;
using System.Collections.Generic;

namespace APIControlNet.Models
{
    public partial class SatRegimenFiscal
    {
        public SatRegimenFiscal()
        {
            Customers = new HashSet<Customer>();
            SatRegimenfiscalUsocfdis = new HashSet<SatRegimenfiscalUsocfdi>();
            Suppliers = new HashSet<Supplier>();
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

        public virtual ICollection<Customer> Customers { get; set; }
        public virtual ICollection<SatRegimenfiscalUsocfdi> SatRegimenfiscalUsocfdis { get; set; }
        public virtual ICollection<Supplier> Suppliers { get; set; }
    }
}
