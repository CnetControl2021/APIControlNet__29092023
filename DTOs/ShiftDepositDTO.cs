using APIControlNet.Models;

namespace APIControlNet.DTOs
{
    public partial class ShiftDepositDTO
    {
        public Guid? StoreId { get; set; }
        public int? IslandIdi { get; set; }
        public int? ShiftDepositNumber { get; set; }
        public Guid? ShiftHeadId { get; set; }
        public Guid? EmployeeId { get; set; }
        public decimal? Amount { get; set; }
        

    }
}
