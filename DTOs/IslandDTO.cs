using APIControlNet.Models;
using System.ComponentModel.DataAnnotations;

namespace APIControlNet.DTOs
{
    public class IslandDTO
    {
        //public IslandDTO()
        //{
        //    LoadPositions = new HashSet<LoadPositionDTO>();
        //}

        public int? IslandIdx { get; set; }
        public int IslandIdi { get; set; }
        public Guid StoreId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime? Date { get; set; }=DateTime.Now;
        public DateTime? Updated { get; set; } = DateTime.Now;
        public bool? Active { get; set; }=true;
        public bool? Locked { get; set; }=false;
        public bool? Deleted { get; set; }=false ;

        //public virtual StoreDTO Store { get; set; }
        //public virtual ICollection<LoadPositionDTO> LoadPositions { get; set; }
    }


}

