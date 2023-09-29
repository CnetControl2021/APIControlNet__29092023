using System;
using System.Collections.Generic;

namespace APIControlNet.Models
{
    public partial class JsonTipoRegistro
    {
        public int JsonTipoRegistroIdx { get; set; }
        public int JsonTipoRegistroId { get; set; }
        public string Descripcion { get; set; }
        public DateTime? Date { get; set; }
        public DateTime? Updated { get; set; }
        public int? Active { get; set; }
        public int? Locked { get; set; }
        public int? Deleted { get; set; }
    }
}
