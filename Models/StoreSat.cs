using System;
using System.Collections.Generic;

namespace APIControlNet.Models
{
    public partial class StoreSat
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
        public DateTime? Date { get; set; }
        public DateTime? Updated { get; set; }
        public bool? Active { get; set; }
        public bool? Locked { get; set; }
        public bool? Deleted { get; set; }

        public virtual Store Store { get; set; }
    }
}
