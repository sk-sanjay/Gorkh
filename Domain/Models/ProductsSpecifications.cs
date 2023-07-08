namespace Domain.Models
{
    public class ProductsSpecifications //: BaseModel
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public int SpecfId { get; set; }
        //public int SpecfSSCatId { get; set; }
        public string SpecfSSCatField { get; set; }
        public virtual Products Product { get; set; }
    }
}
