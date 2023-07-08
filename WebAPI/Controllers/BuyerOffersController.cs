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
    public class BuyerOffersController : Controller
    {
        private readonly IDataService _dataService;
        public BuyerOffersController(IDataService dataService)
        {
            _dataService = dataService;
        }

        
        // GET Buyer Offer
        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var modelVms = await _dataService.BuyerOffers.Get().ConfigureAwait(false);
            if (modelVms == null || modelVms.Count <= 0) return NotFound("Buyers not found");
            return Ok(modelVms);
        }

        // GET Buyer Offer For Admin
        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> GetBuyerOffersForAdmin()
        {
            var modelVms = await _dataService.BuyerOffers.GetBuyerOffersForAdmin().ConfigureAwait(false);
            if (modelVms == null || modelVms.Count <= 0) return NotFound("Buyers not found");
            return Ok(modelVms);
        }
        // GET Buyers Offer/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Get([FromRoute] int id)
        {
            if (id <= 0) return BadRequest("Input not valid or null");
            var modelDto = await _dataService.BuyerOffers.Get(id).ConfigureAwait(false);
            if (modelDto == null) return NotFound("Buyers not found");
            return Ok(modelDto);
        }

        // POST: Buyer Offer/Create
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] BuyerOffersDTO inputModel)
        {


            if (inputModel == null) return BadRequest("Input not valid or null");
            if (!ModelState.IsValid) return BadRequest(ModelState.GetErrorMessages());
            var IsDuplicate = await _dataService.BuyerOffers.CheckDuplicate(inputModel).ConfigureAwait(false);
            if (IsDuplicate) return BadRequest("This record already exists");
            var modelDto = await _dataService.BuyerOffers.Create(inputModel).ConfigureAwait(false);
            if (modelDto != null) return Ok(modelDto);
            return BadRequest("Create failed");
        }

        // POST: Update Buyer Offer/Edit
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateBuyerofers([FromRoute] int id, [FromBody] BuyerOffersDTO inputModel)
        {
            if (inputModel == null) return BadRequest("Input not valid or null");
            if (id != inputModel.Id) return BadRequest("Invalid Id");
            if (!ModelState.IsValid) return BadRequest(ModelState.GetErrorMessages());

            var modelDto = await _dataService.BuyerOffers.UpdateBuyerofers(inputModel).ConfigureAwait(false);
            if (modelDto != null) return Ok(modelDto);
            return BadRequest("Update failed");
        }

    }
}
