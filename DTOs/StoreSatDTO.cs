using APIControlNet.Models;

namespace APIControlNet.DTOs
{
    public class StoreSatDTO
    {
        public int StoreSatIdx { get; set; }
        public Guid StoreId { get; set; }
        public string CrePermission { get; set; }
        public string SatPermission { get; set; }
        public string JsonTipoMedicionId { get; set; }
        public string JsonTipoReporteId { get; set; }
        public string RfcLegalRepresentative { get; set; }
        public string RfcSystemSupplier { get; set; }
        public string SystemDescriptionInstallation { get; set; }
        public string JsonTipoDistribucionId { get; set; }
        public string JsonTipoComplementoId { get; set; }
        public string JsonClaveInstalacionId { get; set; }
        public string SatReportType { get; set; }
        public string SatSystemMeasureTank { get; set; }
        public string SatDescriptionInstallation { get; set; }
        public string SatInstallationKey { get; set; }
        public string SatRfcSupplier { get; set; }
        public DateTime? Date { get; set; } = DateTime.Now;
        public DateTime? Updated { get; set; } = DateTime.Now;
        public bool? Active { get; set; } = true;
        public bool? Locked { get; set; } = false;
        public bool? Deleted { get; set; } = false;

        public virtual StoreDTO Store { get; set; }

    }
}
