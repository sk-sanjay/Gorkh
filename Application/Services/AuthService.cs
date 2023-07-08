using Application.AppSettings;
using Application.Dtos;
using Application.Helpers;
using Application.ServiceInterfaces;
using Application.ViewModels;
using Domain.Models;
using Domain.RepositoryInterfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IJwtService _jwtService;
        private readonly JwtSettings _jwtOptions;
        private readonly IConfiguration _config;
        private readonly RoleManager<ApplicationRole> _roleManager;
        public AuthService(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IUnitOfWork unitOfWork,
            IJwtService jwtService,
            IOptions<JwtSettings> jwtOptions,
            IConfiguration config, RoleManager<ApplicationRole> roleManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _unitOfWork = unitOfWork;
            _jwtService = jwtService;
            _config = config;
            _roleManager = roleManager;
            _jwtOptions = jwtOptions.Value;
        }
        public async Task<bool> CheckUsername(string uname)
        {
            var userFrmDb = await _userManager.FindByNameAsync(uname).ConfigureAwait(false);
            return userFrmDb != null;
        }
        public async Task<bool> CheckEmail(string email)
        {
            var userFrmDb = await _userManager.FindByEmailAsync(email).ConfigureAwait(false);
            return userFrmDb != null;
        }

        public async Task<BuyerSellerRegistrationsVM> GetbyEmail(string email)
        {
            var userFrmDb = await _userManager.FindByEmailAsync(email).ConfigureAwait(false);
            if (userFrmDb != null)
            {
                BuyerSellerRegistrationsVM data = new BuyerSellerRegistrationsVM
                {
                    Email = userFrmDb.UserName,
                    BuyerId = userFrmDb.BuyerId,
                    SellerId = userFrmDb.SellerId,
                    FirstName=userFrmDb.Name
                };
                return data;
            }
            return null;
        }

        public async Task<TokenVM> Login(LoginDTO model)
        {
            //Decrypt the UserName and Password
            model.Username = EnDeCryptor.DecryptStringAES(model.EncUsername);
            model.Password = EnDeCryptor.DecryptStringAES(model.EncPassword.Substring(8));
            // find user with this username
            var User = await _userManager.FindByNameAsync(model.Username).ConfigureAwait(false);
            if (User == null || User.Approved == false || User.IsActive == false) return null;
            var AccountLockedOut = await _userManager.IsLockedOutAsync(User).ConfigureAwait(false);
            if (AccountLockedOut) return null;
            //verify the password
            var pwdVerified = await _userManager.CheckPasswordAsync(User, model.Password).ConfigureAwait(false);
            if (!pwdVerified)
            {
                await _userManager.AccessFailedAsync(User).ConfigureAwait(false);
                return null;
            }
            //get role for this user
            var rolename = ((List<string>)await _userManager.GetRolesAsync(User).ConfigureAwait(false)).First();
            var Role = await _roleManager.FindByNameAsync(rolename).ConfigureAwait(false);
            var TokenVm = await GetTokenAsync(User, Role).ConfigureAwait(false);
            return TokenVm;
        }
        public async Task<TokenVM> PrivilegeLogin(string Id)
        {
            // find user with this user id
            var User = await _userManager.FindByIdAsync(Id).ConfigureAwait(false);
            if (User == null || User.Approved == false || User.IsActive == false) return null;
            var AccountLockedOut = await _userManager.IsLockedOutAsync(User).ConfigureAwait(false);
            if (AccountLockedOut) return null;
            //get role for this user
            var rolename = ((List<string>)await _userManager.GetRolesAsync(User).ConfigureAwait(false)).First();
            var Role = await _roleManager.FindByNameAsync(rolename).ConfigureAwait(false);
            //get mapids for this user
            var mappings = new Dictionary<string, List<int>>();
            //var UserMappings = await _unitOfWork.UserMappingRepo.GetByUserName(User.UserName).ConfigureAwait(false);
            //if (UserMappings != null && UserMappings.Count > 0)
            //{
            //    foreach (var mapping in UserMappings.Where(mapping => !mappings.TryAdd(mapping.MapsTo, new List<int> { mapping.MapId })))
            //        mappings[mapping.MapsTo].Add(mapping.MapId);
            //}
            var TokenVm = await GetTokenAsync(User, Role).ConfigureAwait(false);
            return TokenVm;
        }

        public async Task<TokenVM> PrivilegeLoginBuyer(string Id, string role)
        {
            // find user with this user id
            var User = await _userManager.FindByIdAsync(Id).ConfigureAwait(false);
            if (User == null || User.Approved == false || User.IsActive == false) return null;
            var AccountLockedOut = await _userManager.IsLockedOutAsync(User).ConfigureAwait(false);
            if (AccountLockedOut) return null;
            //get role for this user
            //var rolename = ((List<string>)await _userManager.GetRolesAsync(User).ConfigureAwait(false)).First();
            var Role = await _roleManager.FindByNameAsync(role).ConfigureAwait(false);
            var TokenVm = await GetTokenAsync(User, Role).ConfigureAwait(false);
            return TokenVm;
        }

        public async Task<TokenVM> PrivilegeLoginSeller(string Id, string role)
        {
            // find user with this user id
            var User = await _userManager.FindByIdAsync(Id).ConfigureAwait(false);
            if (User == null || User.Approved == false || User.IsActive == false) return null;
            var AccountLockedOut = await _userManager.IsLockedOutAsync(User).ConfigureAwait(false);
            if (AccountLockedOut) return null;
            //get role for this user
            //var rolename = ((List<string>)await _userManager.GetRolesAsync(User).ConfigureAwait(false)).First();
            var Role = await _roleManager.FindByNameAsync(role).ConfigureAwait(false);
            var TokenVm = await GetTokenAsync(User, Role).ConfigureAwait(false);
            return TokenVm;
        }








        //public async Task<TokenVM> SellerandBuyerLogin(string Id, int Flag)
        //{
        //    // find user with this user id
        //    var User = await _userManager.FindByIdAsync(Id).ConfigureAwait(false);
        //    if (User == null || User.Approved == false || User.IsActive == false) return null;
        //    var AccountLockedOut = await _userManager.IsLockedOutAsync(User).ConfigureAwait(false);
        //    if (AccountLockedOut) return null;
        //    var rolename = Flag == 0 ? "Seller" : "Buyer";
        //    var Role = await _roleManager.FindByNameAsync(rolename).ConfigureAwait(false);
        //    var TokenVm = await GetTokenAsync(User, Role).ConfigureAwait(false);
        //    return TokenVm;
        //}

        public async Task<TokenVM> Register(RegisterDTO model)
        {
            //check if user already exists
            var userFrmDb = await _userManager.FindByNameAsync(model.Username).ConfigureAwait(false);
            if (userFrmDb != null)
                return null;
            //create new user and add to AspNetUsers
            var user = new ApplicationUser
            {
                UserName = model.Username,
                Name = model.Name,
                Email = model.Email,
                EmailConfirmed = true,
                PhoneNumber = model.PhoneNumber,
                PhoneNumberConfirmed = true,
                ProfileImage = model.ProfileImage,
                Approved = model.Approved,
                IsActive = model.IsActive,
                ChangePassword = model.ChangePassword,
                SellerId = model.SellerId,
                BuyerId = model.BuyerId,

                EncSecret = model.EncSecret
            };
            var result = await _userManager.CreateAsync(user, model.Password).ConfigureAwait(false);
            if (!result.Succeeded) return null;
            //add user to Anonymous role
            model.Role = string.IsNullOrEmpty(model.Role) ? _config["DefaultUserRole"] : model.Role;
            await _userManager.AddToRoleAsync(user, model.Role).ConfigureAwait(false);
            var Role = await _roleManager.FindByNameAsync(model.Role).ConfigureAwait(false);
            var TokenVm = await GetTokenAsync(user, Role).ConfigureAwait(false);
            return TokenVm;
        }
        public async Task<TokenVM> RefreshToken(string refreshtoken)
        {
            var refreshTokenDb = await _unitOfWork.RefreshTokensRepo.GetRefreshToken(refreshtoken).ConfigureAwait(false);
            if (refreshTokenDb == null || refreshTokenDb.ExpiresUtc < DateTime.UtcNow)
                return null;
            var User = await _userManager.FindByIdAsync(refreshTokenDb.UserId).ConfigureAwait(false);
            if (User == null || User.Approved == false || User.IsActive == false || !await _signInManager.CanSignInAsync(User).ConfigureAwait(false))
                return null;
            if (_userManager.SupportsUserLockout && await _userManager.IsLockedOutAsync(User).ConfigureAwait(false))
                return null;
            //get role for this user
            var rolename = ((List<string>)await _userManager.GetRolesAsync(User).ConfigureAwait(false)).First();
            var Role = await _roleManager.FindByNameAsync(rolename).ConfigureAwait(false);
            //get mapids for this user
            var mappings = new Dictionary<string, List<int>>();
            //var UserMappings = await _unitOfWork.UserMappingRepo.GetByUserName(User.Name).ConfigureAwait(false);
            //if (UserMappings != null && UserMappings.Count > 0)
            //{
            //    foreach (var mapping in UserMappings.Where(mapping => !mappings.TryAdd(mapping.MapsTo, new List<int> { mapping.MapId })))
            //        mappings[mapping.MapsTo].Add(mapping.MapId);
            //}
            //generate encoded access token
            var encodedToken = await _jwtService.GenerateEncodedToken(User, Role).ConfigureAwait(false);
            //generate TokenVM
            var TokenVm = new TokenVM
            {
                AccessToken = new JwtSecurityTokenHandler().WriteToken(encodedToken),
                RefreshToken = refreshTokenDb.Token
            };
            return TokenVm;
        }
        public async Task<ForgotPasswordVM> GeneratePasswordResetToken(string username)
        {
            // find user with this username
            var user = await _userManager.FindByNameAsync(username).ConfigureAwait(false);
            if (user == null || user.Approved == false || user.IsActive == false) return null;
            //Get the code
            var tokenGenerated = await _userManager.GeneratePasswordResetTokenAsync(user).ConfigureAwait(false);
            var tokenGeneratedBytes = Encoding.UTF8.GetBytes(tokenGenerated);
            var codeEncoded = WebEncoders.Base64UrlEncode(tokenGeneratedBytes);
            var ForgotPasswordVm = new ForgotPasswordVM
            {
                Id = user.EmailConfirmed ? user.Id : null,
                Email = user.EmailConfirmed ? user.Email : null,
                Code = user.EmailConfirmed ? codeEncoded : null
            };
            return ForgotPasswordVm;
        }
        public async Task<string> GetPasswordResetToken(string userid)
        {
            // find user with this username
            var user = await _userManager.FindByIdAsync(userid).ConfigureAwait(false);
            var tokenGenerated = await _userManager.GeneratePasswordResetTokenAsync(user).ConfigureAwait(false);
            var tokenGeneratedBytes = Encoding.UTF8.GetBytes(tokenGenerated);
            var codeEncoded = WebEncoders.Base64UrlEncode(tokenGeneratedBytes);
            return codeEncoded;
        }
        public async Task<EmailDTO> GetPasswordResetTokenByEmail(string email)
        {
            // find user with this email
            var user = await _userManager.FindByEmailAsync(email).ConfigureAwait(false);
            if (user == null || user.Approved == false || user.IsActive == false) return null;
            //get the code
            var tokenGenerated = await _userManager.GeneratePasswordResetTokenAsync(user).ConfigureAwait(false);
            var tokenGeneratedBytes = Encoding.UTF8.GetBytes(tokenGenerated);
            var codeEncoded = WebEncoders.Base64UrlEncode(tokenGeneratedBytes);
            var emailDto = new EmailDTO
            {
                Id = user.Id,
                Email = email,
                Code = codeEncoded
            };
            return emailDto;
        }
        public async Task<string> GetUsernameByEmail(string email)
        {
            // find user with this email
            var user = await _userManager.FindByEmailAsync(email).ConfigureAwait(false);
            return user == null || user.Approved == false || user.IsActive == false ? string.Empty : user.UserName;
        }

        public async Task<List<ApplicationUser>> GetUsersByUserNames(List<string> UserNames)
        {
            var Users = new List<ApplicationUser>(UserNames.Count);
            foreach (var UserName in UserNames)
            {
                var user = await _userManager.FindByNameAsync(UserName);
                if (user == null) continue;
                Users.Add(user);
            }
            return Users;
        }

        public async Task<bool> ResetPassword(ResetPasswordDTO model)
        {
            // find user with this userid
            var user = await _userManager.FindByIdAsync(model.UserId).ConfigureAwait(false);
            if (user == null || user.Approved == false || user.IsActive == false) return false;
            if (string.IsNullOrEmpty(model.Code)) return false;
            user.EncSecret = model.EncSecret;
            model.Password = EnDeCryptor.DecryptStringAES(model.EncPassword.Substring(8));
            //reset the password
            var codeDecodedBytes = WebEncoders.Base64UrlDecode(model.Code);
            var codeDecoded = Encoding.UTF8.GetString(codeDecodedBytes);
            var result = await _userManager.ResetPasswordAsync(user, codeDecoded, model.Password).ConfigureAwait(false);
            return result.Succeeded;
        }
        public async Task<bool> ChangePassword(ChangePasswordDTO model)
        {
            // find user with this username
            var user = await _userManager.FindByNameAsync(model.Username).ConfigureAwait(false);
            if (user == null || user.Approved == false || user.IsActive == false) return false;
            //change the password
            var changePasswordResult = await _userManager.ChangePasswordAsync(user, model.Password, model.NewPassword).ConfigureAwait(false);
            //set password status in user
            user.ChangePassword = false;
            user.EncSecret = model.EncSecret;
            var userPasswordStatusResult = await _userManager.UpdateAsync(user);
            //return true only if both are success
            return changePasswordResult.Succeeded && userPasswordStatusResult.Succeeded;
        }
        private async Task<TokenVM> GetTokenAsync(ApplicationUser User, ApplicationRole Role)
        {
            //try get refresh token of this user from the database
            var refreshTokenDb = await _unitOfWork.RefreshTokensRepo.GetRefreshTokenByUserId(User.Id).ConfigureAwait(false);
            if (refreshTokenDb != null)
            {
                _unitOfWork.RefreshTokensRepo.Delete(refreshTokenDb);
                //await _unitOfWork.SaveChangesAsync().ConfigureAwait(false);
            }
            var newRefreshToken = new RefreshToken
            {
                Id = Guid.NewGuid().ToString(),
                UserId = User.Id,
                Token = _jwtService.GenerateRefreshToken(),
                IssuedUtc = _jwtOptions.IssuedAt,
                ExpiresUtc = _jwtOptions.RefreshTokenExpiresUtc
            };
            _unitOfWork.RefreshTokensRepo.Create(newRefreshToken);
            await _unitOfWork.SaveChangesAsync().ConfigureAwait(false);
            //generate encoded access token
            var encodedToken = await _jwtService.GenerateEncodedToken(User, Role).ConfigureAwait(false);
            //generate TokenVM
            return new TokenVM
            {
                AccessToken = new JwtSecurityTokenHandler().WriteToken(encodedToken),
                RefreshToken = newRefreshToken.Token
            };
        }
        public async Task<TokenVM> SellerLogin(LoginDTO model)
        {
            // string addResponse = "";
            TokenVM tokenVm = new TokenVM();
            //CommonClass obj = new CommonClass();
            //Root res = new Root();

            // find user with this username
            var user = await _userManager.FindByNameAsync(model.Username).ConfigureAwait(false);
            if (user == null) return null;
            var AccountLockedOut = await _userManager.IsLockedOutAsync(user).ConfigureAwait(false);
            if (AccountLockedOut) return null;
            //verify the password
            var pwdVerified = await _userManager.CheckPasswordAsync(user, model.Password).ConfigureAwait(false);
            if (!pwdVerified)
            {
                await _userManager.AccessFailedAsync(user).ConfigureAwait(false);
                return null;
            }
            //get role for this user
            var rolename = ((List<string>)await _userManager.GetRolesAsync(user).ConfigureAwait(false)).First();
            var Role = await _roleManager.FindByNameAsync(rolename).ConfigureAwait(false);
            var TokenVm = await GetTokenAsync(user, Role).ConfigureAwait(false);
            return TokenVm;

        }

        public async Task<int> UpdateBuyerId(CommonRegisterDTO modelDto)
        {
            if (modelDto == null) return 0;
            //var model = await _unitOfWork.BuyerSellerRegistrationsRepo.Get(modelDto.Id).ConfigureAwait(false);
            var model = await _userManager.FindByEmailAsync(modelDto.Emailid).ConfigureAwait(false);
            model.BuyerId = modelDto.BuyerId;
            await _userManager.AddToRoleAsync(model, "Buyer").ConfigureAwait(false);
            var rowsChanged = await _unitOfWork.SaveChangesAsync().ConfigureAwait(false);
            return rowsChanged;
        }

        public async Task<int> UpdateBuyerName(CommonRegisterDTO modelDto)
        {
            if (modelDto == null) return 0;
            //var model = await _unitOfWork.BuyerSellerRegistrationsRepo.Get(modelDto.Id).ConfigureAwait(false);
            var model = await _userManager.FindByEmailAsync(modelDto.Emailid).ConfigureAwait(false);
            // model.BuyerId = modelDto.BuyerId;
            model.Name = modelDto.Name;
            //await _userManager.AddToRoleAsync(model, "Buyer").ConfigureAwait(false);
            var rowsChanged = await _unitOfWork.SaveChangesAsync().ConfigureAwait(false);
            return rowsChanged;
        }


        public async Task<int> UpdateSellerId(CommonRegisterDTO modelDto)
        {
            if (modelDto == null) return 0;

            var model = await _userManager.FindByEmailAsync(modelDto.Emailid).ConfigureAwait(false);
            model.SellerId = modelDto.SellerId;
            await _userManager.AddToRoleAsync(model, "Seller").ConfigureAwait(false);
            var rowsChanged = await _unitOfWork.SaveChangesAsync().ConfigureAwait(false);
            return rowsChanged;
        }

    }
}
