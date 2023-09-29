using System.ComponentModel.DataAnnotations;

namespace APIControlNet.DTOs
{
    public class CustomerTypeDTO
    {
        public CustomerTypeDTO()
        {
            Customers = new HashSet<CustomerDTO>();
        }

        public int CustomerTypeIdx { get; set; }
        public int CustomerTypeId { get; set; }
        public string Name { get; set; }
        public DateTime? Date { get; set; }
        public DateTime? Updated { get; set; }
        public bool? Active { get; set; }
        public bool? Locked { get; set; }
        public bool? Deleted { get; set; }

        public virtual ICollection<CustomerDTO> Customers { get; set; }
    }
}
