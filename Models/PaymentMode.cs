using System;
using System.Collections.Generic;

namespace APIControlNet.Models
{
    public partial class PaymentMode
    {
        public PaymentMode()
        {
            Customers = new HashSet<Customer>();
        }

        public int PaymentModeIdx { get; set; }
        public Guid PaymentModeId { get; set; }
        public int? PaymentModeNumber { get; set; }
        public string Name { get; set; }
        public int? ActiveInvoiceOnFinalCustomer { get; set; }
        public DateTime? Date { get; set; }
        public DateTime? Updated { get; set; }
        public bool? Active { get; set; }
        public bool? Locked { get; set; }
        public bool? Deleted { get; set; }
        public string PaymentModeSat { get; set; }
        public string NameSat { get; set; }

        public virtual ICollection<Customer> Customers { get; set; }
    }
}
