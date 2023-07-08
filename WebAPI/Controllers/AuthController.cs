using Application.Dtos;
using Application.Extensions;
using Application.ServiceInterfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.Threading.Tasks;
namespace WebAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthController : Controller
    {
        private readonly IAuthService _authService;
        private readonly IConfiguration _config;
        public AuthController(IAuthService authService, IConfiguration config)
        {
            _authService = authService;
            _config = config;
        }
        [HttpGet("CheckUsername/{uname}")]
        public async Task<IActionResult> CheckUsername([FromRoute] string uname)
        {
            var userExists = await _authService.CheckUsername(uname).ConfigureAwait(false);
            return Ok(userExists);
        }

        [HttpGet("GetbyEmail/{email}")]
        public async Task<IActionResult> GetbyEmail([FromRoute] string email)
        {
            var userExists = await _authService.GetbyEmail(email).ConfigureAwait(false);
            return Ok(userExists);
        }


        [HttpGet("CheckEmail/{email}")]
        public async Task<IActionResult> CheckEmail([FromRoute] string email)
        {
            var userExists = await _authService.CheckEmail(email).ConfigureAwait(false);
            return Ok(userExists);
        }
        // POST: Auth/Login
        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] LoginDTO model)
        {
            //check if username is null
            if (string.IsNullOrEmpty(model.Username))
                return BadRequest("Username is required");
            //check if password is null
            if (string.IsNullOrEmpty(model.Password))
                return BadRequest("Password is required");
            var TokenVm = await _authService.Login(model).ConfigureAwait(false);
            if (TokenVm == null)
                return BadRequest("Invalid login attempt.<br/>Possible reasons:<br/>1. User not found, not approved, or not active.<br/>2. Password incorrect.<br/>3. Account lockedout.");
            return Ok(TokenVm);
        }


        [HttpPost("SellerLogin")]
        public async Task<IActionResult> SellerLogin([FromBody] LoginDTO model)
        {
            //check if username is null
            if (string.IsNullOrEmpty(model.Username))
                return BadRequest("Username is required");
            //check if password is null
            if (string.IsNullOrEmpty(model.Password))
                return BadRequest("Password is required");
            var TokenVm = await _authService.SellerLogin(model).ConfigureAwait(false);
            if (TokenVm == null)
                return BadRequest("Invalid login attempt.<br/>Possible reasons:<br/>1. User not found, not approved, or not active.<br/>2. Password incorrect.<br/>3. Account lockedout.");
            return Ok(TokenVm);


        }


        // GET: Auth/PrivilegeLogin
        [Authorize(Roles = "SuperAdmin")]
        [HttpGet("PrivilegeLogin/{Id}")]
        public async Task<IActionResult> PrivilegeLogin([FromRoute] string Id)
        {
            var TokenVm = await _authService.PrivilegeLogin(Id).ConfigureAwait(false);
            if (TokenVm == null)
                return BadRequest("Invalid login attempt.<br/>Possible reasons:<br/>1. User not found, not approved, or not active.<br/>2. Password incorrect.<br/>3. Account lockedout.");
            return Ok(TokenVm);
        }

        // GET: Auth/SellerandBuyerLogin
        [Authorize(Roles = "Seller,Buyer")]
        [HttpGet("PrivilegeLoginBuyer/{Id}/{role}")]

        public async Task<IActionResult> PrivilegeLoginBuyer([FromRoute] string Id, string role)
        {
            var TokenVm = await _authService.PrivilegeLoginBuyer(Id, role).ConfigureAwait(false);
            if (TokenVm == null)
                return BadRequest("Invalid login attempt.<br/>Possible reasons:<br/>1. User not found, not approved, or not active.<br/>2. Password incorrect.<br/>3. Account lockedout.");
            return Ok(TokenVm);
        }

        // GET: Auth/BuyerandSellerLogin
        [Authorize(Roles = "Seller,Buyer")]
        [HttpGet("PrivilegeLoginSeller/{Id}/{role}")]

        public async Task<IActionResult> PrivilegeLoginSeller([FromRoute] string Id, string role)
        {
            var TokenVm = await _authService.PrivilegeLoginSeller(Id, role).ConfigureAwait(false);
            if (TokenVm == null)
                return BadRequest("Invalid login attempt.<br/>Possible reasons:<br/>1. User not found, not approved, or not active.<br/>2. Password incorrect.<br/>3. Account lockedout.");
            return Ok(TokenVm);
        }



        // GET: Auth/SellerandBuyerLogin
        //[Authorize(Roles = "Seller,Buyer")]
        //[HttpGet("SellerandBuyerLogin/{Id}")]
        //public async Task<IActionResult> SellerandBuyerLogin([FromRoute] string Id)
        //{
        //    var TokenVm = await _authService.SellerandBuyerLogin(Id).ConfigureAwait(false);
        //    if (TokenVm == null)
        //        return BadRequest("Invalid login attempt.<br/>Possible reasons:<br/>1. User not found, not approved, or not active.<br/>2. Password incorrect.<br/>3. Account lockedout.");
        //    return Ok(TokenVm);
        //}


        //[Authorize(Roles = "SuperAdmin")]
        [HttpPost("Register")]
        public async Task<IActionResult> Register([FromBody] RegisterDTO model)
        {
            //check if username is null
            if (string.IsNullOrEmpty(model.Username))
                return BadRequest("Username is required");
            //check if password is null
            if (string.IsNullOrEmpty(model.Password))
                return BadRequest("Password is required");
            if (string.IsNullOrEmpty(model.Role))
                model.Role = _config["DefaultUserRole"];
            var TokenVm = await _authService.Register(model).ConfigureAwait(false);
            if (TokenVm == null)
                return BadRequest("Registration failed");
            return Ok(TokenVm);
        }
        [HttpGet("RefreshToken/{refreshtoken}")]
        public async Task<IActionResult> RefreshToken([FromRoute] string refreshtoken)
        {
            if (string.IsNullOrEmpty(refreshtoken))
                return BadRequest("Invalid Input");
            var TokenVm = await _authService.RefreshToken(refreshtoken).ConfigureAwait(false);
            if (TokenVm == null)
                return BadRequest("Token refresh failed");
            return Ok(TokenVm);
        }
        // POST: Auth/ForgotPassword
        [HttpPost("ForgotPassword")]
        public async Task<IActionResult> ForgotPassword([FromBody] string username)
        {
            //check if username is null
            if (string.IsNullOrEmpty(username))
                return BadRequest("Username is required");
            //generate password reset token
            var ForgotPasswordVm = await _authService.GeneratePasswordResetToken(username).ConfigureAwait(false);
            return Ok(ForgotPasswordVm);
        }
        // POST: Auth/ForgotUsername
        [HttpPost("ForgotUsername")]
        public async Task<IActionResult> ForgotUsername([FromBody] string email)
        {
            //check if email is null
            if (string.IsNullOrEmpty(email))
                return BadRequest("Email is required");
            //get Username by Email
            var username = await _authService.GetUsernameByEmail(email).ConfigureAwait(false);
            return Ok(username);
        }
        // POST: Auth/ResetPassword
        [HttpPost("ResetPassword")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordDTO model)
        {
            //check if username is null
            if (string.IsNullOrEmpty(model.UserId))
                return BadRequest("User Id is required");
            var result = await _authService.ResetPassword(model).ConfigureAwait(false);
            if (result)
                return Ok("Your password has been reset");
            return BadRequest("Password couldn't be reset");
        }
        // POST: Auth/ChangePassword
        [Authorize]
        [HttpPost("ChangePassword")]
        [AllowAnonymous]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordDTO model)
        {
            //check if Username is null
            if (string.IsNullOrEmpty(model.Username))
                return BadRequest("Username is required");
            //change the password
            var result = await _authService.ChangePassword(model).ConfigureAwait(false);
            if (result)
                return Ok("Your password has been changed");
            return BadRequest("Password couldn't be changed");
        }
        // POST: Auth/GetPasswordResetToken
        [Authorize(Roles = "SuperAdmin,TrainingAdmin,Admin")]
        [HttpPost("GetPasswordResetToken")]
        public async Task<IActionResult> GetPasswordResetToken([FromBody] string userid)
        {
            //check if userid is null
            if (string.IsNullOrEmpty(userid))
                return BadRequest("User Id is required");
            //generate password reset token
            var resetToken = await _authService.GetPasswordResetToken(userid).ConfigureAwait(false);
            return Ok(resetToken);
        }
        // POST: Auth/GetPasswordResetTokenByEmail
        [Authorize(Roles = "SuperAdmin,TrainingAdmin,Admin")]
        [HttpPost("GetPasswordResetTokenByEmail")]
        public async Task<IActionResult> GetPasswordResetTokenByEmail([FromBody] string email)
        {
            //check if email is null
            if (string.IsNullOrEmpty(email))
                return BadRequest("email is required");
            //generate password reset token by email
            var emailDto = await _authService.GetPasswordResetTokenByEmail(email).ConfigureAwait(false);
            return Ok(emailDto);
        }

        [Authorize]
        [HttpPut("UpdateBuyerId/{username}")]
        //[AllowAnonymous]

        public async Task<IActionResult> UpdateBuyerId([FromRoute] string username, [FromBody] CommonRegisterDTO inputModel)
        {
            if (inputModel == null) return BadRequest("Input not valid or null");
            if (username != inputModel.Emailid) return BadRequest("Invalid Id");
            if (!ModelState.IsValid) return BadRequest(ModelState.GetErrorMessages());

            var modelDto = await _authService.UpdateBuyerId(inputModel).ConfigureAwait(false);
            if (modelDto != 0) return Ok(modelDto);
            return BadRequest("Update failed");
        }

        [Authorize]
        [HttpPut("UpdateSellerId/{username}")]
        //[AllowAnonymous]

        public async Task<IActionResult> UpdateSellerId([FromRoute] string username, [FromBody] CommonRegisterDTO inputModel)
        {
            if (inputModel == null) return BadRequest("Input not valid or null");
            if (username != inputModel.Emailid) return BadRequest("Invalid Id");
            if (!ModelState.IsValid) return BadRequest(ModelState.GetErrorMessages());

            var modelDto = await _authService.UpdateSellerId(inputModel).ConfigureAwait(false);
            if (modelDto != 0) return Ok(modelDto);
            return BadRequest("Update failed");
        }
        [Authorize]
        [HttpPut("UpdateBuyerName/{username}")]
        //[AllowAnonymous]
        public async Task<IActionResult> UpdateBuyerName([FromRoute] string username, [FromBody] CommonRegisterDTO inputModel)
        {
            if (inputModel == null) return BadRequest("Input not valid or null");
            if (username != inputModel.Emailid) return BadRequest("Invalid Id");
            if (!ModelState.IsValid) return BadRequest(ModelState.GetErrorMessages());

            var modelDto = await _authService.UpdateBuyerName(inputModel).ConfigureAwait(false);
            if (modelDto != 0) return Ok(modelDto);
            return BadRequest("Update failed");
        }


        //[AllowAnonymous]
        //[HttpGet("ActiveDeactiveUser/{modelStr}")]
        //public async Task<IActionResult> ActiveDeactiveUser([FromRoute] string modelStr)
        //{
        //    var rowsChanged = 0;
        //    if (string.IsNullOrEmpty(modelStr)) return BadRequest("Input not valid or null");
        //    var idsStr = JsonConvert.DeserializeObject<ProductsDTO>(modelStr);
        //    rowsChanged = await _authService.ActiveDeactiveUser(idsStr.Id).ConfigureAwait(false);
        //    if (rowsChanged > 0) return Ok(rowsChanged);
        //    return BadRequest("Product(s) couldn't be updated");
        //}



    }
}
