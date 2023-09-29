using System;
using System.Collections.Generic;

namespace APIControlNet.Models
{
    public partial class SatRegimenfiscalUsocfdi
    {
        public int SatRegimenfiscalUsocfdi1 { get; set; }
        public string SatRegimenFiscalId { get; set; }
        public string SatUsoCfdiId { get; set; }

        public virtual SatRegimenFiscal SatRegimenFiscal { get; set; }
        public virtual SatUsoCfdi SatUsoCfdi { get; set; }
    }
}
