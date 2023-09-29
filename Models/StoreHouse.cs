using System;
using System.Collections.Generic;

namespace APIControlNet.Models
{
    public partial class StoreHouse
    {
        public StoreHouse()
        {
            StoreHouseMovements = new HashSet<StoreHouseMovement>();
        }

        public int StoreHouseIdx { get; set; }
        public Guid StoreHouseId { get; set; }
        public Guid StoreId { get; set; }
        public string Name { get; set; }
        public DateTime? Date { get; set; }
        public DateTime? Updated { get; set; }
        public bool? Active { get; set; }
        public bool? Locked { get; set; }
        public bool? Deleted { get; set; }

        public virtual ICollection<StoreHouseMovement> StoreHouseMovements { get; set; }
    }
}
