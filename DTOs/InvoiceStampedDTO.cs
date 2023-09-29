using APIControlNet.Models;

namespace APIControlNet.DTOs
{
    public class InvoiceStampedDTO
    {
        public int InvoiceStampedIdx { get; set; }
        public Guid? InvoiceId { get; set; }
        public string XmlStamped { get; set; }
        public DateTime? DateStamped { get; set; }
        public Guid? Uuid { get; set; }
        public DateTime? Updated { get; set; }
        public DateTime? Date { get; set; }
        public byte? Active { get; set; }
        public byte? Locked { get; set; }
        public byte? Deleted { get; set; }

        public virtual InvoiceDTO Invoice { get; set; }
    }
}
