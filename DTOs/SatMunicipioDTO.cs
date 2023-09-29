using APIControlNet.Models;

namespace APIControlNet.DTOs
{
    public class SatMunicipioDTO
    {
        public int SatMunicipioIdx { get; set; }
        public string SatMunicipioClave { get; set; }
        public string SatEstadoId { get; set; }
        public string SatMunicipioId { get; set; }
        public string Descripcion { get; set; }
        public DateTime? Date { get; set; }
        public DateTime? Updated { get; set; }
        public bool? Active { get; set; }
        public bool? Locked { get; set; }
        public bool? Deleted { get; set; }
    }
}
