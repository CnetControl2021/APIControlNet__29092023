using System;
using System.Collections.Generic;

namespace APIControlNet.Models
{
    public partial class Volumetric
    {
        public int VolumetricIdx { get; set; }
        public Guid StoreId { get; set; }
        public string TypeOfPeriod { get; set; }
        public DateTime Date { get; set; }
        public int? IsGenarated { get; set; }
        public int? IsSent { get; set; }
        public int? EnableGenerated { get; set; }
        public int? EnableSend { get; set; }
        public string NameFile { get; set; }
        public DateTime? Updated { get; set; }
        public bool? Active { get; set; }
        public bool? Locked { get; set; }
        public bool? Deleted { get; set; }
    }
}
