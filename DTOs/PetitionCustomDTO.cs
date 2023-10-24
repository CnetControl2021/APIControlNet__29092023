namespace APIControlNet.DTOs
{
    public class PetitionCustomDTO
    {
        public int PetitionCustomsIdx { get; set; }
        public Guid PetitionCustomsId { get; set; }// = Guid.NewGuid();
        public string KeyOfImportationExportation { get; set; }
        public decimal KeyPointOfInletOrOulet { get; set; }
        public string SatPaisId { get; set; }
        public int TransportMediumnCustomsId { get; set; }
        public string NumberCustomsDeclaration { get; set; }
        public string Incoterms { get; set; }
        public decimal AmountOfImportationExportation { get; set; }
        public decimal QuantityDocumented { get; set; }
        public string JsonClaveUnidadMedidadId { get; set; }
        public DateTime? Date { get; set; } = DateTime.Now;
        public DateTime? Updated { get; set; } = DateTime.Now;
        public int? Active { get; set; } = 1;
        public int? Locked { get; set; } = 0;
        public int? Deleted { get; set; } = 0;

    }
}
