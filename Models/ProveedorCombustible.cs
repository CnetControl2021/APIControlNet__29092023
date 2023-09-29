using System;
using System.Collections.Generic;

namespace APIControlNet.Models
{
    public partial class ProveedorCombustible
    {
        public int IdxProvComb { get; set; }
        public int IdProvComb { get; set; }
        public int IdSucursal { get; set; }
        public string NombreMarca { get; set; }
        public string NombreProveedor { get; set; }
        public string TipoProveedor { get; set; }
        public string PermisoImportacion { get; set; }
        public string RfcProveedor { get; set; }
        public string PermisoProveedor { get; set; }
        public string PermisoAlmacenamientoYdistribucion { get; set; }
        public int Activo { get; set; }
        public long? Updated { get; set; }
        public byte? Active { get; set; }
        public byte? Locked { get; set; }
        public byte? Deleted { get; set; }
        public string Name { get; set; }
    }
}
