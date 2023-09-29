using System;
using System.Collections.Generic;

namespace APIControlNet.Models
{
    public partial class TankBrand
    {
        public TankBrand()
        {
            Tanks = new HashSet<Tank>();
        }

        public int TankBrandIdx { get; set; }
        public int TankBrandId { get; set; }
        public string Name { get; set; }
        public DateTime? Date { get; set; }
        public DateTime? Updated { get; set; }
        public bool? Active { get; set; }
        public bool? Locked { get; set; }
        public bool? Deleted { get; set; }

        public virtual ICollection<Tank> Tanks { get; set; }
    }
}
