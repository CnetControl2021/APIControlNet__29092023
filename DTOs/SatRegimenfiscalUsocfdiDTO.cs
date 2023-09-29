using APIControlNet.Models;

namespace APIControlNet.DTOs
{
    public class SatRegimenfiscalUsocfdiDTO
    {
        public int SatRegimenfiscalUsocfdi1 { get; set; }
        public string SatRegimenFiscalId { get; set; }
        public string SatUsoCfdiId { get; set; }

        public virtual SatRegimenFiscalDTO SatRegimenFiscal { get; set; }
        public virtual SatUsoCfdiDTO SatUsoCfdi { get; set; }
    }
}
