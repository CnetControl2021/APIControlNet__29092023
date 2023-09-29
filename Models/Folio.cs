using System;
using System.Collections.Generic;

namespace APIControlNet.Models
{
    public partial class Folio
    {
        public int FolioIdx { get; set; }
        public Guid? StoreId { get; set; }
        public int? Folio1 { get; set; }
        public string Type { get; set; }
        public DateTime? Date { get; set; }
        public DateTime? Updated { get; set; }
        public bool? Active { get; set; }
        public bool? Locked { get; set; }
        public bool? Deleted { get; set; }
        public string Name { get; set; }

        public virtual Store Store { get; set; }
    }
}
