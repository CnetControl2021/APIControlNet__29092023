using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace APIControlNet.DTOs
{
    public class SatCodigoPostalDTO
    {
        public int SatCodigoPostalIdx { get; set; }
        public string SatCodigoPostalId { get; set; }
        public string SatEstadoId { get; set; }
        public string SatMunicipioClave { get; set; }
        public string SatLocalidadClave { get; set; }
        public DateTime? Date { get; set; }
        public DateTime? Updated { get; set; }
        public bool? Active { get; set; }
        public bool? Locked { get; set; }
        public bool? Deleted { get; set; }

    }

}
