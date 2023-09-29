using System;
using System.Collections.Generic;

namespace APIControlNet.Models
{
    public partial class ProductStore
    {
        public ProductStore()
        {
            Hoses = new HashSet<Hose>();
        }

        public int ProductStoreIdx { get; set; }
        public Guid StoreId { get; set; }
        public Guid ProductId { get; set; }
        public string ProductPemex { get; set; }
        public string ProductUce { get; set; }
        public int? SystemGradeId { get; set; }
        public decimal? Cost { get; set; }
        public decimal? Existence { get; set; }
        public decimal? Tax { get; set; }
        public decimal? Ieps { get; set; }
        public decimal? Price { get; set; }
        public DateTime? Date { get; set; }
        public DateTime? Updated { get; set; }
        public bool? Active { get; set; }
        public bool? Locked { get; set; }
        public bool? Deleted { get; set; }
        public string Color { get; set; }
        public decimal? FactorTc { get; set; }
        public int? ProductDensityGramsPerCm3 { get; set; }

        public virtual Product Product { get; set; }
        public virtual Store Store { get; set; }
        public virtual ICollection<Hose> Hoses { get; set; }
    }
}
