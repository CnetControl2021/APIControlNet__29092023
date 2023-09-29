using APIControlNet.Models;

namespace APIControlNet.DTOs
{
    public class CardDTO
    {
        public int CardIdx { get; set; }
        public string CardId { get; set; }
        public int CardTypeId { get; set; }
        public string Name { get; set; }
        public Guid? IdentifierId { get; set; }
        public DateTime? Date { get; set; }=DateTime.Now;
        public DateTime? Updated { get; set; }= DateTime.Now;
        public bool? EnableAuthorize { get; set; }=true;
        public bool? Active { get; set; }=true ;
        public bool? Locked { get; set; }=false ;
        public bool? Deleted { get; set; }=false;
        public bool? AskVehicle { get; set; }

        public virtual CardTypeDTO CardType { get; set; }
    }
}
