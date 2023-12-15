using System;
using System.Collections.Generic;

namespace APIControlNet.Models
{
    public partial class ProductComposition
    {
        public int ProductCompositionIdx { get; set; }
        public int ProductCompositionId { get; set; }
        public Guid? ProductId { get; set; }
        public string JsonTipoComposicionId { get; set; }
        public decimal? MolarFraction { get; set; }
        public decimal? CalorificPower { get; set; }
        public DateTime? Date { get; set; }
        public DateTime? Updated { get; set; }
        public bool? Active { get; set; }
        public bool? Locked { get; set; }
        public bool? Deleted { get; set; }
    }
}
