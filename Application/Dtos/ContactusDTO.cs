using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
namespace Application.Dtos
{
    public class ContactusDTO
    {
        [DisplayName("First Name")]
        [Required(ErrorMessage = "{0} is required")]
        public string FirstName { get; set; }

        [DisplayName("Last Name")]
        [Required(ErrorMessage = "{0} is required")]
        public string LastName { get; set; }

        [DisplayName("Email")]
        [Required(ErrorMessage = "{0} is required")]
        [StringLength(64, ErrorMessage = "{1} characters max")]
        [RegularExpression("^[a-z0-9_\\+-]+(\\.[a-z0-9_\\+-]+)*@[a-z0-9-]+(\\.[a-z0-9]+)*\\.([a-z]{2,4})$", ErrorMessage = "Enter a valid email id")]
        public string EmailId { get; set; }

        [DisplayName("Phone Number")]
        [Required(ErrorMessage = "{0} is required")]
        [StringLength(10, ErrorMessage = "{1} characters max")]
        [RegularExpression(@"^\d{10}$", ErrorMessage = "Enter a valid 10 digit number")]
        public string PhoneNo { get; set; }

        [Required(ErrorMessage = "{0} is required")]
        public string Subject { get; set; }

        [Required(ErrorMessage = "{0} is required")]
        public string Message { get; set; }

        public string info { get; set; }
    }
}
