using System.ComponentModel.DataAnnotations;

namespace APIControlNet.DTOs
{
    public class VehicleDTO
    {
        public int VehicleIdx { get; set; }
        public Guid? VehicleId { get; set; } =Guid.NewGuid();
        [Required(ErrorMessage = "El campo {0} es requerio")]
        public string Name { get; set; }
        public Guid? VehicleBrandTypeId { get; set; }
        public Guid? VehicleColorId { get; set; }
        //[Required(ErrorMessage = "El campo {0} es requerio")]
        public Guid? CustomerId { get; set; }
        [Required(ErrorMessage = "El campo {0} es requerio")]
        public string VehicleNumber { get; set; }
        public string Plate { get; set; }
        public string Series { get; set; }
        public string Detail { get; set; }
        public DateTime? Date { get; set; } = DateTime.Now;
        public DateTime? Updated { get; set; } = DateTime.Now;
        public bool? Active { get; set; } = true;
        public bool? Locked { get; set; } = false;
        public bool? Deleted { get; set; }=false;
        public bool? AskPin { get; set; }
        public bool? AskOdometer { get; set; }
        public bool? AskOdr { get; set; }
        public bool? AskOperator { get; set; }
        public bool? ValidateOdometer { get; set; }
        public bool? ValidateOdr { get; set; }
        public bool? ValidateOperator { get; set; }
        public bool? EnableDay1 { get; set; }
        public bool? EnableDay2 { get; set; }
        public bool? EnableDay3 { get; set; }
        public bool? EnableDay4 { get; set; }
        public bool? EnableDay5 { get; set; }
        public bool? EnableDay6 { get; set; }
        public bool? EnableDay7 { get; set; }
        public DateTime? StartHour1 { get; set; }
        public DateTime? StartHour2 { get; set; }
        public DateTime? StartHour3 { get; set; }
        public DateTime? EndHour1 { get; set; }
        public DateTime? EndHour2 { get; set; }
        public DateTime? EndHour3 { get; set; }
        public bool? EnableHour1 { get; set; }
        public bool? EnableHour2 { get; set; }
        public int? ValidateTypeId { get; set; }
        public int? DayBalance { get; set; }
        public int? WeekBalance { get; set; }
        public int? MonthBalance { get; set; }
        public bool? EnableDayBalance { get; set; }
        public bool? EnableWeekBalance { get; set; }
        public bool? EnableMonthBalance { get; set; }
        public bool? EnableHour3 { get; set; }
        public virtual CustomerDTO Customer { get; set; }

    }
}
