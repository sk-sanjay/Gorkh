using Microsoft.AspNetCore.Identity;
namespace Application.Dtos
{
    public class ApplicationUser : IdentityUser
    {
        public string Name { get; set; }
        public string ProfileImage { get; set; }
        public bool Approved { get; set; }
        public bool IsActive { get; set; }
        public bool ChangePassword { get; set; }
        public string EncSecret { get; set; }

        public int SellerId { get; set; }
        public int BuyerId { get; set; }
    }
}
