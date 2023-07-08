using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Application.Dtos
{
    public class LoginDTO
    {
        [Required(ErrorMessage = "{0} is required")]
        [StringLength(64, ErrorMessage = "{1} characters max")]
        //[RegularExpression("^[a-z0-9_\\+-]+(\\.[a-z0-9_\\+-]+)*@[a-z0-9-]+(\\.[a-z0-9]+)*\\.([a-z]{2,4})$", ErrorMessage = "Enter a valid email id")]
        public string Username { get; set; }

        [Required(ErrorMessage = "{0} is required")]
        [DataType(DataType.Password)]
        [StringLength(32, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 8)]
        [RegularExpression(@"^(?=.*?[a-z])(?=.*?[A-Z])(?=.*?[0-9])(?=.*?[!#\$%&'\(\)\*\+,-\.\/:;<=>\?@[\]\^_`\{\|}~])[a-zA-Z0-9!#\$%&'\(\)\*\+,-\.\/:;<=>\?@[\]\^_`\{\|}~]{0,}$", ErrorMessage = "The password must contain atleast one uppercase, one lowercase, one digit and one special character")]
        public string Password { get; set; }

        public string EncUsername { get; set; }

        public string EncPassword { get; set; }

        [DisplayName("Remember me")]
        public bool RememberMe { get; set; }

        [DisplayName("Captcha Code")]
        [Required(ErrorMessage = "{0} is required")]
        [StringLength(4)]
        public string CaptchaCode { get; set; }
    }
}
