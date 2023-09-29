using System;
using System.Collections.Generic;

namespace APIControlNet.Models
{
    public partial class Card
    {
        public int CardIdx { get; set; }
        public string CardId { get; set; }
        public int CardTypeId { get; set; }
        public string Name { get; set; }
        public Guid? IdentifierId { get; set; }
        public DateTime? Date { get; set; }
        public DateTime? Updated { get; set; }
        public bool? EnableAuthorize { get; set; }
        public bool? Active { get; set; }
        public bool? Locked { get; set; }
        public bool? Deleted { get; set; }
        public bool? AskVehicle { get; set; }

        public virtual CardType CardType { get; set; }
    }
}
