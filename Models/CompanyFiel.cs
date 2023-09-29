using System;
using System.Collections.Generic;

namespace APIControlNet.Models
{
    public partial class CompanyFiel
    {
        public int CompanyFielIdx { get; set; }
        public Guid CompanyId { get; set; }
        public string XmlPrivateKey { get; set; }
        public string FilePrivateKey { get; set; }
        public string PemPrivateKey { get; set; }
        public string PasswordPrivateKey { get; set; }
        public string FileCertificate { get; set; }
        public string CertificateNumber { get; set; }
        public DateTime? Updated { get; set; }
        public bool? Active { get; set; }
        public bool? Locked { get; set; }
        public bool? Deleted { get; set; }
        public DateTime? EndDate { get; set; }
        public DateTime? Date { get; set; }
        public string FielTypeId { get; set; }
        public DateTime? StartDate { get; set; }
    }
}
