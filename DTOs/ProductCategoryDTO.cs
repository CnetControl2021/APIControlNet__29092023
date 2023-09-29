namespace APIControlNet.DTOs
{
    public class ProductCategoryDTO
    {
        //public ProductCategoryDTO()
        //{
        //    Products = new HashSet<ProductDTO>();
        //}

        public int ProductCategoryIdx { get; set; }
        public Guid ProductCategoryId { get; set; }
        public string Name { get; set; }
        public DateTime? Date { get; set; }
        public DateTime? Updated { get; set; }
        public bool? Active { get; set; } = true;
        public bool? Locked { get; set; } = false;
        public bool? Deleted { get; set; } = false ;

        //
    }
}
