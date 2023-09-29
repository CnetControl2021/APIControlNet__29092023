using System;
using System.Collections.Generic;

namespace APIControlNet.Models
{
    public partial class Net
    {
        public int NetIdx { get; set; }
        public Guid NetId { get; set; }
        public int NetNumber { get; set; }
        public string Descrition { get; set; }
        public DateTime? Date { get; set; }
        public DateTime? Updated { get; set; }
        public byte? Active { get; set; }
        public byte? Locked { get; set; }
        public byte? Deleted { get; set; }
    }
}
