using System;
using System.Collections.Generic;

namespace APIControlNet.Models
{
    public partial class TypeMovement
    {
        public TypeMovement()
        {
            StoreHouseMovements = new HashSet<StoreHouseMovement>();
        }

        public int TypeMovementIdx { get; set; }
        public string Name { get; set; }
        public DateTime? Date { get; set; }
        public DateTime? Updated { get; set; }
        public byte? Active { get; set; }
        public byte? Locked { get; set; }
        public byte? Deleted { get; set; }

        public virtual ICollection<StoreHouseMovement> StoreHouseMovements { get; set; }
    }
}
