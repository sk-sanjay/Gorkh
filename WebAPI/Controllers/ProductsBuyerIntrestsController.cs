using Application.Dtos;
using Application.Extensions;
using Application.ServiceInterfaces;
using Application.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebAPI.Helpers;
namespace WebAPI.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]/[action]")]
    public class ProductsBuyerIntrestsController : Controller
    {
        private readonly IDataService _dataService;
        private readonly IEmailService _emailservice;
        public ProductsBuyerIntrestsController(IDataService dataService, IEmailService emailService)
        {
            _dataService = dataService;
            _emailservice = emailService;
        }

        public string usename => DataHelper.GetUserName(User);

        // GET ProductsBuyerIntrests
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var modelVms = await _dataService.ProductsBuyerIntrests.Get().ConfigureAwait(false);
            if (modelVms == null || modelVms.Count <= 0) return NotFound("Buyer Intrests not found");
            return Ok(modelVms);
        }
        // GET ProductsBuyerIntrests/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Get([FromRoute] int id)
        {
            if (id <= 0) return BadRequest("Input not valid or null");
            var modelDto = await _dataService.ProductsBuyerIntrests.Get(id).ConfigureAwait(false);
            if (modelDto == null) return NotFound("Buyer Intrests not found");
            return Ok(modelDto);
        }
        // POST: ProductsBuyerIntrests/Create
        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] ProductsBuyerIntrestsDTO inputModel)
        {
            if (inputModel == null) return BadRequest("Input not valid or null");
            if (!ModelState.IsValid) return BadRequest(ModelState.GetErrorMessages());
            var IsDuplicate = await _dataService.ProductsBuyerIntrests.CheckDuplicate(inputModel).ConfigureAwait(false);
            if (IsDuplicate) return BadRequest("This record already exists");
            var modelDto = await _dataService.ProductsBuyerIntrests.Create(inputModel).ConfigureAwait(false);
            if (modelDto != null)
            {
                var modelDtoforbuyer = await _dataService.BuyerSellerRegistrations.Get(modelDto.BuyerId).ConfigureAwait(false);
                var modelDtoforproductNumber = await _dataService.Products.Get(modelDto.ProductId).ConfigureAwait(false);
                var modelDtoforproductName = await _dataService.SubSubCategories.Get(modelDtoforproductNumber.SubSubCatId).ConfigureAwait(false);
                var modelDtoforSeller = await _dataService.BuyerSellerRegistrations.Get(modelDtoforproductNumber.SellerId).ConfigureAwait(false);
                //send mail to buyer
                string strBody = "<html><body>";
                strBody = strBody + "<p>Dear Buyer,</p>";
                strBody = strBody + "<p>Greetings</p>";
                strBody = strBody + "<p>Thanks indeed for your interest in Product Name - <b>" + modelDtoforproductName.SubSubCategoriesName+ "</b> and Code Number - <b>" + modelDtoforproductNumber.ProductNumber+ "</b>.</p>";
                strBody = strBody + "<p>To start the ‘buy’ process, you are requested to pay our service charges as 2% of above Reserved Price to surplusplatform.com.</p>";
                strBody = strBody + "<p>For details, please read <b>Service Charges</b> at bottom of our website.</p>";
                strBody = strBody + "<p>In case of any query, you may write to support@surplusplatform.com or call us on 8287344537.</p>";
                strBody = strBody + "<p><b>Team Surplus Platform</b></p>";

                var EmailVm = new EmailVM
                {
                    ToAddresses = new List<string> { usename },
                    Subject = "Buyer Interest",
                    Body = strBody
                };
                await _emailservice.SendEmailAsync(usename, "Buyer Interest", strBody, null, null, null, null).ConfigureAwait(false);

                //send mail to Seller
                string strBodySeller = "<html><body>";
                strBodySeller = strBodySeller + "<p>Dear Seller,</p>";
                strBodySeller = strBodySeller + "<p>New buyer  interest has been  received successfully!</p>";
                strBodySeller = strBodySeller + "<p>Product Code : " + modelDtoforproductNumber.ProductNumber + "</p>";
                strBodySeller = strBodySeller + "<p>Reserved Price : " + modelDtoforproductNumber.EstimatePrice + "</p>";
                strBodySeller = strBodySeller + "<p>To start the ‘Sale’ process, you are requested to pay our service charges as 2% of above Reserved price to surplusplatform.com.</p>";
                strBodySeller = strBodySeller + "<p>For details, please read <b>Service Charges</b> at bottom of our website</p>";
                strBodySeller = strBodySeller + "<p>In case of any query, you may write to support @surplusplatform.com or call us on 8287344537.</p>";
                strBodySeller = strBodySeller + "<p><b>Team Surplus Platform</b></p>";

                var EmailVmSeller = new EmailVM
                {
                    ToAddresses = new List<string> { modelDtoforSeller.Email },
                    Subject = "Buyer Interest",
                    Body = strBodySeller
                };
                await _emailservice.SendEmailAsync(modelDtoforSeller.Email, "Buyer Interest", strBodySeller, null, null, null, null).ConfigureAwait(false);

                //send mail to Admin

                string strBody1 = "<html><body>";
                strBody1 = strBody1 + "<p>Dear Admin,</p>";
                strBody1 = strBody1 + "<p>New buyer  interest has been  received successfully!</p>";
                strBody1 = strBody1 + "<p>Product Code : " + modelDtoforproductNumber.ProductNumber + "</p>";
                strBody1 = strBody1 + "<p>Reserved Price : " + modelDtoforproductNumber.EstimatePrice + "</p>";
                strBody1 = strBody1 + "<p>Buyer Name : Mr. " + modelDtoforbuyer.FirstName + " "+ modelDtoforbuyer.LastName + "</p>";
                strBody = strBody1 + "<p>Mobile No.: " + modelDtoforbuyer.Mobile + " </p>";
                strBody = strBody1 + "<p>Email : " + modelDtoforbuyer.Email + " </p>";

                var EmailVm1 = new EmailVM
                {
                    ToAddresses = new List<string> {"support@surplusplatform.com" },
                    Subject = "Buyer Interest",
                    Body = strBody1
                };
                await _emailservice.SendEmailAsync("support@surplusplatform.com", "Buyer Interest", strBody1, null, null, null, null).ConfigureAwait(false);

            }
                return Ok(modelDto);
            
            
        }
        // PUT: ProductsBuyerIntrests/Edit/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Edit([FromRoute] int id, [FromBody] ProductsBuyerIntrestsDTO inputModel)
        {
            if (inputModel == null) return BadRequest("Input not valid or null");
            if (id != inputModel.Id) return BadRequest("Invalid Id");
            if (!ModelState.IsValid) return BadRequest(ModelState.GetErrorMessages());
            var modelDto = await _dataService.ProductsBuyerIntrests.Update(inputModel).ConfigureAwait(false);
            if (modelDto != null) return Ok(modelDto);
            return BadRequest("Update failed");
        }
        // DELETE: ProductsBuyerIntrests/Delete/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            if (id <= 0) return BadRequest("Input not valid or null");
            var rowsChanged = await _dataService.ProductsBuyerIntrests.Delete(id).ConfigureAwait(false);
            if (rowsChanged > 0) return Ok(rowsChanged);
            return BadRequest("Delete failed. There might be active child records.");
        }
        [AllowAnonymous]
        [HttpGet("{buyerid}")]
        public async Task<IActionResult> GetProductsBuyerIntrestsByBuyer([FromRoute] int buyerid)
        {
            var modelVms = await _dataService.ProductsBuyerIntrests.GetProductsBuyerIntrestsByBuyer(buyerid).ConfigureAwait(false);
            if (modelVms == null || modelVms.Count <= 0) return NotFound("Products Interest not found");
            return Ok(modelVms);
        }
        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> GetProductsBuyerIntrestsForAdmin()
        {
            var modelVms = await _dataService.ProductsBuyerIntrests.GetProductsBuyerIntrestsForAdmin().ConfigureAwait(false);
            if (modelVms == null || modelVms.Count <= 0) return NotFound("Products Interest not found");
            return Ok(modelVms);
        }
        // GET ProductsBuyerIntrests/5/5
        [HttpGet("{buyerid}/{productid}")]
        public async Task<IActionResult> GetProductsBuyerIntrestsByBuyerandPid([FromRoute] int buyerid, int productid)
        {
            if (productid <= 0 && buyerid <= 0) return BadRequest("Input not valid or null");
            var modelDto = await _dataService.ProductsBuyerIntrests.GetProductsBuyerIntrestsByBuyerandPid(buyerid, productid).ConfigureAwait(false);
            if (modelDto == null) return NotFound("Buyer Intrests not found");
            return Ok(modelDto);
        }
    }
}
