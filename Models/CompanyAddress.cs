using System;
using System.Collections.Generic;

namespace APIControlNet.Models
{
    public partial class CompanyAddress
    {
        public int CompanyAddressIdx { get; set; }
        public Guid CompanyId { get; set; }
        public int? CompanyAddressIdi { get; set; }
        public string Description { get; set; }
        public string SatPaisId { get; set; }
        public string SatEstadoId { get; set; }
        public string SatMunicipioId { get; set; }
        public string SatLocalidadId { get; set; }
        public string Street { get; set; }
        public string OutsideNumber { get; set; }
        public string InsideNumber { get; set; }
        public string Colony { get; set; }
        public string SatCodigoPostalId { get; set; }
        public DateTime? Date { get; set; }
        public DateTime? Updated { get; set; }
        public bool? Active { get; set; }
        public bool? Locked { get; set; }
        public bool? Deleted { get; set; }

        public virtual Company Company { get; set; }
    }
}
