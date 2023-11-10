namespace APIControlNet.DTOs
{
    public class JsonSubclaveProductoDTO
    {
        public int JsonSubclaveProductoIdx { get; set; }
        public string JsonSubclaveProductoId { get; set; }
        public string Descripcion { get; set; }
        public DateTime? Date { get; set; }
        public DateTime? Updated { get; set; }
        public bool? Active { get; set; }
        public bool? Locked { get; set; }
        public bool? Deleted { get; set; }
    }
}
