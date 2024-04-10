using System;
using System.Collections.Generic;

namespace APIControlNet.Models
{
    public partial class CentralizeType
    {
        public int CentralizeTypeIdx { get; set; }
        public int CentralizeTypeId { get; set; }
        public string Description { get; set; }
        public Guid NetgroupId { get; set; }
        public DateTime? Date { get; set; }
        public DateTime? Updated { get; set; }
        public bool? Active { get; set; }
        public bool? Locked { get; set; }
        public bool? Deleted { get; set; }
        public int ItIsToGetInformation { get; set; }
    }
}
