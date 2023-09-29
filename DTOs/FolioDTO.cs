using System.ComponentModel.DataAnnotations;

namespace APIControlNet.DTOs
{
    public class FolioDTO
    {
        public int FolioIdx { get; set; }
        //[Required(ErrorMessage = "El campo {0} es requerio")]
        public Guid? StoreId { get; set; }
        [Required(ErrorMessage = "El campo {0} es requerio")]
        public int? Folio1 { get; set; }
        [Required(ErrorMessage = "El campo {0} es requerio")]
        public string Type { get; set; }
        public DateTime? Date { get; set; } = DateTime.Now;
        public DateTime? Updated { get; set; } = DateTime.Now;
        public byte? Active { get; set; } = 1;
        public byte? Locked { get; set; }
        public byte? Deleted { get; set; }
        public string Name { get; set; }
    }
}
