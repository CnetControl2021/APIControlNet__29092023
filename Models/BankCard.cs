using System;
using System.Collections.Generic;

namespace APIControlNet.Models
{
    public partial class BankCard
    {
        public int BankCardIdx { get; set; }
        public Guid? BankCardId { get; set; }
        public int? BankCardNumber { get; set; }
        public string Name { get; set; }
        public DateTime? Date { get; set; }
        public DateTime? Updated { get; set; }
        public byte? Active { get; set; }
        public byte? Locked { get; set; }
        public byte? Deleted { get; set; }
    }
}
