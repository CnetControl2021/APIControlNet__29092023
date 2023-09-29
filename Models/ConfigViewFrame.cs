using System;
using System.Collections.Generic;

namespace APIControlNet.Models
{
    public partial class ConfigViewFrame
    {
        public int ConfigViewFrameId { get; set; }
        public Guid ConfigViewId { get; set; }
        public int PositionFrameNumber { get; set; }
        public string FrameName { get; set; }
        public long? Date { get; set; }
        public long? Updated { get; set; }
        public byte? Active { get; set; }
        public byte? Locked { get; set; }
        public byte? Deleted { get; set; }
        public string Name { get; set; }
    }
}
