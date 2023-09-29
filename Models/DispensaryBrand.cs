using System;
using System.Collections.Generic;

namespace APIControlNet.Models
{
    public partial class DispensaryBrand
    {
        public DispensaryBrand()
        {
            Dispensaries = new HashSet<Dispensary>();
        }

        public int DispensaryBrandIdx { get; set; }
        public int DispensaryBrandId { get; set; }
        public string Name { get; set; }
        public DateTime Date { get; set; }
        public DateTime Updated { get; set; }
        public bool? Active { get; set; }
        public bool? Locked { get; set; }
        public bool? Deleted { get; set; }

        public virtual ICollection<Dispensary> Dispensaries { get; set; }
    }
}
