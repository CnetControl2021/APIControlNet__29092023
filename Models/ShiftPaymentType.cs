﻿using System;
using System.Collections.Generic;

namespace APIControlNet.Models
{
    public partial class ShiftPaymentType
    {
        public int ShiftPaymentTypeIdx { get; set; }
        public int ShiftPaymentTypeId { get; set; }
        public string Description { get; set; }
        public DateTime? Date { get; set; }
        public DateTime? Updated { get; set; }
        public bool? Active { get; set; }
        public bool? Locked { get; set; }
        public bool? Deleted { get; set; }
    }
}
