using System.ComponentModel.DataAnnotations;

namespace Application.Dtos
{
    public class BannersDTO : BaseDTO
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Title Name is required")]
        [StringLength(100, ErrorMessage = "{1} characters max")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Descriptions is required")]
        public string Descriptions { get; set; }

        //[Required(ErrorMessage = "Please select image")]
        public string BannerImage { get; set; }
    }
}
