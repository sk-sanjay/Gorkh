namespace Application.Dtos
{
    public class ProductsSpecificationsDTO //: BaseDTO
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public int SpecfId { get; set; }
        //public int SpecfSSCatId { get; set; }
        public string SpecfSSCatField { get; set; }

        public ProductsDTO Product { get; set; }
    }
}
