namespace APIControlNet.DTOs
{
    public class InvInDoc_Invoice
    {
        public InventoryInDocumentDTO NInventoryInDocumentDTO { get; set; } =new InventoryInDocumentDTO();
        public InvoiceDTO NInvoiceDTO { get; set; } =new InvoiceDTO();

        public InvInDoc_Invoice()
        {
            NInventoryInDocumentDTO = new InventoryInDocumentDTO();
            NInvoiceDTO = new InvoiceDTO();
        }

    }
}
