using System;
using System.Collections.Generic;

namespace APIControlNet.Models
{
    public partial class SupplierTransportRegister
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
        public DateTime? Date { get; set; }
        public DateTime? Updated { get; set; }
        public int? Active { get; set; }
        public int? Locked { get; set; }
        public int? Deleted { get; set; }
    }
}
