using APIControlNet.Models;

namespace APIControlNet.DTOs
{
    public class StatusDispenserDTO
    {
        public StatusDispenserDTO()
        {
            LoadPositionResponses = new HashSet<LoadPositionResponseDTO>();
        }

        public int StatusDispenserIdx { get; set; }
        //public int StatusDispenserIdi { get; set; }
        public string Description { get; set; }
        ////public DateTime? Date { get; set; }
        ////public DateTime? Updated { get; set; }
        ////public bool? Active { get; set; }
        ////public bool? Locked { get; set; }
        ////public bool? Deleted { get; set; }
        public string ColorStatus { get; set; }
        //public string Logo { get; set; }

        public virtual ICollection<LoadPositionResponseDTO> LoadPositionResponses { get; set; }

    }
}
