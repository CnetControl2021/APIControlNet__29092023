namespace APIControlNet.DTOs
{
    public class ApiRespSupplierFuelDTO
    {
        public int NTotal { get; set; }
        public IEnumerable<SupplierFuelDTO> NSupplierFuelDTOs { get; set; }
    }
}
