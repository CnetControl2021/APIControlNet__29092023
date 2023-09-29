using System;
using System.Collections.Generic;

namespace APIControlNet.Models
{
    public partial class VolModuloTipoEvento
    {
        public int IdFolio { get; set; }
        public int IdModulo { get; set; }
        public string IdEvento { get; set; }
        public string Descripcion { get; set; }
        public DateTime Fecha { get; set; }
    }
}
