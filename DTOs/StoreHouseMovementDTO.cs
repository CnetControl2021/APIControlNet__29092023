namespace APIControlNet.DTOs
{
    public class StoreHouseMovementDTO
    {
        //public StoreHouseMovement()
        //{
        //    StoreHouseMovementDetails = new HashSet<StoreHouseMovementDetail>();
        //}

        public int StoreHouseMovementIdx { get; set; }
        public Guid StoreHouseMovementId { get; set; } = Guid.NewGuid();
        public int? TypeMovementIdx { get; set; }
        public Guid? ProviderId { get; set; }
        public Guid? CustomerId { get; set; }
        public Guid? StoreHouseIdOrigin { get; set; }
        public Guid? StoreHouseIdDestination { get; set; }
        public DateTime? Date { get; set; } = DateTime.Now;
        public DateTime? Updated { get; set; } = DateTime.Now;
        public byte? Active { get; set; }
        public byte? Locked { get; set; }
        public byte? Deleted { get; set; }


        public virtual ICollection<StoreHouseMovementDetailDTO> StoreHouseMovementDetails { get; set; }
        public virtual TypeMovementDTO TypeMovementIdxNavigation { get; set; }
        public virtual StoreHouseDTO StoreHouseIdDestinationNavigation { get; set; }

    }

}

