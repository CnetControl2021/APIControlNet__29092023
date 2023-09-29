using System;
using System.Collections.Generic;

namespace APIControlNet.Models
{
    public partial class ChPageRol
    {
        public int Id { get; set; }
        public int IdPage { get; set; }
        public string IdRol { get; set; }
        public DateTime? Date { get; set; }
        public DateTime? Updated { get; set; }
        public byte? Active { get; set; }
        public byte? Loked { get; set; }
        public byte? Deleted { get; set; }

        public virtual ChPage IdPageNavigation { get; set; }
        public virtual AspNetRole IdRolNavigation { get; set; }
    }
}
