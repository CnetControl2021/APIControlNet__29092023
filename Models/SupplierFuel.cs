using System;
using System.Collections.Generic;

namespace APIControlNet.Models
{
    public partial class SupplierFuel
    {
        public int SupplierFuelIdx { get; set; }
        public Guid SupplierId { get; set; }
        public Guid StoreId { get; set; }
        public int SupplierFuelIdi { get; set; }
        public string BrandName { get; set; }
        public string Name { get; set; }
        public string SupplierType { get; set; }
        public string ImportPermission { get; set; }
        public string Rfc { get; set; }
        public string FuelPermission { get; set; }
        public string StorageAndDistributionPermission { get; set; }
        public string IsConsignment { get; set; }
        public DateTime? Date { get; set; }
        public DateTime? Updated { get; set; }
        public bool? Active { get; set; }
        public bool? Locked { get; set; }
        public bool? Deleted { get; set; }

        public virtual Supplier Supplier { get; set; }
    }
}
