using System;
using System.Collections.Generic;

namespace APIControlNet.Models
{
    public partial class SatUsoCfdi
    {
        public SatUsoCfdi()
        {
            Customers = new HashSet<Customer>();
            SatRegimenfiscalUsocfdis = new HashSet<SatRegimenfiscalUsocfdi>();
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

        public virtual ICollection<Customer> Customers { get; set; }
        public virtual ICollection<SatRegimenfiscalUsocfdi> SatRegimenfiscalUsocfdis { get; set; }
    }
}
