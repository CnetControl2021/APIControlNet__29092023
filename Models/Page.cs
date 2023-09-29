using System;
using System.Collections.Generic;

namespace APIControlNet.Models
{
    public partial class Page
    {
        public int PageIdx { get; set; }
        public Guid? PageId { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }
        public long? Date { get; set; }
        public long? Updated { get; set; }
        public byte? Active { get; set; }
        public byte? Locked { get; set; }
        public byte? Deleted { get; set; }
    }
}
