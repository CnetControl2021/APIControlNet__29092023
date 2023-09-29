using System;
using System.Collections.Generic;

namespace APIControlNet.Models
{
    public partial class NetgroupUser
    {
        public int NetgroupUserIdx { get; set; }
        public Guid NetgroupId { get; set; }
        public string UserId { get; set; }
        public Guid? NetgroupUserId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool MakeInvoice { get; set; }
        public DateTime? Date { get; set; }
        public DateTime? Updated { get; set; }
        public bool? Active { get; set; }
        public bool? Locked { get; set; }
        public bool? Deleted { get; set; }
    }
}
