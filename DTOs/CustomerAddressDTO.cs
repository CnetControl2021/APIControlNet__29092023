using APIControlNet.Models;

namespace APIControlNet.DTOs
{
    public class CustomerAddressDTO
    {
        public int CustomerAddressIdx { get; set; }
        public Guid CustomerId { get; set; } =Guid.NewGuid();
        public int? CustomerAddressIdi { get; set; }
        public string CountryId { get; set; }
        public Guid? CityId { get; set; }
        public string Street { get; set; }
        public string OutsideNumber { get; set; }
        public string InsideNumber { get; set; }
        public string Location { get; set; }
        public string Colony { get; set; }
        public string ZipCode { get; set; }
        public string StateId { get; set; }
        public string Municipality { get; set; }
        public DateTime? Date { get; set; } = DateTime.Now;
        public DateTime? Updated { get; set; } = DateTime.Now;
        public bool? Active { get; set; } = true;
        public bool? Locked { get; set; } = false;
        public bool? Deleted { get; set; } = false;

        public virtual CustomerDTO Customer { get; set; }
    }
}
