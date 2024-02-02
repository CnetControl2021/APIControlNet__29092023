using System;
using System.Collections.Generic;

namespace APIControlNet.Models
{
    public partial class NetgroupRaffleTicket
    {
        public int NetgroupRaffleTicketIdx { get; set; }
        public Guid NetgroupRaffleId { get; set; }
        public Guid UserId { get; set; }
        public string IdtranCode { get; set; }
        public int? TicketNumber { get; set; }
        public decimal? TicketAmount { get; set; }
        public DateTime? Date { get; set; }
        public DateTime? Updated { get; set; }
        public int? Active { get; set; }
        public int? Locked { get; set; }
        public int? Deleted { get; set; }
        public Guid? StoreId { get; set; }
    }
}
