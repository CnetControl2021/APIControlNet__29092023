using System;
using System.Collections.Generic;

namespace APIControlNet.Models
{
    public partial class ClienteProveedorCvol
    {
        public int RegistroId { get; set; }
        public string NumeroEstacion { get; set; }
        public int NumeroRegistro { get; set; }
        public string Tipo { get; set; }
        public string Nombre { get; set; }
        public string Rfc { get; set; }
        public string PermisoCre { get; set; }
    }
}
