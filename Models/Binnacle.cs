using System;
using System.Collections.Generic;

namespace APIControlNet.Models
{
    public partial class Binnacle
    {
        public int BinnacleIdx { get; set; }
        public Guid? BinnacleId { get; set; }
        public Guid? StoreId { get; set; }
        public string Name { get; set; }
        public string UserId { get; set; }
        public int? BinnacleTypeId { get; set; }
        public string Description { get; set; }
        public string ValueName { get; set; }
        public string StartValue { get; set; }
        public string EndValue { get; set; }
        public string IpAddress { get; set; }
        public string MacAddress { get; set; }
        public string Response { get; set; }
        public DateTime? Date { get; set; }
        public DateTime? Updated { get; set; }
        public bool? Active { get; set; }
        public bool? Locked { get; set; }
        public bool? Deleted { get; set; }
    }
}
