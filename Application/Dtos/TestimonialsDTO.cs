
using System.ComponentModel.DataAnnotations;

namespace Application.Dtos
{
   public class TestimonialsDTO :BaseDTO
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Name is required")]
        [StringLength(64, ErrorMessage = "{1} characters max")]
        public string Name { get; set; }
        public string Image { get; set; }

        [Required(ErrorMessage = "Description is required")]
        public string Description { get; set; }
        public string Designtion { get; set; }
        public string CompanyName { get; set; }
        public string Location { get; set; }
    }
}
