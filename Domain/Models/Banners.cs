namespace Domain.Models
{
    public class Banners : BaseModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Descriptions { get; set; }
        public string BannerImage { get; set; }
    }
}
