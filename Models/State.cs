using System;
using System.Collections.Generic;

namespace APIControlNet.Models
{
    public partial class State
    {
        public int? StateIdx { get; set; }
        public Guid? StateId { get; set; }
        public Guid? CountryId { get; set; }
        public string Name { get; set; }
        public long? Date { get; set; }
        public long? Updated { get; set; }
        public byte? Active { get; set; }
        public byte? Locked { get; set; }
        public byte? Deleted { get; set; }
    }
}
