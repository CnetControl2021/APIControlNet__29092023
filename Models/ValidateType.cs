using System;
using System.Collections.Generic;

namespace APIControlNet.Models
{
    public partial class ValidateType
    {
        public int ValidateTypeIdx { get; set; }
        public int ValidateTypeId { get; set; }
        public string Name { get; set; }
        public DateTime? Date { get; set; }
        public DateTime? Updated { get; set; }
        public bool? Active { get; set; }
        public bool? Locked { get; set; }
        public bool? Deleted { get; set; }
    }
}
