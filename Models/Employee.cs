using System;
using System.Collections.Generic;

namespace APIControlNet.Models
{
    public partial class Employee
    {
        public int EmployeeIdx { get; set; }
        public Guid? EmployeeId { get; set; }
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
        public DateTime? Date { get; set; }
        public DateTime? Updated { get; set; }
        public bool? Active { get; set; }
        public bool? Locked { get; set; }
        public bool? Deleted { get; set; }

        public virtual Store Store { get; set; }
    }
}
