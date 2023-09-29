using System;
using System.Collections.Generic;

namespace APIControlNet.Models
{
    public partial class ButtonPerUser
    {
        public int ButtonPerUserIdx { get; set; }
        public Guid? ButtonPerUserId { get; set; }
        public Guid? UserId { get; set; }
        public Guid? PageId { get; set; }
        public Guid? ButtonId { get; set; }
        public long? Date { get; set; }
        public long? Updated { get; set; }
        public byte? Active { get; set; }
        public byte? Locked { get; set; }
        public byte? Deleted { get; set; }
        public string Name { get; set; }
    }
}
