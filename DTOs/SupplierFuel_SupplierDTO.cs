namespace APIControlNet.DTOs
{
    public class SupplierFuel_SupplierDTO
    {
        public SupplierFuelDTO NSupplierFuelDTO { get; set; }
        public SupplierDTO NSupplierDTO { get; set; }


        public SupplierFuel_SupplierDTO()
        {
            NSupplierFuelDTO = new SupplierFuelDTO();
            NSupplierDTO = new SupplierDTO();
        }
    }
}
