using System;
using System.Collections.Generic;

namespace APIControlNet.Models
{
    public partial class Button
    {
        public int ButtonIdx { get; set; }
        public string Id { get; set; }
        public long? Date { get; set; }
        public long? Updated { get; set; }
        public byte? Active { get; set; }
        public byte? Locked { get; set; }
        public byte? Deleted { get; set; }
        public string Name { get; set; }
    }
}
