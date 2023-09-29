namespace APIControlNet.DTOs
{
    public class SoSsubDTO
    {
        public SaleOrderDTO saleOrderDTO { get; set; }
        public SaleSuborderDTO saleSuborderDTO { get; set; }

        public SoSsubDTO()
        {
            saleOrderDTO = new SaleOrderDTO();
            saleSuborderDTO = new SaleSuborderDTO();
        }

    }
}
