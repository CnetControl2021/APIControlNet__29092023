﻿using System;
using System.Collections.Generic;

namespace APIControlNet.Models
{
    public partial class System
    {
        public int SystemIdx { get; set; }
        public int SystemId { get; set; }
        public string Description { get; set; }
        public DateTime? Date { get; set; }
        public DateTime? Updated { get; set; }
        public bool? Active { get; set; }
        public bool? Locked { get; set; }
        public bool? Deleted { get; set; }
    }
}
