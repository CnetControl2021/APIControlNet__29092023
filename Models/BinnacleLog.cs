﻿using System;
using System.Collections.Generic;

namespace APIControlNet.Models
{
    public partial class BinnacleLog
    {
        public int BinnacleLogIdx { get; set; }
        public Guid? BinnacleLogId { get; set; }
        public Guid? StoreId { get; set; }
        public int? BinnacleTypeId { get; set; }
        public string KeyValue { get; set; }
        public string UserId { get; set; }
        public string Description { get; set; }
        public string Response { get; set; }
        public DateTime? Date { get; set; }
        public DateTime? Updated { get; set; }
        public bool? Active { get; set; }
        public bool? Locked { get; set; }
        public bool? Deleted { get; set; }
        public string Name { get; set; }
    }
}
