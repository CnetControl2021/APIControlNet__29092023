using System;
using System.Collections.Generic;

namespace APIControlNet.Models
{
    public partial class TransportSupplier
    {
        public int IdxTransportSupplier { get; set; }
        public Guid TransportSupplierId { get; set; }
        public Guid StoreId { get; set; }
        public string Name { get; set; }
        public string TradeName { get; set; }
        public string TransportProviderRfc { get; set; }
        public string TransportPermission { get; set; }
        public DateTime? Date { get; set; }
        public DateTime? Updated { get; set; }
        public byte? Active { get; set; }
        public byte? Locked { get; set; }
        public byte? Deleted { get; set; }

        public virtual Store Store { get; set; }
    }
}
