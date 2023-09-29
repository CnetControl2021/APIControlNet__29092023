using System;
using System.Collections.Generic;

namespace APIControlNet.Models
{
    public partial class UserDatum
    {
        public int UserDataIdx { get; set; }
        public Guid? UserLoginId { get; set; }
        public Guid? UserDataId { get; set; }
        public string Name { get; set; }
        public string Lastname { get; set; }
        public string MotherLastname { get; set; }
        public long? Birthday { get; set; }
        public long? Updated { get; set; }
        public byte? Active { get; set; }
        public byte? Locked { get; set; }
        public byte? Deleted { get; set; }
    }
}
