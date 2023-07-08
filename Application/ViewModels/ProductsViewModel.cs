namespace Application.ViewModels
{
    public class ProductsViewModel
    {
        public int Id { get; set; }
        public string ProductNumber { get; set; }
        public int SubSubCatId { get; set; }
        public int SubCategoryId { get; set; }
        public int QuantityAvl { get; set; }
        public decimal ReservePrice { get; set; }
        public string SubSubCategoriesName { get; set; }
        public string ProductImage { get; set; }
        public string StateName { get; set; }
    }
}
