namespace APIControlNet.DTOs
{
    public class SupplierTransportRegisterDTO
    {
        public int SupplierTransportRegisterIdx { get; set; }
        public Guid SupplierTransportRegisterId { get; set; } 
        public Guid SupplierId { get; set; }
        public decimal AmountPerFee { get; set; }
        public decimal AmountPerCapacity { get; set; }
        public decimal AmountPerUse { get; set; }
        public decimal AmountPerVolume { get; set; }
        public decimal AmountPerService { get; set; }
        public decimal AmountOfDiscount { get; set; }
        public DateTime? Date { get; set; } = DateTime.Now;
        public DateTime? Updated { get; set; } = DateTime.Now;
        public int? Active { get; set; } = 1;
        public int? Locked { get; set; } = 0;
        public int? Deleted { get; set; } = 0;

    }
}
