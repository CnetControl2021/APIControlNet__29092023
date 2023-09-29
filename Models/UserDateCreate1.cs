using System;
using System.Collections.Generic;

namespace APIControlNet.Models
{
    public partial class UserDateCreate1
    {
        public int UserdateCreate { get; set; }
        public string UserName { get; set; }
        public DateTime? Date { get; set; }
        public DateTime? Update { get; set; }
    }
}
