﻿namespace APIControlNet.DTOs
{
    public class NetgroupUserTypeDTO
    {
        public int NetgroupUserTypeIdx { get; set; }
        public int NetgroupUserTypeId { get; set; }
        public string Description { get; set; }
        public DateTime? Date { get; set; }
        public DateTime? Updated { get; set; }
        public bool? Active { get; set; }
        public bool? Locked { get; set; }
        public bool? Deleted { get; set; }
    }
}
