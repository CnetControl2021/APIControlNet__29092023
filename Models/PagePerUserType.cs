using System;
using System.Collections.Generic;

namespace APIControlNet.Models
{
    public partial class PagePerUserType
    {
        public int PagePerUserTypeIdx { get; set; }
        public Guid? PagePerUserTypeId { get; set; }
        public Guid? UserTypeId { get; set; }
        public long? Date { get; set; }
        public long? Updated { get; set; }
        public byte? Active { get; set; }
        public byte? Locked { get; set; }
        public byte? Deleted { get; set; }
        public string Name { get; set; }
    }
}
