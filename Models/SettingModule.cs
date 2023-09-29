using System;
using System.Collections.Generic;

namespace APIControlNet.Models
{
    public partial class SettingModule
    {
        public SettingModule()
        {
            Settings = new HashSet<Setting>();
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

        public virtual ICollection<Setting> Settings { get; set; }
    }
}
