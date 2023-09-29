using System;
using System.Collections.Generic;

namespace APIControlNet.Models
{
    public partial class Hose
    {
        public int HoseIdx { get; set; }
        public Guid StoreId { get; set; }
        public int? HoseIdi { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int LoadPositionIdi { get; set; }
        public Guid? ProductId { get; set; }
        public int? CpuAddressHose { get; set; }
        public int? Position { get; set; }
        public decimal? SlowFlow { get; set; }
        public DateTime? Date { get; set; }
        public DateTime? Updated { get; set; }
        public bool? Active { get; set; }
        public bool? Locked { get; set; }
        public bool? Deleted { get; set; }

        public virtual LoadPosition LoadPosition { get; set; }
        public virtual ProductStore ProductStore { get; set; }
        public virtual Store Store { get; set; }
    }
}
