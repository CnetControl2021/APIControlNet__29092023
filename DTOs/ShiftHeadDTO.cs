using APIControlNet.Models;

namespace APIControlNet.DTOs
{
    public partial class ShiftHeadDTO
    {
        public int ShiftHeadIdx { get; set; }
        public Guid? ShiftHeadId { get; set; }
        public Guid? StoreId { get; set; }
        public DateTime? ShiftDate { get; set; }
        public int? ShiftNumber { get; set; }
        public int? ShiftDay { get; set; }

        public int? EmployeeIdi { get; set; }
        public String EmployeeName { get; set; }

        public virtual ICollection<ShiftDTO> Fuels { get; set; }
        public virtual ICollection<ShiftDTO> Products { get; set; }
        public virtual ICollection<ShiftPaymentDetailDTO> Clients { get; set; }
        public virtual ICollection<ShiftDepositDTO> Deposits { get; set; }
        public virtual ICollection<ShiftPaymentDetailDTO> Spents { get; set; }
        public virtual ICollection<ShiftPaymentDetailDTO> Vouchers { get; set; }
        public virtual ICollection<ShiftPaymentDetailDTO> Cards { get; set; }
    }
}