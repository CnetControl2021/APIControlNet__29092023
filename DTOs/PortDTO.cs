using APIControlNet.Models;
using System.ComponentModel.DataAnnotations;

namespace APIControlNet.DTOs
{
    public class PortDTO
    {
        public PortDTO()
        {
            LoadPositions = new HashSet<LoadPositionDTO>();
            Tanks = new HashSet<TankDTO>();
        }

        public int PortIdx { get; set; }
        public Guid StoreId { get; set; }
        public int PortIdi { get; set; }
        public string Name { get; set; }
        public string NameLinux { get; set; }
        public int PortTypeIdi { get; set; }
        public int? BaudRate { get; set; }
        public DateTime? Date { get; set; } = DateTime.Now;
        public int? IsShowTxrx { get; set; }
        public bool? IsWithEcho { get; set; }
        public DateTime? Updated { get; set; }=DateTime.Now;
        public string Description { get; set; }
        public bool? Active { get; set; } = true;
        public bool? Locked { get; set; }=false;
        public bool? Deleted { get; set; }=false ;

        public virtual PortTypeDTO PortTypeIdiNavigation { get; set; }
        public virtual StoreDTO Store { get; set; }
        public virtual ICollection<LoadPositionDTO> LoadPositions { get; set; }
        public virtual ICollection<TankDTO> Tanks { get; set; }
    }
}
