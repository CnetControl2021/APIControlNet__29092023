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
        public int? InventoryInIdi { get; set; } = 1;
        public DateTime Date { get; set; }
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
        //[Required(ErrorMessage = "El campo GuidValue es obligatorio.")]
        //[ValidateNonZeroGuid]
        public Guid? Uuid { get; set; }
        public string JsonTipoComplementoId { get; set; }
        public string JsonClaveUnidadMedidaId { get; set; }
        public string SatAclaracion { get; set; }
        public DateTime? Updated { get; set; }
        public bool? Active { get; set; }
        public bool? Locked { get; set; }
        public bool? Deleted { get; set; }
        public Guid? SupplierTransportRegisterId { get; set; }
        public Guid? PetitionCustomsId { get; set; }

        //public virtual InventoryInDTO InventoryIn { get; set; }
        //public virtual SupplierTransportDTO S { get; set; }

    }
}
