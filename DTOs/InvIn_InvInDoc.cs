﻿namespace APIControlNet.DTOs
{
    public class InvIn_InvInDoc
    {
        public InventoryInDTO NInventoryInDTO { get; set; }
        public InventoryInDocumentDTO NInventoryInDocumentDTO { get; set; }

        public InvIn_InvInDoc()
        {
            NInventoryInDTO = new InventoryInDTO();
            NInventoryInDocumentDTO = new InventoryInDocumentDTO();
        }

    }
}
