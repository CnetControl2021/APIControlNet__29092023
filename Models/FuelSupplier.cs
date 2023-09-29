using System;
using System.Collections.Generic;

namespace APIControlNet.Models
{
    public partial class FuelSupplier
    {
        public int IdxFuelSupplier { get; set; }
        public Guid FuelSupplierId { get; set; }
        public Guid StoreId { get; set; }
        public string BrandName { get; set; }
        public string Name { get; set; }
        public string ProviderType { get; set; }
        public string ImportPermission { get; set; }
        public string ProviderRfc { get; set; }
        public string ProviderPermission { get; set; }
        public string StorageAndDistributionPermission { get; set; }
        public DateTime? Date { get; set; }
        public DateTime? Updated { get; set; }
        public byte? Active { get; set; }
        public byte? Locked { get; set; }
        public byte? Deleted { get; set; }

        public virtual Store Store { get; set; }
    }
}
