using System;
using System.Collections.Generic;

namespace APIControlNet.Models
{
    public partial class StoreHouseMovementDetail
    {
        public int StoreHouseMovementDetailIdx { get; set; }
        public Guid StoreHouseMovementId { get; set; }
        public Guid ProductId { get; set; }
        public decimal? QuantityEntry { get; set; }
        public decimal? QuantityExit { get; set; }
        public decimal? QuantityTrasfer { get; set; }
        public string Location { get; set; }
        public DateTime? Date { get; set; }
        public DateTime? Updated { get; set; }
        public byte? Active { get; set; }
        public byte? Locked { get; set; }
        public byte? Deleted { get; set; }

        public virtual StoreHouseMovement StoreHouseMovement { get; set; }
    }
}
