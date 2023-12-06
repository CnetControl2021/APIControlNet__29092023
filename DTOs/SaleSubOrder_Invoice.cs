namespace APIControlNet.DTOs
{
    public class SaleSubOrder_Invoice
    {
        public SaleOrderDTO NSaleOrderDTO { get; set; }
        public SaleSuborderDTO NSaleSuborderDTO { get; set; } 
        public InvoiceDTO NInvoiceDTO { get; set; } 

        public SaleSubOrder_Invoice()
        {
            NSaleSuborderDTO = new SaleSuborderDTO();
            NInvoiceDTO = new InvoiceDTO();
            NSaleOrderDTO = NSaleOrderDTO;
        }
    }
}
