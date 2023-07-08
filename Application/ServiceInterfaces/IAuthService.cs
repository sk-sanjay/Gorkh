using Application.Dtos;
using Application.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;
namespace Application.ServiceInterfaces
{
    public interface IAuthService
    {
        Task<bool> CheckUsername(string uname);
        Task<bool> CheckEmail(string email);
        Task<TokenVM> Login(LoginDTO model);
        Task<TokenVM> PrivilegeLogin(string Id);
        Task<TokenVM> PrivilegeLoginBuyer(string Id, string role);
        Task<TokenVM> PrivilegeLoginSeller(string Id, string role);
        Task<TokenVM> Register(RegisterDTO model);
        Task<TokenVM> RefreshToken(string refreshtoken);
        Task<ForgotPasswordVM> GeneratePasswordResetToken(string username);
        Task<string> GetPasswordResetToken(string userid);
        Task<EmailDTO> GetPasswordResetTokenByEmail(string email);
        Task<bool> ResetPassword(ResetPasswordDTO model);
        Task<bool> ChangePassword(ChangePasswordDTO model);
        Task<string> GetUsernameByEmail(string email);
        Task<List<ApplicationUser>> GetUsersByUserNames(List<string> UserNames);
        Task<TokenVM> SellerLogin(LoginDTO model);
        Task<BuyerSellerRegistrationsVM> GetbyEmail(string email);
        Task<int> UpdateBuyerId(CommonRegisterDTO modelDto);
        Task<int> UpdateSellerId(CommonRegisterDTO modelDto);
        Task<int> UpdateBuyerName(CommonRegisterDTO modelDto);
        
    }
}
