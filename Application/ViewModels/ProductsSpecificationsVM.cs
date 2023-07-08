namespace Application.ViewModels
{
    public class ProductsSpecificationsVM //: BaseVM
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public int SpecfId { get; set; }
        //public int SpecfSSCatId { get; set; }
        public string SpecfSSCatField { get; set; }

        public ProductsVM Product { get; set; }
    }
}
