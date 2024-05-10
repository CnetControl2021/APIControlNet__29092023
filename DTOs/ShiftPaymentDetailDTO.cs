using APIControlNet.Models;

namespace APIControlNet.DTOs
{
    public partial class ShiftPaymentDetailDTO
    {
        public Guid ShiftPaymentId { get; set; }
        public int ShiftPaymentTypeIdi { get; set; }
        public int ShiftPaymentNumberReference { get; set; }
        public decimal? Amount { get; set; }
        public Guid? SaleOrderId { get; set; }
        public int? IsCaptured { get; set; }

        // =====  ADICIONALES  =====
        public string ShiftPaymentTypeDescription { set; get; }
        public int? CustomerNumber { set; get; }
        public string CustomerName { set; get; }
        public String Description { set; get; }


        public enum ePaymentType
        {
            Efectivo = 1,
            ClienteCredito = 2,
            Jarreo = 3,
            Consignación = 4,
            Vales = 5,
            Tarjetas = 6,
            Gastos = 7
        }
    }
}
