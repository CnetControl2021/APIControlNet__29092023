using System;
using System.Collections.Generic;

namespace APIControlNet.Models
{
    public partial class NetDetail
    {
        public int NetDetailIdx { get; set; }
        public Guid NetId { get; set; }
        public Guid StoreId { get; set; }
        public DateTime? Date { get; set; }
        public DateTime? Updated { get; set; }
        public byte? Active { get; set; }
        public byte? Locked { get; set; }
        public byte? Deleted { get; set; }
    }
}
