using System;
using System.Collections.Generic;

namespace APIControlNet.Models
{
    public partial class FormaPagoCnetcore
    {
        public int FormaPagoCnetcoreIdx { get; set; }
        public int? FormaPagoCnetcoreId { get; set; }
        public int? IsOwnCard { get; set; }
        public int? IsVoucher { get; set; }
        public string Name { get; set; }
        public DateTime? Date { get; set; }
        public DateTime? Updated { get; set; }
        public bool? Active { get; set; }
        public bool? Locked { get; set; }
        public bool? Deleted { get; set; }
    }
}
