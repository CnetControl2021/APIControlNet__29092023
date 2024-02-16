using System;
using System.Collections.Generic;

namespace APIControlNet.Models
{
    public partial class CustomerNetgroupNet
    {
        public int CustomerNetgroupNetIdx { get; set; }
        public Guid? CustomerId { get; set; }
        public Guid? NetgroupNetId { get; set; }
        public DateTime? Date { get; set; }
        public DateTime? Updated { get; set; }
        public bool? Active { get; set; }
        public bool? Locked { get; set; }
        public bool? Daleted { get; set; }
    }
}
