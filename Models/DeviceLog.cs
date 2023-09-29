using System;
using System.Collections.Generic;

namespace APIControlNet.Models
{
    public partial class DeviceLog
    {
        public int DeviceLogIdx { get; set; }
        public Guid DeviceLogId { get; set; }
        public Guid? StoreId { get; set; }
        public string DeviceId { get; set; }
        public string Name { get; set; }
        public string Message { get; set; }
        public decimal? Percentage { get; set; }
        public long? Date { get; set; }
        public long? Updated { get; set; }
        public byte? Active { get; set; }
        public byte? Locked { get; set; }
        public byte? Deleted { get; set; }
    }
}
