namespace APIControlNet.DTOs.Seeding
{
    public class InvInDoc_Invoice
    {

        public InventoryInDocumentDTO inventoryInDocumentDTO { get; set; }
        public InvoiceDTO invoiceDTO { get; set; }

        public InvInDoc_Invoice()
        {
            inventoryInDocumentDTO = new InventoryInDocumentDTO();
            invoiceDTO = new InvoiceDTO();
        }
    }
}
