namespace APIControlNet.DTOs
{
    public class VersionDTO
    {
        public int VersionIdx { get; set; }
        public int? SystemId { get; set; }
        public string VersionId { get; set; }
        public string RevisionId { get; set; }
        public string UserName { get; set; }
        public string UserNameCheck { get; set; }
        public string Description { get; set; }
        public string Hash512 { get; set; }
        public string Upgrade { get; set; }
        public DateTime? VersionDate { get; set; }
        public DateTime? Updated { get; set; }
        public bool? Active { get; set; }
        public bool? Locked { get; set; }
        public bool? Deleted { get; set; }

    }
}
