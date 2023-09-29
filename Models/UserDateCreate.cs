using System;
using System.Collections.Generic;

namespace APIControlNet.Models
{
    public partial class UserDateCreate
    {
        public string UserName { get; set; }
        public DateTime? Date { get; set; }
        public DateTime? Update { get; set; }
    }
}
