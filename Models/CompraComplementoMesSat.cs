using System;
using System.Collections.Generic;

namespace APIControlNet.Models
{
    public partial class CompraComplementoMesSat
    {
        public int IdxCompraComplementoSat { get; set; }
        public Guid? IdComplementoSat { get; set; }
        public Guid? InventoryIn { get; set; }
        public string TipoComplementoSat { get; set; }
        public string Cfdi { get; set; }
        public string TipoCfdi { get; set; }
        public decimal? PrecioVenta { get; set; }
        public decimal? PrecioVentaPublico { get; set; }
        public string UnidadMedidaSat { get; set; }
        public string AclaracionesSat { get; set; }
        public long? Updated { get; set; }
        public byte? Active { get; set; }
        public byte? Locked { get; set; }
        public byte? Deleted { get; set; }
        public string Name { get; set; }
    }
}
