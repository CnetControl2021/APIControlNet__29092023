using APIControlNet.Models;
using System.ComponentModel.DataAnnotations;

namespace APIControlNet.DTOs
{
    public class HoseDTO
    {
        public int HoseIdx { get; set; }
        public Guid StoreId { get; set; }
        public int? HoseIdi { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int LoadPositionIdi { get; set; }
        public Guid? ProductId { get; set; }
        public int? CpuAddressHose { get; set; }
        public int? Position { get; set; }
        public decimal? SlowFlow { get; set; }
        public DateTime? Date { get; set; } = DateTime.Now;
        public DateTime? Updated { get; set; } = DateTime.Now;
        public bool? Active { get; set; } = true;
        public bool? Locked { get; set; } = false;
        public bool? Deleted { get; set; } = false;

        public virtual LoadPositionDTO LoadPosition { get; set; }
        public virtual ProductStoreDTO ProductStore { get; set; }
        public virtual StoreDTO Store { get; set; }

    }
}
