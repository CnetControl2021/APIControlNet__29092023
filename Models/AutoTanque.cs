using System;
using System.Collections.Generic;

namespace APIControlNet.Models
{
    public partial class AutoTanque
    {
        public int AutoTanqueId { get; set; }
        public string NumeroEmpresa { get; set; }
        public string NumeroEstacion { get; set; }
        public string TipoAutoTanque { get; set; }
        public int NumeroAutoTanque { get; set; }
        public string Descripcion { get; set; }
        public string NumeroProducto { get; set; }
        public string UnidadMedida { get; set; }
        public decimal CapacidadTotal { get; set; }
        public decimal CapacidadOperativa { get; set; }
        public decimal CapacidadUtil { get; set; }
        public decimal CapacidadFondaje { get; set; }
        public decimal CapacidadGasTalon { get; set; }
        public decimal VolumenMinimoOperacion { get; set; }
        public string EstadoTanque { get; set; }
        public string TipoSistMedicion { get; set; }
        public string LocalizOdescripSistMedicion { get; set; }
        public decimal IncertidumbreMedicionSistMedicion { get; set; }
        public string TipoMedioAlmacenamiento { get; set; }
        public DateTime? VigenciaCalibracionSistMedicion { get; set; }
    }
}
