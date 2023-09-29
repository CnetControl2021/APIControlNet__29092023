using System;
using System.Collections.Generic;

namespace APIControlNet.Models
{
    public partial class StoreHouseMovement
    {
        public StoreHouseMovement()
        {
            StoreHouseMovementDetails = new HashSet<StoreHouseMovementDetail>();
        }

        public int StoreHouseMovementIdx { get; set; }
        public Guid StoreHouseMovementId { get; set; }
        public int? TypeMovementIdx { get; set; }
        public Guid? ProviderId { get; set; }
        public Guid? CustomerId { get; set; }
        public Guid? StoreHouseIdOrigin { get; set; }
        public Guid? StoreHouseIdDestination { get; set; }
        public DateTime? Date { get; set; }
        public DateTime? Updated { get; set; }
        public byte? Active { get; set; }
        public byte? Locked { get; set; }
        public byte? Deleted { get; set; }

        public virtual StoreHouse StoreHouseIdDestinationNavigation { get; set; }
        public virtual TypeMovement TypeMovementIdxNavigation { get; set; }
        public virtual ICollection<StoreHouseMovementDetail> StoreHouseMovementDetails { get; set; }
    }
}
