using System;
using System.Collections.Generic;

namespace APIControlNet.Models
{
    public partial class Product
    {
        public Product()
        {
            DailySummaries = new HashSet<DailySummary>();
            MonthlySummaries = new HashSet<MonthlySummary>();
            ProductSats = new HashSet<ProductSat>();
            ProductStores = new HashSet<ProductStore>();
        }

        public int ProductIdx { get; set; }
        public Guid ProductId { get; set; }
        public string ProductCode { get; set; }
        public string SatClaveProductoServicioId { get; set; }
        public string SatClaveUnidadId { get; set; }
        public bool? IsFuel { get; set; }
        public string Name { get; set; }
        public Guid ProductCategoryId { get; set; }
        public string Barcode { get; set; }
        public DateTime? Date { get; set; }
        public DateTime? Updated { get; set; }
        public bool? Active { get; set; }
        public bool? Locked { get; set; }
        public bool? Deleted { get; set; }
        public string AppName { get; set; }
        public string Color { get; set; }
        public string JsonClaveUnidadMedidaId { get; set; }

        public virtual ICollection<DailySummary> DailySummaries { get; set; }
        public virtual ICollection<MonthlySummary> MonthlySummaries { get; set; }
        public virtual ICollection<ProductSat> ProductSats { get; set; }
        public virtual ICollection<ProductStore> ProductStores { get; set; }
    }
}
