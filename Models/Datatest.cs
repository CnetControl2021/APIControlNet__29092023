using System;
using System.Collections.Generic;

namespace APIControlNet.Models
{
    public partial class Datatest
    {
        public int Idx { get; set; }
        public Guid? DatatestId { get; set; }
        public int? DatatestNumber { get; set; }
        public string Name { get; set; }
        public string Descripcion { get; set; }
        public DateTime? Date { get; set; }
        public DateTime? Updated { get; set; }
        public bool? Active { get; set; }
        public bool? Locked { get; set; }
        public bool? Deleted { get; set; }
    }
}
