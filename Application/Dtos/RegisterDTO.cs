using Application.Helpers;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Application.Dtos
{
    public class RegisterDTO
    {
        public string Id { get; set; }

        [Required(ErrorMessage = "{0} is required")]
        public string Role { get; set; }

        //[Required(ErrorMessage = "{0} is required")]
        //[StringLength(32, ErrorMessage = "{1} characters max")]
        public string Username { get; set; }

        [Required(ErrorMessage = "{0} is required")]
        [StringLength(64, ErrorMessage = "{1} characters max")]
        [RegularExpression("^[a-z0-9_\\+-]+(\\.[a-z0-9_\\+-]+)*@[a-z0-9-]+(\\.[a-z0-9]+)*\\.([a-z]{2,4})$", ErrorMessage = "Enter a valid email id")]
        public string Email { get; set; }

        //[Required(ErrorMessage = "{0} is required")]
        //[DataType(DataType.Password)]
        //[DisplayName("Password")]
        //[StringLength(32, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 8)]
        public string Password { get; set; }

        //[Required(ErrorMessage = "{0} is required")]
        //[DataType(DataType.Password)]
        //[DisplayName("Confirm password")]
        //[Compare("Password", ErrorMessage = "The password and confirmation password must match.")]
        //public string ConfirmPassword { get; set; }

        [Required(ErrorMessage = "{0} is required")]
        [StringLength(10, ErrorMessage = "{1} characters max")]
        [RegularExpression(@"^\d{10}$", ErrorMessage = "Enter a valid 10 digit number")]
        public string PhoneNumber { get; set; }

        //[Required(ErrorMessage = "{0} is required")]
        [StringLength(64, ErrorMessage = "{1} characters max")]
        public string Name { get; set; }

        [StringLength(64, ErrorMessage = "{1} characters max")]
        public string ProfileImage { get; set; }

        public bool Approved { get; set; }

        public bool IsActive { get; set; }

        public bool ChangePassword { get; set; }

        //[DisplayName("Captcha Code")]
        //[Required(ErrorMessage = "{0} is required")]
        //[StringLength(4)]
        public string CaptchaCode { get; set; }

        public int SellerId { get; set; }

        public int BuyerId { get; set; }

        public string EncSecret => string.IsNullOrEmpty(Password) ? null : Convert.ToBase64String(EnDeCryptor.EncryptStringAES(Password));
    }
}
