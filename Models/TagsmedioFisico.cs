using System;
using System.Collections.Generic;

namespace APIControlNet.Models
{
    public partial class TagsmedioFisico
    {
        public int IdtagmeFis { get; set; }
        public string MedioFisico { get; set; }
        public decimal? FolioSiguiente { get; set; }
        public byte? Tag { get; set; }
        public decimal? Idredes { get; set; }
    }
}
