namespace APIControlNet.DTOs
{
    public class SoSsubDTO
    {
        public SaleOrderDTO NSaleOrderDTO { get; set; }
        public SaleSuborderDTO NSaleSuborderDTO { get; set; }
        //public InventoryInSaleOrderDTO inventoryInSaleOrderDTO { get; set; }
        public SoSsubDTO()
        {
            NSaleOrderDTO = new SaleOrderDTO();
            NSaleSuborderDTO = new SaleSuborderDTO();
            //inventoryInSaleOrderDTO = new InventoryInSaleOrderDTO();
        }

    }
}
