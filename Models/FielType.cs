using System;
using System.Collections.Generic;

namespace APIControlNet.Models
{
    public partial class FielType
    {
        public int FielTypeIdx { get; set; }
        public string FielTypeId { get; set; }
        public string Description { get; set; }
        public DateTime? Date { get; set; }
        public DateTime? Updated { get; set; }
        public int? Active { get; set; }
        public int? Locked { get; set; }
        public int? Deleted { get; set; }
    }
}
