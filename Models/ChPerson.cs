using System;
using System.Collections.Generic;

namespace APIControlNet.Models
{
    public partial class ChPerson
    {
        public int Id { get; set; }
        public string IdUserEntity { get; set; }
        public int IdRol { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }
        public DateTime? Date { get; set; }
        public DateTime? Updated { get; set; }
        public byte? Active { get; set; }
        public byte? Loked { get; set; }
        public byte? Deleted { get; set; }
    }
}
