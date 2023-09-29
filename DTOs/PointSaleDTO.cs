using APIControlNet.Models;
using System.ComponentModel.DataAnnotations;

namespace APIControlNet.DTOs
{
    public class PointSaleDTO
    {
        public int PointSaleIdx { get; set; }
        [Required(ErrorMessage = "El campo {0} es requerio")]
        public Guid? StoreId { get; set; }
        [Required(ErrorMessage = "El campo {0} es requerio")]
        public int? PointSaleIdi { get; set; }
        [Required(ErrorMessage = "El campo {0} es requerio")]
        public int? PortIdi { get; set; }
        public string Name { get; set; }
        [Required(ErrorMessage = "El campo {0} es requerio")]
        public int? Type { get; set; }
        [Required(ErrorMessage = "El campo {0} es requerio")]
        public int? Subtype { get; set; }
        [Required(ErrorMessage = "El campo {0} es requerio")]
        public int? Address { get; set; }
        [Required(ErrorMessage = "El campo {0} es requerio")]
        public int? PrinterBaudRate { get; set; }
        [Required(ErrorMessage = "El campo {0} es requerio")]
        public int? PrinterType { get; set; }
        public int? PrinterIdi { get; set; }
        public int? TypeAuthorization { get; set; }
        public string PointSaleUnique { get; set; }
        public string InvoiceSerieId { get; set; }
        public int? StatusRes { get; set; }
        public int? CommPercentage { get; set; }
        public DateTime? Date { get; set; } = DateTime.Now;
        public DateTime? Updated { get; set; }=DateTime.Now;
        public bool? Active { get; set; } = true;
        public bool? Locked { get; set; } = false;
        public bool? Deleted { get; set; }= false;

        public virtual StoreDTO Store { get; set; }
    }
}
