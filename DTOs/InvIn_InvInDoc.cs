namespace APIControlNet.DTOs
{
    public class InvIn_InvInDoc
    {
        public InventoryInDTO inventoryInDTO { get; set; }
        public InventoryInDocumentDTO inventoryInDocumentDTO { get; set; }

        public InvIn_InvInDoc()
        {
            inventoryInDTO = new InventoryInDTO();
            inventoryInDocumentDTO = new InventoryInDocumentDTO();
        }

    }
}
