using System;
using System.Collections.Generic;

namespace APIControlNet.Models
{
    public partial class CardType
    {
        public CardType()
        {
            Cards = new HashSet<Card>();
        }

        public int CardTypeIdx { get; set; }
        public int? CardTypeId { get; set; }
        public string Name { get; set; }
        public DateTime? Date { get; set; }
        public DateTime? Updated { get; set; }
        public bool? Active { get; set; }
        public bool? Locked { get; set; }
        public bool? Deleted { get; set; }

        public virtual ICollection<Card> Cards { get; set; }
    }
}
