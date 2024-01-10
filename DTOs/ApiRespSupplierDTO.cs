namespace APIControlNet.DTOs
{
    public class ApiRespSupplierDTO
    {
        public int NTotal { get; set; }
        public IEnumerable<SupplierDTO> NProvedores { get; set; }
    }
}
