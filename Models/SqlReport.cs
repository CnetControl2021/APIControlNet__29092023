using System;
using System.Collections.Generic;

namespace APIControlNet.Models
{
    public partial class SqlReport
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Query { get; set; }
        public Guid? StoreId { get; set; }
        public string Something { get; set; }
        public DateTime? Date { get; set; }
        public DateTime? Updated { get; set; }
        public bool? Active { get; set; }
        public bool? Locked { get; set; }
        public bool? Deleted { get; set; }
    }
}
