using System;
using System.Collections.Generic;

namespace APIControlNet.Models
{
    public partial class ConfigViewItem
    {
        public int ConfigViewItemId { get; set; }
        public Guid ConfigViewId { get; set; }
        public string Name { get; set; }
        public string DataType { get; set; }
        public string FieldType { get; set; }
        public string DefaultValue { get; set; }
        public string AliasList { get; set; }
        public string AliasForm { get; set; }
        public byte? EnableShowList { get; set; }
        public byte? EnableShowForm { get; set; }
        public byte? IsRequired { get; set; }
        public byte? IsVisible { get; set; }
        public byte? IsKey { get; set; }
        public byte? IsFieldInput { get; set; }
        public string MethodListName { get; set; }
        public byte? PositionFrameNumber { get; set; }
        public int? PositionIndexList { get; set; }
        public int? PositionX { get; set; }
        public int? PositionY { get; set; }
        public int? PositionSize { get; set; }
        public byte? IsFilter { get; set; }
        public int? FilterPositionFrameNumber { get; set; }
        public int? FilterPositionX { get; set; }
        public int? FilterPositionY { get; set; }
        public int? FilterPositionSize { get; set; }
        public int? IsOrder { get; set; }
        public long? Date { get; set; }
        public long? Updated { get; set; }
        public byte? Active { get; set; }
        public byte? Locked { get; set; }
        public byte? Deleted { get; set; }
    }
}
