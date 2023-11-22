namespace APIControlNet.DTOs
{
    public class InvInDoc_Invoice
    {
        public InventoryInDTO NInventoryInDTO { get; set; } = new InventoryInDTO();
        public InventoryInDocumentDTO NInventoryInDocumentDTO { get; set; } =new InventoryInDocumentDTO();
        public InvoiceDTO NInvoiceDTO { get; set; } =new InvoiceDTO();
        public InvoiceDetailDTO NInvoiceDetailDTO { get; set; } = new InvoiceDetailDTO();

        public InvInDoc_Invoice()
        {
            NInventoryInDTO = new InventoryInDTO();
            NInventoryInDocumentDTO = new InventoryInDocumentDTO();
            NInvoiceDTO = new InvoiceDTO();
            NInvoiceDetailDTO = new InvoiceDetailDTO();
        }

    }
}
