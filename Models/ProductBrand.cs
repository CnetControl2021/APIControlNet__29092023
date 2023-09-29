using System;
using System.Collections.Generic;

namespace APIControlNet.Models
{
    public partial class ProductBrand
    {
        public ProductBrand()
        {
            Products = new HashSet<Product>();
        }

        public int ProductBrandIdx { get; set; }
        public Guid ProductBrandId { get; set; }
        public string Name { get; set; }
        public DateTime? Date { get; set; }
        public DateTime? Updated { get; set; }
        public bool? Active { get; set; }
        public bool? Locked { get; set; }
        public bool? Deleted { get; set; }

        public virtual ICollection<Product> Products { get; set; }
    }
}
