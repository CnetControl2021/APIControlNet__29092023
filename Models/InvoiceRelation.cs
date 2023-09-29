using System;
using System.Collections.Generic;

namespace APIControlNet.Models
{
    public partial class InvoiceRelation
    {
        public int InvoiceRelatedIdx { get; set; }
        public Guid? InvoiceId { get; set; }
        public Guid? Uuid { get; set; }
        public Guid? InvoiceIdRelated { get; set; }
        public DateTime? Date { get; set; }
        public DateTime? Updated { get; set; }
        public byte? Active { get; set; }
        public byte? Locked { get; set; }
        public byte? Deleted { get; set; }
    }
}
