using System;
using System.Collections.Generic;

namespace APIControlNet.Models
{
    public partial class StoreNetwork
    {
        public int StoreNetworkIdx { get; set; }
        public Guid? StoreNetworkId { get; set; }
        public int? StoreNetworkNumber { get; set; }
        public string Description { get; set; }
        public DateTime? Date { get; set; }
        public DateTime? Updated { get; set; }
        public byte? Active { get; set; }
        public byte? Locked { get; set; }
        public byte? Deleted { get; set; }
    }
}
