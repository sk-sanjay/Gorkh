using Application.Dtos;
using Application.Extensions;
using Application.ServiceInterfaces;
using Application.ViewModels;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
namespace WebAPI.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class UsersController : Controller
    {
        private readonly IIdentityService _identityService;
        private readonly IMapper _mapper;
        private readonly IEmailService _emailService;
        private readonly IRandomService _randomService;
        private readonly IFileService _fileService;
        public UsersController(
            IIdentityService identityService,
            IMapper mapper,
            IEmailService emailService,
            IRandomService randomService,
            IFileService fileService)
        {
            _identityService = identityService;
            _mapper = mapper;
            _emailService = emailService;
            _randomService = randomService;
            _fileService = fileService;
        }
        [Authorize(Roles = "SuperAdmin,Admin")]
        // GET Users
        [HttpGet("Get")]
        public async Task<IActionResult> Get()
        {
            var users = await _identityService.GetUsers().ConfigureAwait(false);
            if (users == null || users.Count <= 0) return NotFound("Users not found");
            var userVms = _mapper.Map<List<UserVM>>(users);
            return Ok(userVms);
        }

        [Authorize(Roles = "SuperAdmin,Admin,QualityPM")]
        // GET GetById
        [HttpGet("GetById/{uid}")]
        public async Task<IActionResult> GetById(string uid)
        {
            if (string.IsNullOrEmpty(uid))
                return BadRequest("UserId is required");
            var user = await _identityService.GetUser(uid).ConfigureAwait(false);
            if (user == null)
                return BadRequest("User does not exist");
            var userTR = _mapper.Map<RegisterDTO>(user);
            var userRolesVm = await _identityService.GetRolesForUser(user).ConfigureAwait(false);
            userTR.Role = userRolesVm.userRoles.First();
            return Ok(userTR);
        }

        [Authorize(Roles = "SuperAdmin,Admin")]
        // POST "Users/Create"
        [HttpPost("Create")]
        public async Task<IActionResult> Create([FromBody] RegisterDTO model)
        {
            //Password
            model.Password = await _randomService.RandomPassword().ConfigureAwait(false);
            var user = await _identityService.CreateUser(model).ConfigureAwait(false);
            if (user == null) return BadRequest("User couldn't be created or added to specified role");
            var userTR = _mapper.Map<RegisterDTO>(user);
            //send the username and password to new User
            var body = string.Format("<html>" + Environment.NewLine +
                                     "<body>" + Environment.NewLine +
                                     "<h3>Respected User</h3>" + Environment.NewLine +
                                     "<p>Congratulations, your registration on <b>{0}</b> Surplus Platform has been successful.</p>" + Environment.NewLine +
                                     "<p>Your one time credentials for login into the system  are:</p>" + Environment.NewLine +
                                     "<p>Username: <b>{1}</b></p>" + Environment.NewLine +
                                     "<p>Password: <b>{2}</b></p>" + Environment.NewLine + Environment.NewLine +
                                     "<p><b>Note</b>: This password is a one time temporary password, you are highly recommended to change your password on your first login.</p>" + Environment.NewLine + Environment.NewLine +
                                     "<p>Regards</p>" + Environment.NewLine +
                                     "<p>Surplus Platform Administration</p>" + Environment.NewLine +
                                     "</body>" + Environment.NewLine +
                                     "</html>", model.Role, user.UserName, model.Password);
            var EmailVm = new EmailVM
            {
                ToAddresses = new List<string> { model.Email },
                Subject = "Account credentials",
                Body = body
            };
            await _emailService.SendEmailAsync(model.Email, "Account credentials", body, null, null, null, null).ConfigureAwait(false);
            //await _emailService.SendEmailAsync(EmailVm).ConfigureAwait(false);
            return Ok(userTR);
        }
        [Authorize(Roles = "SuperAdmin,Admin")]
        // PUT: "Users/Edit/id
        [HttpPut("Edit/{id}")]
        public async Task<IActionResult> Edit([FromRoute] string id, [FromBody] RegisterDTO model)
        {
            if (model == null) return BadRequest("Input not valid or null");
            if (id != model.Id) return BadRequest("Invalid Id");
            if (!ModelState.IsValid) return BadRequest(ModelState.GetErrorMessages());
            var user = await _identityService.GetUser(id).ConfigureAwait(false);
            if (user == null) return BadRequest("Input not valid or null");
            user.Name = model.Name;
            user.Email = model.Email;
            user.PhoneNumber = model.PhoneNumber;
            user.Approved = model.Approved;
            user.IsActive = model.IsActive;
            var result = await _identityService.Update(user).ConfigureAwait(false);
            if (!result.Succeeded) return BadRequest("User couldn't be updated");
            var userTR = _mapper.Map<RegisterDTO>(user);
            return Ok(userTR);
        }
        [Authorize(Roles = "SuperAdmin,Admin")]
        // GET: Users/UsersForList
        [HttpGet("UsersForList")]
        public async Task<IActionResult> UsersForList()
        {
            var users = await _identityService.GetUsers().ConfigureAwait(false);
            var userTR = _mapper.Map<List<UserVM>>(users);
            return Ok(userTR);
        }
        [Authorize]
        // GET: Users/UserById
        [HttpGet("UserById/{uid}")]
        public async Task<IActionResult> UserById([FromRoute] string uid)
        {
            if (string.IsNullOrEmpty(uid))
                return BadRequest("UserId is required");
            var user = await _identityService.GetUser(uid).ConfigureAwait(false);
            if (user == null)
                return BadRequest("User does not exist");
            var userTR = _mapper.Map<UserProfileDTO>(user);
            return Ok(userTR);
        }
        //Method accepting MultipartFormData with IFormFile
        // GET: Users/UpdateUser
        [Authorize]
        [HttpPost("UpdateUser")]
        public async Task<IActionResult> UpdateUser()
        {
            var Id = Request.Form["Id"].ToString();
            var Name = Request.Form["Name"].ToString();
            var Email = Request.Form["Email"].ToString();
            var PhoneNumber = Request.Form["PhoneNumber"].ToString();
            var ProfileImage = Request.Form["ProfileImage"].ToString();
            var user = await _identityService.GetUser(Id).ConfigureAwait(false);
            if (user == null)
                return BadRequest("User does not exist");
            if (Request.Form.Files != null && Request.Form.Files.Count > 0)
            {
                var ImageFile = Request.Form.Files[0];
                if (ImageFile != null && ImageFile.Length > 0 && _fileService.CheckImageFile(ImageFile) && _fileService.CheckFileSize(ImageFile))
                {
                    var FileUploadDto = new FileUploadDTO
                    {
                        UploadedFile = ImageFile,
                        FilePath = "\\img\\users",
                        ChangeName = true,
                        ReturnValue = "name",
                        FileOldName = ProfileImage,
                        ChangeDimensions = true,
                        Width = 128,
                        Height = 128
                    };
                    if (!string.IsNullOrEmpty(ProfileImage) && ProfileImage != "default_user100.png")
                        //Delete previous file
                        _fileService.DeleteFile(!string.IsNullOrEmpty(FileUploadDto.FileOldName)
                            ? $"{FileUploadDto.FilePath}\\{FileUploadDto.FileOldName}"
                            : $"{FileUploadDto.FilePath}\\{FileUploadDto.UploadedFile.FileName}");

                    ProfileImage = await _fileService.SaveFileAsync(FileUploadDto).ConfigureAwait(false);
                }
            }
            user.Name = Name;
            user.Email = Email;
            user.PhoneNumber = PhoneNumber;
            user.ProfileImage = ProfileImage;
            var result = await _identityService.Update(user).ConfigureAwait(false);
            if (!result.Succeeded) return BadRequest("User couldn't be updated");
            var userTR = _mapper.Map<UserProfileDTO>(user);
            return Ok(userTR);
        }
        // GET: Users/UserRoles
        [Authorize(Roles = "SuperAdmin,Admin")]
        [HttpGet("UserRoles/{uid}")]
        public async Task<IActionResult> UserRoles([FromRoute] string uid)
        {
            if (string.IsNullOrEmpty(uid))
                return BadRequest("UserId is required");
            var user = await _identityService.GetUser(uid).ConfigureAwait(false);
            if (user == null) return BadRequest("User does not exist");
            var UserRoleVm = await _identityService.GetRolesForUser(user).ConfigureAwait(false);
            if (UserRoleVm == null)
                return BadRequest("User does not exist");
            return Ok(UserRoleVm);
        }
        // POST: Users/AddToRole
        [Authorize(Roles = "SuperAdmin,Admin")]
        [HttpPost("AddToRole")]
        public async Task<IActionResult> AddToRole([FromBody] UserRolesVM model)
        {
            if (string.IsNullOrEmpty(model.userId))
                return BadRequest("UserId is required");
            var user = await _identityService.GetUser(model.userId).ConfigureAwait(false);
            if (user == null)
                return BadRequest("User does not exist");
            var result = await _identityService.AddToRole(user, model.userRole).ConfigureAwait(false);
            if (result.Succeeded)
            {
                var UserRoleVm = await _identityService.GetRolesForUser(user).ConfigureAwait(false);
                return Ok(UserRoleVm);
            }
            return BadRequest("User couldn't be added to role");
        }
        [Authorize(Roles = "SuperAdmin,Admin")]
        // POST: Users/RemoveFromRole
        [HttpPost("RemoveFromRole")]
        public async Task<IActionResult> RemoveFromRole([FromBody] UserRolesVM model)
        {
            if (string.IsNullOrEmpty(model.userId))
                return BadRequest("UserId is required");
            var user = await _identityService.GetUser(model.userId).ConfigureAwait(false);
            if (user == null)
                return BadRequest("User does not exist");
            var result = await _identityService.RemoveFromRole(user, model.userRole).ConfigureAwait(false);
            if (result.Succeeded)
            {
                var UserRoleVm = await _identityService.GetRolesForUser(user).ConfigureAwait(false);
                return Ok(UserRoleVm);
            }
            return BadRequest("User couldn't be removed from role");
        }
        // GET Users Dropdown
        [Authorize(Roles = "SuperAdmin,Admin")]
        [HttpGet("GetDropdown")]
        public async Task<IActionResult> GetDropdown()
        {
            var users = await _identityService.GetUsers().ConfigureAwait(false);
            if (users == null || users.Count <= 0) return NotFound("Users not found");
            var dropDownStrVms = _mapper.Map<List<DropdownStrVM>>(users.Where(x => x.Approved).ToList());
            return Ok(dropDownStrVms);
        }
        // GET: Users/GetRole
        [Authorize(Roles = "SuperAdmin,Admin")]
        [HttpGet("GetRole/{uid}")]
        public async Task<IActionResult> GetRole([FromRoute] string uid)
        {
            if (string.IsNullOrEmpty(uid)) return BadRequest("UserId is required");
            var user = await _identityService.GetUser(uid).ConfigureAwait(false);
            if (user == null) return BadRequest("User does not exist");
            var UserRoleVm = await _identityService.GetRoleForUser(user).ConfigureAwait(false);
            if (UserRoleVm == null) return BadRequest("User does not exist");
            return Ok(UserRoleVm);
        }
        // POST: Users/UpdateUserRole
        [Authorize(Roles = "SuperAdmin,Admin")]
        [HttpPost("UpdateUserRole")]
        public async Task<IActionResult> UpdateUserRole([FromBody] UserRoleVM model)
        {
            //The Validations
            if (string.IsNullOrEmpty(model.UserId)) return BadRequest("UserId is required");
            if (string.IsNullOrEmpty(model.Role)) return BadRequest("Role is required");
            //The User
            var user = await _identityService.GetUser(model.UserId).ConfigureAwait(false);
            if (user == null) return BadRequest("User does not exist");
            //The Roles
            var userRoles = (await _identityService.GetUserRoles(user).ConfigureAwait(false)).Distinct().ToList();
            if (userRoles.Count > 0)
            {
                foreach (var role in userRoles)
                {
                    var result1 = await _identityService.RemoveFromRole(user, role).ConfigureAwait(false);
                    if (!result1.Succeeded) return BadRequest("User couldn't be removed from existing role");
                }
            }
            var result2 = await _identityService.AddToRole(user, model.Role).ConfigureAwait(false);
            if (!result2.Succeeded) return BadRequest("User couldn't be added to specied role");
            return Ok(true);
        }
        [Authorize]
        // GET: Users/UpdatePasswordStatus
        [HttpGet("UpdatePasswordStatus/{uid}/{status}")]
        public async Task<IActionResult> UpdatePasswordStatus([FromRoute] string uid, [FromRoute] int status)
        {
            var user = await _identityService.GetUser(uid).ConfigureAwait(false);
            if (user == null)
                return BadRequest("User does not exist");
            user.ChangePassword = Convert.ToBoolean(status);
            var result = await _identityService.Update(user).ConfigureAwait(false);
            if (!result.Succeeded) return BadRequest("Password status couldn't be updated");
            return Ok(true);
        }
        [Authorize]
        // GET: Users/GetByRole
        [HttpGet("GetByRole/{role}")]
        public async Task<IActionResult> GetByRole([FromRoute] string role)
        {
            var users = await _identityService.GetUsersByRole(role).ConfigureAwait(false);
            if (users == null || users.Count <= 0)
                return BadRequest("No users in this role");
            var userVms = _mapper.Map<List<UserVM>>(users.Distinct());
            return Ok(userVms);
        }

        [Authorize]
        // GET: Buyer and Seller Count
        [HttpGet("GetBuyerandSellerCount")]
        public async Task<IActionResult> GetBuyerandSellerCount()
        {
                var dropDownVms = await _identityService.GetBuyerandSellerCount().ConfigureAwait(false);
            if (dropDownVms == null) return NotFound("Buyer and SellerCount not found");
                return Ok(dropDownVms);
           
        }

        [Authorize]
        // GET: Product Count
        [HttpGet("GetProductCount")]
        public async Task<IActionResult> GetProductCount()
        {
            var dropDownVms = await _identityService.GetProductCount().ConfigureAwait(false);
            if (dropDownVms == null) return NotFound("Product not found");
            return Ok(dropDownVms);

        }

        [Authorize]
        // GET: Product Count
        [HttpGet("GetProductCountbyCategory")]
        public async Task<IActionResult>GetProductCountbyCategory()
        {
            var dropDownVms = await _identityService.GetProductCountbyCategory().ConfigureAwait(false);
            if (dropDownVms == null) return NotFound("Product not found");
            return Ok(dropDownVms);

        }

        [AllowAnonymous]
        [HttpGet("ActiveDeactiveUser/{modelStr}")]
        public async Task<IActionResult> ActiveDeactiveUser([FromRoute] string modelStr)
        {
            var rowsChanged = 0;
            if (string.IsNullOrEmpty(modelStr)) return BadRequest("Input not valid or null");
            var idsStr = JsonConvert.DeserializeObject<RegisterDTO>(modelStr);
            rowsChanged = await _identityService.ActiveDeactiveUser(idsStr.Id).ConfigureAwait(false);
            if (rowsChanged > 0) return Ok(rowsChanged);
            return BadRequest("Product(s) couldn't be updated");
        }
    }
}
