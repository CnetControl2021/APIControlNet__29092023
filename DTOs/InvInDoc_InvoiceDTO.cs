using APIControlNet.Models;

namespace APIControlNet.DTOs
{
    public class InvInDoc_InvoiceDTO
    {
        public InventoryInDocumentDTO inventoryInDocumentDTO { get; set; }
        public InvoiceDTO invoiceDTO { get; set; }

        public InvInDoc_InvoiceDTO()
        {
            inventoryInDocumentDTO = new InventoryInDocumentDTO();
            invoiceDTO = new InvoiceDTO();
        }
    }
}
