using System;
using System.Collections.Generic;

namespace APIControlNet.Models
{
    public partial class UserLogin
    {
        public int UserLoginIdx { get; set; }
        public Guid? UserLoginId { get; set; }
        public Guid? UserTypeId { get; set; }
        public string Cellular { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Secret { get; set; }
        public long? Date { get; set; }
        public long? Updated { get; set; }
        public byte? Active { get; set; }
        public byte? Locked { get; set; }
        public byte? Deleted { get; set; }
        public string Name { get; set; }
    }
}
