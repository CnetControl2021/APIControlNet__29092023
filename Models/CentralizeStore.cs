using System;
using System.Collections.Generic;

namespace APIControlNet.Models
{
    public partial class CentralizeStore
    {
        public int CentralizeStoreIdx { get; set; }
        public Guid StoreId { get; set; }
        public int CentralizeTypeId { get; set; }
        public string Description { get; set; }
        public string Url { get; set; }
        public int IsEnableToSend { get; set; }
        public DateTime? Date { get; set; }
        public DateTime? Updated { get; set; }
        public bool? Active { get; set; }
        public bool? Locked { get; set; }
        public bool? Deleted { get; set; }
        public string UserId { get; set; }
        public string Password { get; set; }
    }
}
