using System;
using System.Collections.Generic;

namespace APIControlNet.Models
{
    public partial class ConfigGral
    {
        public int IdConfigGral { get; set; }
        public Guid StoreId { get; set; }
        public int ModuleId { get; set; }
        public string RowName { get; set; }
        public string Description { get; set; }
        public string Value { get; set; }
        public long FechaUpd { get; set; }
        public long? Updated { get; set; }
        public byte? Active { get; set; }
        public byte? Locked { get; set; }
        public byte? Deleted { get; set; }
        public string Name { get; set; }
    }
}
