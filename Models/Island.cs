using System;
using System.Collections.Generic;

namespace APIControlNet.Models
{
    public partial class Island
    {
        public Island()
        {
            LoadPositions = new HashSet<LoadPosition>();
        }

        public int IslandIdx { get; set; }
        public int IslandIdi { get; set; }
        public Guid StoreId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime? Date { get; set; }
        public DateTime? Updated { get; set; }
        public bool? Active { get; set; }
        public bool? Locked { get; set; }
        public bool? Deleted { get; set; }

        public virtual Store Store { get; set; }
        public virtual ICollection<LoadPosition> LoadPositions { get; set; }
    }
}
