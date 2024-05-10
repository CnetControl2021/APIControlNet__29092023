using System;
using System.Collections.Generic;

namespace APIControlNet.Models
{
    public partial class CentralizeTable
    {
        public int CentralizeTableIdx { get; set; }
        public int CentralizeTableId { get; set; }
        public int CentralizeTypeId { get; set; }
        public int IsToReceive { get; set; }
        public string Description { get; set; }
        public string TableName { get; set; }
        public string TableKey { get; set; }
        public string Query { get; set; }
        public string FieldToInsert { get; set; }
        public string FieldToUpdate { get; set; }
        public DateTime? Date { get; set; }
        public DateTime? Updated { get; set; }
        public bool? Active { get; set; }
        public bool? Locked { get; set; }
        public bool? Deleted { get; set; }
    }
}
