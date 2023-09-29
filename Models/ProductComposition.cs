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
        public int? Active { get; set; }
        public int? Locked { get; set; }
        public int? Deleted { get; set; }
    }
}
