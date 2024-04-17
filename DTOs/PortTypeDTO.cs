using APIControlNet.Models;

namespace APIControlNet.DTOs
{
    public class PortTypeDTO
    {
        //public PortTypeDTO()
        //{
        //    Ports = new HashSet<PortDTO>();
        //}

        public int PortTypeIdx { get; set; }
        public int PortTypeIdi { get; set; }
        public string Name { get; set; }
        //public DateTime? Date { get; set; }
        //public DateTime? Updated { get; set; }
        //public bool? Active { get; set; }
        //public bool? Locked { get; set; }
        //public bool? Deleted { get; set; }

        //public virtual ICollection<PortDTO> Ports { get; set; }
    }
}
