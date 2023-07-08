namespace Domain.Models
{
    public class ProductsFileUploads
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public string ProductImage { get; set; }
        public bool IsImageDefault { get; set; }
        public int FlagImage { get; set; }
    }
}
