using APIControlNet.Models;
using APIControlNet.Utilidades;
using System.ComponentModel.DataAnnotations;

namespace APIControlNet.DTOs
{
    public class InventoryInDocumentDTO
    {
        public int InventoryInDocumentIdx { get; set; }
        public Guid StoreId { get; set; }
        public Guid InventoryInId { get; set; }
        //[Required(ErrorMessage = "El campo {0} es requerio")]
        public int? InventoryInIdi { get; set; }
        //[Required(ErrorMessage = "El campo {0} es requerio")]
        public DateTime Date { get; set; } = DateTime.Now;
        //[Required(ErrorMessage = "El campo {0} es requerio")]
        public string Type { get; set; }
        public int Folio { get; set; }
        public string Vehicle { get; set; }
        public decimal? Volume { get; set; }
        public decimal? Amount { get; set; }
        public int? SupplierFuelIdi { get; set; }
        public int? SupplierTransportIdi { get; set; }
        public string TerminalStorage { get; set; }
        public decimal? Price { get; set; }
        public decimal? SalePrice { get; set; }
        public decimal? PublicSalePrice { get; set; }
        public string SatTipoComprobanteId { get; set; }
        
        public Guid? InvoiceId { get; set; }

        [Required(ErrorMessage = "El campo GuidValue es obligatorio.")]
        //[RegularExpression(@"^[A-Fa-f0-9]{8}-[A-Fa-f0-9]{4}-[A-Fa-f0-9]{4}-[A-Fa-f0-9]{4}-[A-Fa-f0-9]{12}$", ErrorMessage = "El campo GuidValue debe ser un GUID válido.")]
        [ValidateNonZeroGuid]
        //[RegularExpression(@"^[0-9a-fA-F]{8}-[0-9a-fA-F]{4}-[0-9a-fA-F]{4}-[0-9a-fA-F]{4}-[0-9a-fA-F]{12}$")]
        public Guid? Uuid { get; set; }
        public string JsonTipoComplementoId { get; set; }
        public string JsonClaveUnidadMedidaId { get; set; }
        public string SatAclaracion { get; set; }
        public DateTime? Updated { get; set; } = DateTime.Now;
        public bool? Active { get; set; } = true; 
        public bool? Locked { get; set; } = false;
        public bool? Deleted { get; set; } = false;

        public virtual InventoryInDTO InventoryIn { get; set; }

    }
}
