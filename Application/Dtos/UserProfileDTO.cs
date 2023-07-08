using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
namespace Application.Dtos
{
    public class UserProfileDTO
    {
        public string Id { get; set; }

        [Required(ErrorMessage = "{0} is required")]
        [RegularExpression("^[a-z0-9_\\+-]+(\\.[a-z0-9_\\+-]+)*@[a-z0-9-]+(\\.[a-z0-9]+)*\\.([a-z]{2,4})$", ErrorMessage = "Enter a valid email id")]
        public string Email { get; set; }

        [DisplayName("Phone Number")]
        [Required(ErrorMessage = "{0} is required")]
        [StringLength(10, ErrorMessage = "{1} characters max")]
        [RegularExpression(@"^\d{10}$", ErrorMessage = "Enter a valid 10 digit number")]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "{0} is required")]
        [StringLength(64, ErrorMessage = "{1} characters max")]
        public string Name { get; set; }

        [DisplayName("Profile Image")]
        [StringLength(64, ErrorMessage = "{1} characters max")]
        public string ProfileImage { get; set; }
    }
}
