using Application.Dtos;
using Application.Extensions;
using Application.ServiceInterfaces;
using Application.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Domain.RepositoryInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Domain.Models;

namespace Application.Services
{
    public class IdentityService : IIdentityService
    {
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public IdentityService(
            RoleManager<ApplicationRole> roleManager,
            UserManager<ApplicationUser> userManager, IUnitOfWork unitOfWork, IMapper mapper)
        {
            _roleManager = roleManager;
            _userManager = userManager;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        //Users
        public Task<List<ApplicationUser>> GetUsers()
        {
            return _userManager.Users.OrderBy(x => x.UserName).ToListAsync();
        }


        public Task<ApplicationUser> GetUser(int sellerid)
        {
            return _userManager.FindByIdAsync(sellerid.ToString());
        }


        public Task<ApplicationUser> GetUser(string id)
        {
            return _userManager.FindByIdAsync(id);
        }
        public Task<IList<ApplicationUser>> GetUsersByRole(string role)
        {
            return _userManager.GetUsersInRoleAsync(role);
        }
        //Roles
        public Task<List<ApplicationRole>> GetRoles()
        {
            return _roleManager.Roles.OrderBy(r => r.Name).ToListAsync();
        }
        public Task<ApplicationRole> GetRole(string id)
        {
            return _roleManager.FindByIdAsync(id);
        }
        public Task<ApplicationRole> GetRoleByName(string rolename)
        {
            return _roleManager.FindByNameAsync(rolename);
        }
        public Task<IdentityResult> AddRole(ApplicationRole role)
        {
            return _roleManager.CreateAsync(role);
        }
        public Task<IdentityResult> EditRole(ApplicationRole role)
        {
            return _roleManager.UpdateAsync(role);
        }
        public Task<IdentityResult> DeleteRole(ApplicationRole role)
        {
            return _roleManager.DeleteAsync(role);
        }
        public Task<bool> RoleExists(string rolename)
        {
            return _roleManager.RoleExistsAsync(rolename);
        }
        public async Task<List<string>> GetUserRoles(ApplicationUser user)
        {
            var userroles = await _userManager.GetRolesAsync(user).ConfigureAwait(false);
            return userroles.ToList();
        }
        public async Task<UserRolesVM> GetRolesForUser(ApplicationUser user)
        {
            var userRoles = await _userManager.GetRolesAsync(user).ConfigureAwait(false);
            var roles = await _roleManager.Roles.ToListAsync().ConfigureAwait(false);
            var remainingRoles = (from role in roles where !userRoles.Contains(role.Name) select role.Name).ToList();
            var urVM = new UserRolesVM
            {
                userId = user.Id,
                user = user,
                userRoles = userRoles.OrderBy(ur => ur).ToList(),
                remainingRoles = remainingRoles.OrderBy(rr => rr).ToList()
            };
            return urVM;
        }
        public async Task<UserRoleVM> GetRoleForUser(ApplicationUser user)
        {
            var userRoles = await _userManager.GetRolesAsync(user).ConfigureAwait(false);
            var urVM = new UserRoleVM
            {
                UserId = user.Id,
                Role = userRoles != null && userRoles.Count > 0 ? userRoles.First() : "Anonymous"
            };
            return urVM;
        }
        public Task<IdentityResult> AddToRole(ApplicationUser user, string role)
        {
            return _userManager.AddToRoleAsync(user, role);
        }
        public Task<IdentityResult> RemoveFromRole(ApplicationUser user, string role)
        {
            return _userManager.RemoveFromRoleAsync(user, role);
        }

        //NotInUse
        //public Task<IdentityResult> RemoveFromRoles(ApplicationUser user, List<string> roles)
        //{
        //    return _userManager.RemoveFromRolesAsync(user, roles);
        //}
        public Task<IdentityResult> Update(ApplicationUser user)
        {
            return _userManager.UpdateAsync(user);
        }
        public async Task<ApplicationUser> CreateUser(RegisterDTO model)
        {
            if (string.IsNullOrEmpty(model.Username))
            {
                //Create new Username
                var users = await _userManager.GetUsersInRoleAsync(model.Role).ConfigureAwait(false);
                var lastUser = users.OrderBy(x => x.UserName).LastOrDefault(x => x.UserName.Contains(model.Role));
                if (lastUser != null && !string.IsNullOrEmpty(lastUser.UserName) && lastUser.UserName.Length > 3)
                    model.Username = $"{model.Role}{Convert.ToInt32(lastUser.UserName.GetLast(3)) + 1:D3}";
                else
                {
                    model.Username = $"{model.Role}001";
                    if (model.Role == "TrainingPartner")
                        model.Username = "TP001";
                    else if (model.Role == "OperatingPartner")
                        model.Username = "OP001";
                    else if (model.Role == "ResourcePartner")
                        model.Username = "RP001";
                }
            }
            //check if username already exists
            var userFrmDb = await _userManager.FindByNameAsync(model.Username).ConfigureAwait(false);
            if (userFrmDb != null)
                return null;
            ////check if email already exists
            //userFrmDb = await _userManager.FindByEmailAsync(model.Email).ConfigureAwait(false);
            //if (userFrmDb != null)
            //    return null;
            //create new user and add to AspNetUsers
            var user = new ApplicationUser
            {
                UserName = model.Username,
                Name = model.Name,
                Email = model.Email,
                PhoneNumber = model.PhoneNumber,
                ProfileImage = "default_user100.png",
                Approved = model.Approved,
                IsActive = model.IsActive,
                EmailConfirmed = true,
                PhoneNumberConfirmed = true,
                ChangePassword = model.ChangePassword,
                EncSecret = model.EncSecret
            };
            var createUserResult = await _userManager.CreateAsync(user, model.Password).ConfigureAwait(false);
            if (!createUserResult.Succeeded) return null;
            //add user to role
            var addToRoleResult = await _userManager.AddToRoleAsync(user, model.Role).ConfigureAwait(false);
            return !addToRoleResult.Succeeded ? null : user;
        }

        public async Task<List<BuyerSellerCountVM>> GetBuyerandSellerCount()
        {
            List<KeyValuePair<string, string>> p = new List<KeyValuePair<string, string>>
            {
                // new KeyValuePair<string, string>("@SubCategoryId", subcategoryid.ToString()),
            };
            var model = await _unitOfWork.BuyerandSellerCountRepo.GetListFromSql("Sp_Buyer_SellerCount", p).ConfigureAwait(false);
            if (model == null) return null;
            var modelVm = _mapper.Map<List<BuyerSellerCountVM>>(model);
            return modelVm;
        }

        public async Task<List<ProductCountVM>> GetProductCount()
        {
            List<KeyValuePair<string, string>> p = new List<KeyValuePair<string, string>>
            {
                // new KeyValuePair<string, string>("@SubCategoryId", subcategoryid.ToString()),
            };
            var model = await _unitOfWork.ProductCountRepo.GetListFromSql("Sp_ProductsCount", p).ConfigureAwait(false);
            if (model == null) return null;
            var modelVm = _mapper.Map<List<ProductCountVM>>(model);
            return modelVm;
        }
        public async Task<List<ProductCountVM>> GetProductCountbyCategory()
        {
            List<KeyValuePair<string, string>> p = new List<KeyValuePair<string, string>>
            {
                // new KeyValuePair<string, string>("@SubCategoryId", subcategoryid.ToString()),
            };
            var model = await _unitOfWork.ProductCountRepo.GetListFromSql("Sp_ProductCountByCategory", p).ConfigureAwait(false);
            if (model == null) return null;
            var modelVm = _mapper.Map<List<ProductCountVM>>(model);
            return modelVm;
        }

        public async Task<int> ActiveDeactiveUser(string id)
        {
            var model = await _userManager.FindByIdAsync(id).ConfigureAwait(false);
            if (model == null) return -1;
            if (!model.IsActive)
            {
                model.IsActive = true;

            }
            else
            {
                model.IsActive = false;
            }
            var rowsChanged = await _unitOfWork.SaveChangesAsync().ConfigureAwait(false);
            return rowsChanged > 0 ? rowsChanged : -1;
        }


    }
}
