using System;
using System.Collections.Generic;

namespace APIControlNet.Models
{
    public partial class DuctoMedidor
    {
        public string NumeroEstacion { get; set; }
        public int NumeroDucto { get; set; }
        public int NumeroMedidor { get; set; }
        public string ClaveSistemaMedicion { get; set; }
        public string Descripcion { get; set; }
        public DateTime? VigenciaCalibracion { get; set; }
        public decimal? IncertidumbreMedicion { get; set; }
    }
}
