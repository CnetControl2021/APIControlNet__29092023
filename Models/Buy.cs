using System;
using System.Collections.Generic;

namespace APIControlNet.Models
{
    public partial class Buy
    {
        public int BuyIdx { get; set; }
        public Guid? BuyId { get; set; }
        public Guid? StoreId { get; set; }
        public int? BuyNumber { get; set; }
        public Guid? EmployeeId { get; set; }
        public decimal? Amount { get; set; }
        public DateTime? Date { get; set; }
        public DateTime? Updated { get; set; }
        public byte? Active { get; set; }
        public byte? Locked { get; set; }
        public byte? Deleted { get; set; }
    }
}
