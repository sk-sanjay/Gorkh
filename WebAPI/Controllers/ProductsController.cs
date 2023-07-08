using Application.Dtos;
using Application.Extensions;
using Application.ServiceInterfaces;
using Application.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
namespace WebAPI.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class ProductsController : Controller
    {
        private readonly IDataService _dataService;
        private readonly IEmailService _emailService;
        public ProductsController(IDataService dataService, IEmailService emailService)
        {
            _dataService = dataService;
            _emailService = emailService;
        }
        // GET Products
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var modelVms = await _dataService.Products.Get().ConfigureAwait(false);
            if (modelVms == null || modelVms.Count <= 0) return NotFound("Products not found");
            return Ok(modelVms);
        }
        // GET Products/5
        [AllowAnonymous]
        //[HttpGet("{id}")]
        [HttpGet("Get/{id}")]
        public async Task<IActionResult> Get([FromRoute] int id)
        {
            if (id <= 0) return BadRequest("Input not valid or null");
            var modelDto = await _dataService.Products.Get(id).ConfigureAwait(false);
            if (modelDto == null) return NotFound("Products not found");
            return Ok(modelDto);
        }
        // POST: Products/Create
        //[HttpPost]
        // POST: SpecificationsSSCategories/CreateRange
        [AllowAnonymous]
        [HttpPost("Create")]
        public async Task<IActionResult> Create([FromBody] ProductsDTO inputModel)
        {
            if (inputModel == null) return BadRequest("Input not valid or null");
            if (!ModelState.IsValid) return BadRequest(ModelState.GetErrorMessages());
            var modelDto = await _dataService.Products.Create(inputModel).ConfigureAwait(false);
            if (modelDto != null) return Ok(modelDto);
            return BadRequest("Create failed");
        }
        // PUT: Products/Edit/5
        //[HttpPut("{id}")]
        [AllowAnonymous]
        [HttpPut("Edit/{id}")]
        public async Task<IActionResult> Edit([FromRoute] int id, [FromBody] ProductsDTO inputModel)
        {
            if (inputModel == null) return BadRequest("Input not valid or null");
            if (id != inputModel.Id) return BadRequest("Invalid Id");
            if (!ModelState.IsValid) return BadRequest(ModelState.GetErrorMessages());

            var modelDto = await _dataService.Products.Update(inputModel).ConfigureAwait(false);
            if (modelDto != null) return Ok(modelDto);
            return BadRequest("Update failed");
        }
        // DELETE: Products/Delete/5
        [AllowAnonymous]
        [HttpDelete("Delete/{id}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            if (id <= 0) return BadRequest("Input not valid or null");
            var rowsChanged = await _dataService.Products.Delete(id).ConfigureAwait(false);
            if (rowsChanged > 0) return Ok(rowsChanged);
            return BadRequest("Delete failed. There might be active child records.");
        }
        [AllowAnonymous]
        //[HttpGet("{subsubcategoryid}")]
        [HttpPost("FinalSubmit")]
        public async Task<IActionResult> PostFinalSubmit([FromBody] CommonDTO argModelDto)
        {
            if (argModelDto == null) return BadRequest("Input not valid or null");
            var rowsAffected = await _dataService.Products.FinalSubmitflagupdate(argModelDto).ConfigureAwait(false);
            return Ok(rowsAffected);
        }
        [AllowAnonymous]
        //[HttpGet("{subsubcategoryid}")]
        [HttpGet("GetProductsBySeller/{sellerid}")]
        public async Task<IActionResult> GetProductsBySeller([FromRoute] int sellerid)
        {
            var dropDownVms = await _dataService.Products.GetProductsBySeller(sellerid).ConfigureAwait(false);
            if (dropDownVms == null || dropDownVms.Count <= 0) return NotFound("Products not found");
            return Ok(dropDownVms);
        }

        [AllowAnonymous]
        //[HttpGet("{subsubcategoryid}")]
        [HttpGet("GetProductVisitor/{productid}")]
        public async Task<IActionResult> GetProductVisitor([FromRoute] int productid)
        {
            var dropDownVms = await _dataService.Products.GetProductVisitor(productid).ConfigureAwait(false);
            if (dropDownVms == null || dropDownVms.Count <= 0) return NotFound("Products not found");
            return Ok(dropDownVms);
        }

        //get all products for admin
        [AllowAnonymous]
        [HttpGet("GetAllProductsForAdmin")]
        public async Task<IActionResult> GetAllProductsForAdmin()
        {
            var modelVms = await _dataService.Products.GetAllProductsForAdmin().ConfigureAwait(false);
            if (modelVms == null || modelVms.Count <= 0) return NotFound("Products not found");
            return Ok(modelVms);
        }
        //Get all Pending products for admin
        [AllowAnonymous]
        [HttpGet("GetAllPendingProducts")]
        public async Task<IActionResult> GetAllPendingProducts()
        {
            var modelVms = await _dataService.Products.GetAllPendingProducts().ConfigureAwait(false);
            if (modelVms == null || modelVms.Count <= 0) return NotFound("Products not found");
            return Ok(modelVms);
        }

      
        [AllowAnonymous]
        [HttpGet("UpdateSelected/{modelStr}")]
        public async Task<IActionResult> UpdateSelected([FromRoute] string modelStr)
        {
            var rowsChanged = 0;
            if (string.IsNullOrEmpty(modelStr)) return BadRequest("Input not valid or null");
            var idsStr = JsonConvert.DeserializeObject<ProductsDTO>(modelStr);
            if (idsStr.IsApprove == false)
            {
                var modelproducts = await _dataService.Products.GettProductsDetailsById(idsStr.Id).ConfigureAwait(false); ;
                var modelsellerinfo = await _dataService.BuyerSellerRegistrations.Get(modelproducts.SellerId).ConfigureAwait(false);
                //send mail to seller on Product Photos Uploaded
                var body = string.Format("<html>" + Environment.NewLine +
                                         "<body>" + Environment.NewLine +
                                         "<p>Dear Seller,</p>" + Environment.NewLine +
                                         "<p>Congratulations! You have successfully uploaded your <b>" + modelproducts.SubSubCategoriesName + "</b>, Photo & specifications & your product code is <b>" + modelproducts.ProductNumber + "</b>.</p>" + Environment.NewLine +
                                         "<p>In case of any query, you may write to support@surplusplatform.com or call us on 8287344537.</p>" + Environment.NewLine +
                                         "<p>Thanks</p>" + Environment.NewLine +
                                         "<p>Team Surplus Platform </p>" + Environment.NewLine +
                                         "</body>" + Environment.NewLine +
                                         "</html>");
                var EmailVm = new EmailVM
                {
                    ToAddresses = new List<string> { modelsellerinfo.Email },
                    Subject = "Account credentials",
                    Body = body
                };
                await _emailService.SendEmailAsync(modelsellerinfo.Email, "Product Photos Uploaded", body, null, null, null, null).ConfigureAwait(false);

            }
            
            //foreach (var item in idsStr)
            //{
            //    var id = Convert.ToInt32(item[0]);
            //    //rowsChanged += await _dataService.NotificationDetails.Delete(id).ConfigureAwait(false);
            //    rowsChanged += await _dataService.Products.UpdateSelected(id).ConfigureAwait(false);
            //}
            rowsChanged = await _dataService.Products.UpdateSelected(idsStr.Id).ConfigureAwait(false);
            if (rowsChanged > 0) return Ok(rowsChanged);
               return BadRequest("Product(s) couldn't be updated");

        }

        [AllowAnonymous]
        [HttpGet("UpdateasFeatured/{modelStr}")]
        public async Task<IActionResult> UpdateasFeatured([FromRoute] string modelStr)
        {
            var rowsChanged = 0;
            if (string.IsNullOrEmpty(modelStr)) return BadRequest("Input not valid or null");
            var idsStr = JsonConvert.DeserializeObject<ProductsDTO>(modelStr);
            rowsChanged = await _dataService.Products.UpdateasFeatured(idsStr.Id).ConfigureAwait(false);
            if (rowsChanged > 0) return Ok(rowsChanged);
            return BadRequest("Product(s) couldn't be updated");
        }




        //get all products by sub category id
        [AllowAnonymous]
        [HttpGet("GettProductsBySubCatId/{subcategoryid}")]
        public async Task<IActionResult> GettProductsBySubCatId(int subcategoryid)
        {
            var modelVms = await _dataService.Products.GettProductsBySubCatId(subcategoryid).ConfigureAwait(false);
            if (modelVms == null || modelVms.Count <= 0) return NotFound("Products not found");
            return Ok(modelVms);
        }

        [AllowAnonymous]
        [HttpGet("GettProductsBySubCatId1/{catid}/{subcategoryid}/{countryId}/{stateId}/{saleType}/{conditionId}/{keyword}")]
        public async Task<IActionResult> GettProductsBySubCatId1(int catid, int subcategoryid, int countryId, int stateId, string saleType, int conditionId, string keyword)
        {
            var modelVms = await _dataService.Products1.GettProductsBySubCatId1(catid, subcategoryid, countryId, stateId, saleType, conditionId, keyword).ConfigureAwait(false);
            if (modelVms == null || modelVms.Count <= 0) return NotFound("Products not found");
            return Ok(modelVms);
        }

        [AllowAnonymous]
        [HttpGet("GetProductsasfeaturedmachine")]
        public async Task<IActionResult> GetProductsasfeaturedmachine()
        {
            var modelVms = await _dataService.Products1.GetProductsasfeaturedmachine().ConfigureAwait(false);
            if (modelVms == null || modelVms.Count <= 0) return NotFound("Products not found");
            return Ok(modelVms);
        }

        [AllowAnonymous]
        [HttpGet("GetLatestProducts")]
        public async Task<IActionResult> GetLatestProducts()
        {
            var modelVms = await _dataService.Products1.GetLatestProducts().ConfigureAwait(false);
            if (modelVms == null || modelVms.Count <= 0) return NotFound("Products not found");
            return Ok(modelVms);
        }

        [AllowAnonymous]
        [HttpGet("GetCategoryImage")]
        public async Task<IActionResult> GetCategoryImage()
        {
            var modelVms = await _dataService.Products1.GetCategoryImage().ConfigureAwait(false);
            if (modelVms == null || modelVms.Count <= 0) return NotFound("Products not found");
            return Ok(modelVms);
        }




        [AllowAnonymous]
        [HttpGet("GettProductsBySubCatId2/{subcategoryid}")]
        public async Task<IActionResult> GettProductsBySubCatId2(int subcategoryid)
        {
            var modelVms = await _dataService.Products.GettProductsBySubCatId2(subcategoryid).ConfigureAwait(false);
            if (modelVms == null || modelVms.Count <= 0) return NotFound("Products not found");
            return Ok(modelVms);
        }
        //get products details by id
        [AllowAnonymous]
        [HttpGet("GettProductsDetailsById/{id}")]
        public async Task<IActionResult> GettProductsDetailsById(int id)
        {
            var modelVms = await _dataService.Products.GettProductsDetailsById(id).ConfigureAwait(false);
            if (modelVms == null) return NotFound("Products not found");
            return Ok(modelVms);
        }
        //get products details by id
        [AllowAnonymous]
        [HttpGet("GettProductsDetailsByIdwebsite/{id}")]
        public async Task<IActionResult> GettProductsDetailsByIdwebsite(int id)
        {
            var modelVms = await _dataService.Products.GettProductsDetailsByIdwebsite(id).ConfigureAwait(false);
            if (modelVms == null) return NotFound("Products not found");
            return Ok(modelVms);
        }

        //get products details by id for view product in admin
        [AllowAnonymous]
        [HttpGet("GetProductsDetailsById/{id}")]
        public async Task<IActionResult> GetProductsDetailsById(int id)
        {
            var modelVms = await _dataService.Products.GettProductsDetailsById(id).ConfigureAwait(false);
            if (modelVms == null) return NotFound("Products not found");
            return Ok(modelVms);
        }

        [AllowAnonymous]
        [HttpGet("GetProductsSellerDetailsById/{id}")]
        public async Task<IActionResult> GetProductsSellerDetailsById(int id)
        {
            var modelVms = await _dataService.Products.GetProductsSellerDetailsById(id).ConfigureAwait(false);
            if (modelVms == null) return NotFound("Products not found");
            return Ok(modelVms);
        }
        [AllowAnonymous]
        [HttpGet("GetProductsBreadCrumbs/{catid}/{subcatid}")]
        public async Task<IActionResult> GetProductsBreadCrumbs(int catid, int subcatid)
        {
            var modelVms = await _dataService.Products.GetProductsBreadCrumbs(catid, subcatid).ConfigureAwait(false);
            if (modelVms == null || modelVms.Count <= 0) return NotFound("Products not found");
            return Ok(modelVms);
        }
        //Get Related Products by sub-sub category id
        [AllowAnonymous]
        [HttpGet("GetRelatedProducts/{subsubcatid}")]
        public async Task<IActionResult> GetRelatedProducts(int subsubcatid)
        {
            var modelVms = await _dataService.Products.GetRelatedProducts(subsubcatid).ConfigureAwait(false);
            if (modelVms == null || modelVms.Count <= 0) return NotFound("Products not found");
            return Ok(modelVms);
        }
    }
}
