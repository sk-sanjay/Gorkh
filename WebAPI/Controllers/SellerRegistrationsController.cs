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
    public class SellerRegistrationsController : Controller
    {
        private readonly IDataService _dataService;
        public SellerRegistrationsController(IDataService dataService)
        {
            _dataService = dataService;
        }
        // GET Seller
        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var modelVms = await _dataService.SellerRegistrations.Get().ConfigureAwait(false);
            if (modelVms == null || modelVms.Count <= 0) return NotFound("Buyers not found");
            return Ok(modelVms);
        }
        // GET Seller/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Get([FromRoute] int id)
        {
            if (id <= 0) return BadRequest("Input not valid or null");
            var modelDto = await _dataService.SellerRegistrations.Get(id).ConfigureAwait(false);
            if (modelDto == null) return NotFound("Buyers not found");
            return Ok(modelDto);
        }

        // GET Seller/email
        [HttpGet("{email}")]
        public async Task<IActionResult> GetbyEmail([FromRoute] string email)
        {
            if (email == null) return BadRequest("Input not valid or null");
            var modelDto = await _dataService.SellerRegistrations.GetbyEmail(email).ConfigureAwait(false);
            if (modelDto == null) return NotFound("Buyers not found");
            return Ok(modelDto);
        }

        // GET Seller/sellerid
        [HttpGet("{sellerid}")]
        public async Task<IActionResult> GetbySellerId([FromRoute] int sellerid)
        {
            if (sellerid == 0) return BadRequest("Input not valid or null");
            var modelDto = await _dataService.SellerRegistrations.GetbySellerId(sellerid).ConfigureAwait(false);
            if (modelDto == null) return NotFound("Seller not found");
            return Ok(modelDto);
        }





        // POST: Buyers/Create
        [HttpPost]

        public async Task<IActionResult> Create([FromBody] SellerRegistrationsDTO inputModel)
        {


            if (inputModel == null) return BadRequest("Input not valid or null");
            if (!ModelState.IsValid) return BadRequest(ModelState.GetErrorMessages());
            var modelDto = await _dataService.SellerRegistrations.Create(inputModel).ConfigureAwait(false);
            if (modelDto != null) return Ok(modelDto);
            return BadRequest("Create failed");
        }


        [HttpGet("{name}")]
        public async Task<IActionResult> CheckEmail([FromRoute] string name)
        {
            var userExists = await _dataService.SellerRegistrations.CheckEmail(name).ConfigureAwait(false);
            return Ok(userExists);
        }




    }
}
