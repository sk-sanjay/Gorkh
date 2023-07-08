
namespace Application.Dtos
{
    public class ProductsFileUploadsDTO
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public string ProductImage { get; set; }
        public bool IsImageDefault { get; set; }
        public int FlagImage { get; set; }

        //public List<IFormFile> ProductImage { get; set; }
    }
}
