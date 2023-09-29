using System;
using System.Collections.Generic;

namespace APIControlNet.Models
{
    public partial class License
    {
        public int LicenseIdx { get; set; }
        public Guid? LicenseId { get; set; }
        public Guid? StoreId { get; set; }
        public string SystemName { get; set; }
        public string UniqueId { get; set; }
        public string LicenseKey { get; set; }
        public string LicenseValue { get; set; }
        public DateTime? EndDate { get; set; }
        public DateTime? Updated { get; set; }
        public byte? Active { get; set; }
        public byte? Locked { get; set; }
        public byte? Deleted { get; set; }
        public string Value { get; set; }
    }
}
