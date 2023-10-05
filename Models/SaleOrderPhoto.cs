using System;
using System.Collections.Generic;

namespace APIControlNet.Models
{
    public partial class SaleOrderPhoto
    {
        public int SaleOrderPhotoIdx { get; set; }
        public Guid SaleOrderId { get; set; }
        public int? SaleOrderPhotoIdi { get; set; }
        public string Photo { get; set; }
        public DateTime? Date { get; set; }
        public DateTime? Updated { get; set; }
        public byte? Active { get; set; }
        public byte? Locked { get; set; }
        public byte? Deleted { get; set; }
    }
}
