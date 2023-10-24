using APIControlNet.Models;

namespace APIControlNet.DTOs
{
    public class Invoice_InvoiceDet_InvInDoc
    {
        public InvoiceDTO NInvoice { get; set; }
        public InvoiceDetailDTO NInvoiceDetail { get; set; }
        public InventoryInDocumentDTO NInventoryInDocumentDTO { get; set; }

        public Invoice_InvoiceDet_InvInDoc()
        {
            NInvoice = new InvoiceDTO();
            NInvoiceDetail = new InvoiceDetailDTO();
            NInventoryInDocumentDTO = new InventoryInDocumentDTO();
        }
    }
}
