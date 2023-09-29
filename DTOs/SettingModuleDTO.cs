using APIControlNet.Models;

namespace APIControlNet.DTOs
{
    public class SettingModuleDTO
    {
        public SettingModuleDTO()
        {
            Settings = new HashSet<SettingDTO>();
        }

        public int SettingModuleIdx { get; set; }
        public int SettingModuleIdi { get; set; }
        public string Description { get; set; }
        public DateTime? Date { get; set; }
        public DateTime? Updated { get; set; }
        public byte? Active { get; set; }
        public byte? Locked { get; set; }
        public byte? Deleted { get; set; }
        public string Name { get; set; }

        public virtual ICollection<SettingDTO> Settings { get; set; }

    }
}
