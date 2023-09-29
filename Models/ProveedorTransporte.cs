using System;
using System.Collections.Generic;

namespace APIControlNet.Models
{
    public partial class ProveedorTransporte
    {
        public int IdxProvTran { get; set; }
        public int IdProvTran { get; set; }
        public int IdSucursal { get; set; }
        public string NombreMarca { get; set; }
        public string PermisoTransporte { get; set; }
        public string NombreTransporte { get; set; }
        public int Activo { get; set; }
        public long? Updated { get; set; }
        public byte? Active { get; set; }
        public byte? Locked { get; set; }
        public byte? Deleted { get; set; }
        public string Name { get; set; }
    }
}
