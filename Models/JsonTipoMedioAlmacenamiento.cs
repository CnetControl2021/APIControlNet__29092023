using System;
using System.Collections.Generic;

namespace APIControlNet.Models
{
    public partial class JsonTipoMedioAlmacenamiento
    {
        public int JsonTipoMedioAlmacenamientoIdx { get; set; }
        public string JsonTipoMedioAlmacenamientoId { get; set; }
        public string Descripcion { get; set; }
        public DateTime? Date { get; set; }
        public DateTime? Updated { get; set; }
        public int? Active { get; set; }
        public int? Locked { get; set; }
        public int? Deleted { get; set; }
    }
}
