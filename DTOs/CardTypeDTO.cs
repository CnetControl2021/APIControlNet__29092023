using APIControlNet.Models;

namespace APIControlNet.DTOs
{
    public class CardTypeDTO
    {
        public CardTypeDTO()
        {
            Cards = new HashSet<CardDTO>();
        }

        public int CardTypeIdx { get; set; }
        public int? CardTypeId { get; set; }
        public string Name { get; set; }
        public DateTime? Date { get; set; }
        public DateTime? Upadte { get; set; }
        public bool? Active { get; set; }
        public bool? Locked { get; set; }
        public bool? Deleted { get; set; }

        public virtual ICollection<CardDTO> Cards { get; set; }
    }
}
