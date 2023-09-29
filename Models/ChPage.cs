using System;
using System.Collections.Generic;

namespace APIControlNet.Models
{
    public partial class ChPage
    {
        public ChPage()
        {
            ChPageRols = new HashSet<ChPageRol>();
        }

        public int Id { get; set; }
        public string Mensaje { get; set; }
        public string Accion { get; set; }
        public DateTime? Date { get; set; }
        public DateTime? Updated { get; set; }
        public byte? Active { get; set; }
        public byte? Loked { get; set; }
        public byte? Deleted { get; set; }

        public virtual ICollection<ChPageRol> ChPageRols { get; set; }
    }
}
