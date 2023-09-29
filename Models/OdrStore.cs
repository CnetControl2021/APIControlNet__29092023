using System;
using System.Collections.Generic;

namespace APIControlNet.Models
{
    public partial class OdrStore
    {
        public int OdrStoreIdx { get; set; }
        public Guid? OdrStoreId { get; set; }
        public Guid? CustomerId { get; set; }
        public Guid? StoreId { get; set; }
        public DateTime? Date { get; set; }
        public DateTime? Updated { get; set; }
        public bool? Active { get; set; }
        public bool? Locked { get; set; }
        public bool? Deleted { get; set; }
    }
}
