using System;
using System.Collections.Generic;

namespace APIControlNet.Models
{
    public partial class UciCfg
    {
        public int IdxUciCfg { get; set; }
        public int IdUciCfg { get; set; }
        public int? IdPuerto { get; set; }
        public int? IdSucursal { get; set; }
        public byte? Enable { get; set; }
        public int? Address { get; set; }
        public int? TipoUci { get; set; }
        public long? FechaUpd { get; set; }
        public int? PrnBaud { get; set; }
        public int? IdImpresora { get; set; }
        public int? TipoEnvioAutorizacion { get; set; }
        public long? Updated { get; set; }
        public byte? Active { get; set; }
        public byte? Locked { get; set; }
        public byte? Deleted { get; set; }
        public string Name { get; set; }
    }
}
