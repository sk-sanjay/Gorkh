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
    public class BuyersController : Controller
    {
        private readonly IDataService _dataService;
        public BuyersController(IDataService dataService)
        {
            _dataService = dataService;
        }
        // GET Buyers
        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var modelVms = await _dataService.Buyers.Get().ConfigureAwait(false);
            if (modelVms == null || modelVms.Count <= 0) return NotFound("Buyers not found");
            return Ok(modelVms);
        }
        // GET Buyers/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Get([FromRoute] int id)
        {
            if (id <= 0) return BadRequest("Input not valid or null");
            var modelDto = await _dataService.Buyers.Get(id).ConfigureAwait(false);
            if (modelDto == null) return NotFound("Buyers not found");
            return Ok(modelDto);
        }

        // POST: Buyers/Create
        [HttpPost]

        public async Task<IActionResult> Create([FromBody] BuyersDTO inputModel)
        {


            if (inputModel == null) return BadRequest("Input not valid or null");
            if (!ModelState.IsValid) return BadRequest(ModelState.GetErrorMessages());
            var modelDto = await _dataService.Buyers.Create(inputModel).ConfigureAwait(false);
            if (modelDto != null) return Ok(modelDto);
            return BadRequest("Create failed");
        }

        [HttpGet("{name}")]
        public async Task<IActionResult> CheckEmail([FromRoute] string name)
        {
            var userExists = await _dataService.Buyers.CheckEmail(name).ConfigureAwait(false);
            return Ok(userExists);
        }

        // GET /buyerid
        [HttpGet("{buyerid}")]
        public async Task<IActionResult> GetbyBuyerId([FromRoute] int buyerid)
        {
            if (buyerid == 0) return BadRequest("Input not valid or null");
            var modelDto = await _dataService.Buyers.GetbyBuyerId(buyerid).ConfigureAwait(false);
            if (modelDto == null) return NotFound("Seller not found");
            return Ok(modelDto);
        }

    }
}
