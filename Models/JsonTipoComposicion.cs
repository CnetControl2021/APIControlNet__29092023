using System;
using System.Collections.Generic;

namespace APIControlNet.Models
{
    public partial class JsonTipoComposicion
    {
        public int JsonTipoComposicionIdx { get; set; }
        public string JsonTipoComposicionId { get; set; }
        public string Descripcion { get; set; }
        public DateTime? Date { get; set; }
        public DateTime? Updated { get; set; }
        public int? Active { get; set; }
        public int? Locked { get; set; }
        public int? Deleted { get; set; }
    }
}
