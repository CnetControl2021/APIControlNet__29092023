﻿using System;
using System.Collections.Generic;

namespace APIControlNet.Models
{
    public partial class TaskType
    {
        public int TaskTypeIdx { get; set; }
        public Guid? TaskTypeId { get; set; }
        public Guid? EventTypeId { get; set; }
        public string Description { get; set; }
        public long? Date { get; set; }
        public long? Updated { get; set; }
        public byte? Active { get; set; }
        public byte? Locked { get; set; }
        public byte? Deleted { get; set; }
        public string Name { get; set; }
    }
}
