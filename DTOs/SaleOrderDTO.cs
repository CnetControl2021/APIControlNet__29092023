using APIControlNet.Models;
using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace APIControlNet.DTOs
{
    public class SaleOrderDTO 
    {
        public SaleOrderDTO()
        {
            SaleSuborders = new HashSet<SaleSuborderDTO>();
        }

        public int SaleOrderIdx { get; set; }
        public Guid SaleOrderId { get; set; }
        public Guid StoreId { get; set; }
        public int? SaleOrderNumber { get; set; }
        public string Name { get; set; }
        public Guid? EmployeeId { get; set; }
        public int? SaleOrderNumberStart { get; set; }
        public Guid? CustomerControlId { get; set; }
        public int? HoseIdi { get; set; }
        public Guid? ShiftId { get; set; }
        public Guid? CustomerId { get; set; }
        public Guid? VehicleId { get; set; }
        public string CardEmployeeId { get; set; }
        public decimal? Amount { get; set; }
        public string Status { get; set; }
        public DateTime? StartDate { get; set; }
        public int? TicketNumber { get; set; }
        public DateTime? Date { get; set; }
        public DateTime? Updated { get; set; } =DateTime.Now;
        public bool? Active { get; set; } = true;
        public bool? Locked { get; set; } = false;
        public bool? Deleted { get; set; }=false;
        public string JsonTipoDistribucionId { get; set; }
        public Guid? SupplierTransportRegisterId { get; set; }
        public int? ProductCompositionId { get; set; }
        public Guid? PetitionCustomsId { get; set; }
        public int? TankIdi { get; set; }

        public virtual StoreDTO Store { get; set; }
        public virtual ICollection<SaleSuborderDTO> SaleSuborders { get; set; }
    }
   
}
