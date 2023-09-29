using System;
using System.Collections.Generic;

namespace APIControlNet.Models
{
    public partial class PetitionCustom
    {
        public int PetitionCustomsIdx { get; set; }
        public Guid PetitionCustomsId { get; set; }
        public string KeyOfImportationExportation { get; set; }
        public decimal KeyPointOfInletOrOulet { get; set; }
        public string SatPaisId { get; set; }
        public int TransportMediumnCustomsId { get; set; }
        public string NumberCustomsDeclaration { get; set; }
        public string Incoterms { get; set; }
        public decimal AmountOfImportationExportation { get; set; }
        public decimal QuantityDocumented { get; set; }
        public string JsonClaveUnidadMedidadId { get; set; }
        public DateTime? Date { get; set; }
        public DateTime? Updated { get; set; }
        public int? Active { get; set; }
        public int? Locked { get; set; }
        public int? Deleted { get; set; }
    }
}
