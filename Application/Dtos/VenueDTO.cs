using System.ComponentModel.DataAnnotations;
namespace Application.Dtos
{
    public class VenueDTO : BaseDTO
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "{0} is required")]
        [StringLength(32, ErrorMessage = "{1} characters max")]
        public string Name { get; set; }
    }
}
