using APIControlNet.Models;
using System.ComponentModel.DataAnnotations;

namespace APIControlNet.DTOs
{
    public class SettingDTO
    {
        public int SettingIdx { get; set; }
        public Guid SettingId { get; set; }
        public Guid StoreId { get; set; }
        public int SettingModuleIdi { get; set; }
        public string Field { get; set; }
        public string Value { get; set; }
        public string Description { get; set; }
        public string Type { get; set; }
        public DateTime? Date { get; set; }
        public DateTime? Updated { get; set; }
        public bool? Active { get; set; }
        public bool? Locked { get; set; }
        public bool? Deleted { get; set; }
        public string Response { get; set; }
        public string ResponseNodata { get; set; }
        public bool? IsSend { get; set; }
        public string Name { get; set; }
        public long? Count { get; set; }
        public decimal? AccumulatedTime { get; set; }
        public int? IsThereError { get; set; }
        public string Sql { get; set; }

        public virtual SettingModule SettingModuleIdiNavigation { get; set; }
        public virtual Store Store { get; set; }

    }
}
