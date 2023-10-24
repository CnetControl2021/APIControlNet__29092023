namespace APIControlNet.DTOs
{
    public class Invoice_InvInDoc
    {
        public InvoiceDTO NInvoiceDTO { get; set; }
        public InventoryInDocumentDTO NInventoryInDocumentDTO { get; set; }

        public Invoice_InvInDoc()
        {
            NInvoiceDTO = new InvoiceDTO();
            NInventoryInDocumentDTO = new InventoryInDocumentDTO();
        }
    }
}
