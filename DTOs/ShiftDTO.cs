using APIControlNet.Models;

namespace APIControlNet.DTOs
{
    public partial class ShiftDTO
    {
        public Guid? StoreId { get; set; }
        public Guid? ShiftId { get; set; }
        public Guid? ShiftHeadId { get; set; }
        public int? HoseIdi { get; set; }
        public int? ShiftIdNumber { get; set; }
        public Guid? EmployeeId { get; set; }
        public decimal? StartQuantity { get; set; }
        public decimal? EndQuantity { get; set; }
        public decimal? TotalQuantity { get; set; }
        public decimal? TotalAmount { get; set; }
        public decimal? Price { get; set; }


        // =====  ADICIONALES  =====
        public Guid? ProductId { get; set; }
        public string ProductCode { get; set; }
        public string ProductName { get; set; }
        public decimal? ProductTax { set; get; }

        // ==> Combustibles
        public decimal? JarQuantity { set; get; }
        public decimal? JarAmount { set; get; }
        public decimal? ConsignmentQuantity { set; get; }
        public decimal? ConsignmentAmount { set; get; }

        // ==> Productos
        public decimal? DeliveredQuantity { set; get; }
        public decimal? ReceivedQuantity { set; get; }

    }
}
