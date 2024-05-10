using System;
using System.Collections.Generic;

namespace APIControlNet.Models
{
    public partial class CentralizeStore
    {
        public int CentralizeStoreIdx { get; set; }
        public Guid StoreId { get; set; }
        public int CentralizeStoreIdi { get; set; }
        public int CentralizeTypeId { get; set; }
        public Guid NetgroupId { get; set; }
        public string Url { get; set; }
        public string Description { get; set; }
        public string UserId { get; set; }
        public string Password { get; set; }
        public int TopSend { get; set; }
        public int TopRead { get; set; }
        public int TopSendLow { get; set; }
        public int TopReadLow { get; set; }
        public bool? Send { get; set; }
        public bool? Receive { get; set; }
        public int NotUpdatedFieldGeographic { get; set; }
        public DateTime? Date { get; set; }
        public DateTime? Updated { get; set; }
        public bool? Active { get; set; }
        public bool? Locked { get; set; }
        public bool? Deleted { get; set; }
        public bool? IsFuelnet { get; set; }
    }
}
