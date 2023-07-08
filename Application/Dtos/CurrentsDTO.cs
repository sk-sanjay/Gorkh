using System.ComponentModel.DataAnnotations;
namespace Application.Dtos
{
    public class CurrentsDTO : BaseDTO
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Current name is required")]
        [StringLength(50, ErrorMessage = "{1} characters max")]
        public string CurrentName { get; set; }
    }
}
