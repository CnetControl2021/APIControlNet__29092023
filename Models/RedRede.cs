using System;
using System.Collections.Generic;

namespace APIControlNet.Models
{
    public partial class RedRede
    {
        public int Idredes { get; set; }
        public string NombreRed { get; set; }
        public DateTime? FechaAlta { get; set; }
        public decimal? FolioUbn { get; set; }
        public decimal? Iduser { get; set; }
        public byte? PublicarMapa { get; set; }
        public decimal? Idredtag { get; set; }
        public int? IdredesTipo { get; set; }
    }
}
