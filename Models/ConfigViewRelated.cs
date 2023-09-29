using System;
using System.Collections.Generic;

namespace APIControlNet.Models
{
    public partial class ConfigViewRelated
    {
        public int ConfigViewRelatedId { get; set; }
        public Guid ConfigViewId { get; set; }
        public Guid ConfigViewIdRelated { get; set; }
        public string IdFieldNameRelation { get; set; }
        public int? ReturnOrder { get; set; }
        public byte? IsChild { get; set; }
        public byte? SaveOrder { get; set; }
        public string Name { get; set; }
        public long? Date { get; set; }
        public long? Updated { get; set; }
        public byte? Active { get; set; }
        public byte? Locked { get; set; }
        public byte? Deleted { get; set; }
    }
}
