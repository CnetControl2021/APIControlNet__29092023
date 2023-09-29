using System;
using System.Collections.Generic;

namespace APIControlNet.Models
{
    public partial class ConfigView
    {
        public int ConfigViewIdx { get; set; }
        public Guid ConfigViewId { get; set; }
        public int Type { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string QueryData { get; set; }
        public string QueryDataCondition { get; set; }
        public string QueryResume { get; set; }
        public string TableSave { get; set; }
        public string TableName { get; set; }
        public string TableKey { get; set; }
        public string Fields { get; set; }
        public string FieldsSave { get; set; }
        public int? FrontendFrameType { get; set; }
        public string FrontendName { get; set; }
        public byte? FrontendEnableDeleted { get; set; }
        public byte? FrontendEnableBlock { get; set; }
        public byte? FrontendEnableActive { get; set; }
        public byte? FrontendEnableSave { get; set; }
        public byte? FrontendEnableEdit { get; set; }
        public byte? FrontendEnableNext { get; set; }
        public string FieldPairKey { get; set; }
        public string FieldPairValue { get; set; }
        public string FieldKeyOne { get; set; }
        public long? Date { get; set; }
        public long? Updated { get; set; }
        public byte Active { get; set; }
        public byte? Locked { get; set; }
        public byte? Deleted { get; set; }
    }
}
