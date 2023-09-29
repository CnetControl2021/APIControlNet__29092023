using APIControlNet.Models;

namespace APIControlNet.DTOs
{
    public class EmployeeDTO
    {
        public int EmployeeIdx { get; set; }
        public Guid? EmployeeId { get; set; } = Guid.NewGuid();
        public int? EmployeeNumber { get; set; }
        public string UserName { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Cellular { get; set; }
        public string Password { get; set; }
        public int? PasswordNumber { get; set; }
        public string Secret { get; set; }
        public byte? IsSupervisor { get; set; }
        public bool? EnableDispensary { get; set; }
        public bool? EnableOpenIsland { get; set; }
        public bool? EnableTank { get; set; }
        public bool? EnableMakeShift { get; set; }
        public bool? EnablePrintShift { get; set; }
        public bool? EnablePhotoOnEnding { get; set; }
        public bool? EnableCardPayment { get; set; }
        public bool? EnableOwnVouchers { get; set; }
        public Guid? StoreId { get; set; }
        public DateTime? Date { get; set; } = DateTime.Now;
        public DateTime? Updated { get; set; } = DateTime.Now;
        public bool? Active { get; set; } = true;
        public bool? Locked { get; set; }=false;
        public bool? Deleted { get; set; }=false ;

        public virtual StoreDTO Store { get; set; }
    }
}
