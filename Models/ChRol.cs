using System;
using System.Collections.Generic;

namespace APIControlNet.Models
{
    public partial class ChRol
    {
        public int Id { get; set; }
        public string NameRol { get; set; }
        public string Description { get; set; }
        public DateTime? Date { get; set; }
        public DateTime? Updated { get; set; }
        public byte? Active { get; set; }
        public byte? Loked { get; set; }
        public byte? Deleted { get; set; }
    }
}
