using System;
using System.Collections.Generic;

namespace APIControlNet.Models
{
    public partial class CompanyCustomer
    {
        public int CompanyCustomerIdx { get; set; }
        public Guid CompanyId { get; set; }
        public Guid CustomerId { get; set; }
        public DateTime? Date { get; set; }
        public DateTime? Updated { get; set; }
        public bool? Active { get; set; }
        public bool? Locked { get; set; }
        public bool? Deleted { get; set; }
    }
}
