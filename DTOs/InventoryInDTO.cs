using APIControlNet.Models;
using APIControlNet.Utilidades;
using System.ComponentModel.DataAnnotations;

namespace APIControlNet.DTOs
{
    public class InventoryInDTO
    {
        public InventoryInDTO()
        {
            InventoryInDocuments = new HashSet<InventoryInDocumentDTO>();
        }

        public int InventoryInIdx { get; set; }
        public Guid InventoryInId { get; set; } //= Guid.NewGuid();
        public Guid StoreId { get; set; }
        public int? InventoryInNumber { get; set; }
        public DateTime? Date { get; set; } = DateTime.Now;
        public int TankIdi { get; set; }
        public string TankName { get; set; }
        public Guid? ProductId { get; set; }
        public string ProductName { get; set; } // agrege para nombre
        public decimal? Price { get; set; } // agrege para nombre
        public Guid? ShiftHeadId { get; set; }
        public decimal? StartVolume { get; set; }
        public decimal? StartVolumeTc { get; set; }
        public decimal? StartVolumeWater { get; set; }
        public decimal? StartTemperature { get; set; }
        public decimal? StartHeight { get; set; }
        public DateTime? StartDate { get; set; }
        public decimal? EndVolume { get; set; }
        public decimal? EndVolumeTc { get; set; }
        public decimal? EndVolumeWater { get; set; }
        public decimal? EndTemperature { get; set; }
        public decimal? EndHeight { get; set; }
        public DateTime? EndDate { get; set; }
        public decimal? Volume { get; set; }
        public decimal? VolumeTc { get; set; }
        public decimal? VolumeWater { get; set; }
        public int? StatusRx { get; set; }
        public DateTime? Updated { get; set; } = DateTime.Now;
        public bool? Active { get; set; } = true;
        public bool? Locked { get; set; } = false;
        public bool? Deleted { get; set; } = false;
        public string Name { get; set; }
        public string JsonTipoDistribucionId { get; set; }
        public decimal? AbsolutePressure { get; set; }
        public decimal? CalorificPower { get; set; }
        public int? ProductCompositionId { get; set; }
        public int? ImportPermissionId { get; set; }
        public int? TransportPermissionId { get; set; }
        public Guid SupplierId { get; set; }
        public decimal AmountPerFee { get; set; } //agregue
        public decimal AmountPerCapacity { get; set; }
        public decimal AmountPerUse { get; set; }
        public decimal AmountPerVolume { get; set; }
        public decimal AmountPerService { get; set; }
        public decimal AmountOfDiscount { get; set; }
        public Guid? SupplierTransportRegisterId { get; set; } = Guid.Empty;
        public string SatTipoComprobanteId { get; set; }
        public string InvoiceSerieId { get; set; }
        public string Folio { get; set; }
        public DateTime? Date2 { get; set; } = DateTime.Now;
        public decimal? Amount { get; set; }
        //[Required]
        //[ValidateGuid]
        public Guid Uuid { get; set; }
        public Guid? CustomerId { get; set; }

        //public virtual StoreDTO Store { get; set; }
        public virtual ICollection<InventoryInDocumentDTO> InventoryInDocuments { get; set; }
    }
}
