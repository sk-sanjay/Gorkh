using Application.Dtos;
using Application.Extensions;
using Application.ServiceInterfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace WebAPI.Controllers
{
    // [Authorize]
    [ApiController]
    [Route("[controller]/[action]")]
    public class BuyerSellerRegistrationsController : Controller
    {
        private readonly IDataService _dataService;

        public BuyerSellerRegistrationsController(IDataService dataService)
        {
            _dataService = dataService;
        }
        // GET Seller
        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var modelVms = await _dataService.BuyerSellerRegistrations.Get().ConfigureAwait(false);
            if (modelVms == null || modelVms.Count <= 0) return NotFound("Buyers not found");
            return Ok(modelVms);
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> GetBuyersandSellers()
        {
            var modelVms = await _dataService.BuyerSellerRegistrations.GetBuyersandSellers().ConfigureAwait(false);
            if (modelVms == null || modelVms.Count <= 0) return NotFound("Products not found");
            return Ok(modelVms);
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> GetBuyersData()
        {
            var modelVms = await _dataService.BuyerSellerRegistrations.GetBuyersData().ConfigureAwait(false);
            if (modelVms == null || modelVms.Count <= 0) return NotFound("Buyers not found");
            return Ok(modelVms);
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> GetSellerData()
        {
            var modelVms = await _dataService.BuyerSellerRegistrations.GetSellerData().ConfigureAwait(false);
            if (modelVms == null || modelVms.Count <= 0) return NotFound("Seller not found");
            return Ok(modelVms);
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> GetBothData()
        {
            var modelVms = await _dataService.BuyerSellerRegistrations.GetBothData().ConfigureAwait(false);
            if (modelVms == null || modelVms.Count <= 0) return NotFound("Seller & Buyer not found");
            return Ok(modelVms);
        }



        // GET Seller/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Get([FromRoute] int id)
        {
            if (id <= 0) return BadRequest("Input not valid or null");
            var modelDto = await _dataService.BuyerSellerRegistrations.Get(id).ConfigureAwait(false);
            if (modelDto == null) return NotFound("Buyers not found");
            return Ok(modelDto);
        }

        // GET Seller/email
        [HttpGet("{email}")]
        public async Task<IActionResult> GetbyEmail([FromRoute] string email)
        {
            if (email == null) return BadRequest("Input not valid or null");
            var modelDto = await _dataService.BuyerSellerRegistrations.GetbyEmail(email).ConfigureAwait(false);
            if (modelDto == null) return NotFound("Buyers or Seller not found");
            return Ok(modelDto);
        }

        // GET check mobile

        [HttpGet("{mobile}")]
        public async Task<IActionResult> GetbyMobile([FromRoute] string mobile)
        {
            var userExists = await _dataService.BuyerSellerRegistrations.GetbyMobile(mobile).ConfigureAwait(false);
            return Ok(userExists);
        }


        // GET Seller/sellerid
        [HttpGet("{sellerid}")]
        public async Task<IActionResult> GetbySellerId([FromRoute] int sellerid)
        {
            if (sellerid == 0) return BadRequest("Input not valid or null");
            var modelDto = await _dataService.BuyerSellerRegistrations.GetbySellerId(sellerid).ConfigureAwait(false);
            if (modelDto == null) return NotFound("Seller not found");
            return Ok(modelDto);
        }





        // POST: Buyers/Create
        [HttpPost]

        public async Task<IActionResult> Create([FromBody] BuyerSellerRegistrationsDTO inputModel)
        {


            if (inputModel == null) return BadRequest("Input not valid or null");
            if (!ModelState.IsValid) return BadRequest(ModelState.GetErrorMessages());
            var IsDuplicate = await _dataService.BuyerSellerRegistrations.CheckDuplicate(inputModel).ConfigureAwait(false);
            if (IsDuplicate) return BadRequest("This record already exists");
            var modelDto = await _dataService.BuyerSellerRegistrations.Create(inputModel).ConfigureAwait(false);
            if (modelDto != null) return Ok(modelDto);
            return BadRequest("Create failed");
        }

        // POST: Buyers/Edit
        //[HttpPut("{id}")]
        //public async Task<IActionResult> Edit([FromRoute] int id, [FromBody] SubSubCategoriesDTO inputModel)
        //{
        //    if (inputModel == null) return BadRequest("Input not valid or null");
        //    if (id != inputModel.Id) return BadRequest("Invalid Id");
        //    if (!ModelState.IsValid) return BadRequest(ModelState.GetErrorMessages());

        //    var modelDto = await _dataService.SubSubCategories.Update(inputModel).ConfigureAwait(false);
        //    if (modelDto != null) return Ok(modelDto);
        //    return BadRequest("Update failed");
        //}

        // POST: Buyers/Edit
        [HttpPut("{id}")]
        public async Task<IActionResult> Edit([FromRoute] int id, [FromBody] BuyerCommonDTO inputModel)
        {
            if (inputModel == null) return BadRequest("Input not valid or null");
            if (id != inputModel.Id) return BadRequest("Invalid Id");
            if (!ModelState.IsValid) return BadRequest(ModelState.GetErrorMessages());

            var modelDto = await _dataService.BuyerSellerRegistrations.Update(inputModel).ConfigureAwait(false);
            if (modelDto != null) return Ok(modelDto);
            return BadRequest("Update failed");
        }
        // POST: Buyers/Edit
        [HttpPut("{id}")]
        public async Task<IActionResult> EditUpdateBuyerDetails([FromRoute] int id, [FromBody] BuyerSellerRegistrationsDTO inputModel)
        {
            if (inputModel == null) return BadRequest("Input not valid or null");
            if (id != inputModel.Id) return BadRequest("Invalid Id");
            if (!ModelState.IsValid) return BadRequest(ModelState.GetErrorMessages());

            var modelDto = await _dataService.BuyerSellerRegistrations.UpdateBuyerDetails(inputModel).ConfigureAwait(false);
            if (modelDto != null) return Ok(modelDto);
            return BadRequest("Update failed");
        }

        // POST: Seller/Edit
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateSellerProfile([FromRoute] int id, [FromBody] BuyerSellerRegistrationsDTO inputModel)
        {
            if (inputModel == null) return BadRequest("Input not valid or null");
            if (id != inputModel.Id) return BadRequest("Invalid Id");
            if (!ModelState.IsValid) return BadRequest(ModelState.GetErrorMessages());

            var modelDto = await _dataService.BuyerSellerRegistrations.UpdateSellerProfile(inputModel).ConfigureAwait(false);
            if (modelDto != null) return Ok(modelDto);
            return BadRequest("Update failed");
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateOrganisationType([FromRoute] int id, [FromBody] BuyerCommonDTO inputModel)
        {
            if (inputModel == null) return BadRequest("Input not valid or null");
            if (id != inputModel.Id) return BadRequest("Invalid Id");
            if (!ModelState.IsValid) return BadRequest(ModelState.GetErrorMessages());

            var modelDto = await _dataService.BuyerSellerRegistrations.UpdateOrganisationType(inputModel).ConfigureAwait(false);
            if (modelDto != 0) return Ok(modelDto);
            return BadRequest("Update failed");
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateDetails([FromRoute] int id, [FromBody] SellerCommonDTO inputModel)
        {
            if (inputModel == null) return BadRequest("Input not valid or null");
            if (id != inputModel.Id) return BadRequest("Invalid Id");
            if (!ModelState.IsValid) return BadRequest(ModelState.GetErrorMessages());

            var modelDto = await _dataService.BuyerSellerRegistrations.UpdateDetails(inputModel).ConfigureAwait(false);
            if (modelDto != 0) return Ok(modelDto);
            return BadRequest("Update failed");
        }




        // GET /buyerid
        [HttpGet("{buyerid}")]
        public async Task<IActionResult> GetbyBuyerId([FromRoute] int buyerid)
        {
            if (buyerid == 0) return BadRequest("Input not valid or null");
            var modelDto = await _dataService.BuyerSellerRegistrations.GetbyBuyerId(buyerid).ConfigureAwait(false);
            if (modelDto == null) return NotFound("Seller not found");
            return Ok(modelDto);
        }

        // GET /buyerid
        [HttpGet("{orgid}")]
        public async Task<IActionResult> GetdropdownbySellerId([FromRoute] int orgid)
        {
            if (orgid == 0) return BadRequest("Input not valid or null");
            var modelDto = await _dataService.BuyerSellerRegistrations.GetdropdownbySellerId(orgid).ConfigureAwait(false);
            if (modelDto == null) return NotFound("Seller not found");
            return Ok(modelDto);
        }












    }
}
