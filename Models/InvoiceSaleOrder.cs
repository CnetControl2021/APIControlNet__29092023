﻿using System;
using System.Collections.Generic;

namespace APIControlNet.Models
{
    public partial class InvoiceSaleOrder
    {
        public int InvoiceSaleOrderIdx { get; set; }
        public Guid InvoiceId { get; set; }
        public Guid SaleOrderId { get; set; }
        public decimal? Amount { get; set; }
        public DateTime? Date { get; set; }
        public DateTime? Updated { get; set; }
        public byte? Active { get; set; }
        public byte? Locked { get; set; }
        public byte? Deleted { get; set; }
    }
}
