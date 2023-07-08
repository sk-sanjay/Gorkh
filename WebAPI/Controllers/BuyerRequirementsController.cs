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
    public class BuyerRequirementsController : Controller
    {
        private readonly IDataService _dataService;
        public BuyerRequirementsController(IDataService dataService)

        {
            _dataService = dataService;

        }
        // GET BuyerRequirements
        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var modelVms = await _dataService.BuyerRequirements.Get().ConfigureAwait(false);
            if (modelVms == null || modelVms.Count <= 0) return NotFound("Buyers not found");
            return Ok(modelVms);
        }
        // GET BuyerRequirements
        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> GetBuyerRequirements()
        {
            var modelVms = await _dataService.BuyerRequirements.GetBuyerRequirements().ConfigureAwait(false);
            if (modelVms == null || modelVms.Count <= 0) return NotFound("Buyers not found");
            return Ok(modelVms);
        }
        // GET BuyerRequirements
        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> GetBuyerRequirementsforWebsite()
        {
            var modelVms = await _dataService.BuyerRequirements.GetBuyerRequirementsforWebsite().ConfigureAwait(false);
            if (modelVms == null || modelVms.Count <= 0) return NotFound("Buyers not found");
            return Ok(modelVms);
        }

        
        // GET BuyerRequirements by id
        [HttpGet("{id}")]
        public async Task<IActionResult> GetBuyerRequirements(int id)
        {
            if (id <= 0) return BadRequest("Input not valid or null");
            var modelVms = await _dataService.BuyerRequirements.GetBuyerRequirements(id).ConfigureAwait(false);
            if (modelVms == null) return NotFound("Buyers not found");
            return Ok(modelVms);
        }

        // GET BuyerRequirements by email
        [HttpGet("{email}")]
        public async Task<IActionResult> GetBuyerRequirementsbyusername(string email)
        {
            if (email == null) return BadRequest("Input not valid or null");
            var modelVms = await _dataService.BuyerRequirements.GetBuyerRequirementsbyusername(email).ConfigureAwait(false);
            if (modelVms == null) return NotFound("Buyers not found");
            return Ok(modelVms);
        }


        // GET BuyerRequirements/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Get([FromRoute] int id)
        {
            if (id <= 0) return BadRequest("Input not valid or null");
            var modelDto = await _dataService.BuyerRequirements.Get(id).ConfigureAwait(false);
            if (modelDto == null) return NotFound("Buyers not found");
            return Ok(modelDto);
        }

        // POST: BuyerRequirements/Create
        [HttpPost]

        public async Task<IActionResult> Create([FromBody] BuyerRequirementsDTO inputModel)
        {


            if (inputModel == null) return BadRequest("Input not valid or null");
            if (!ModelState.IsValid) return BadRequest(ModelState.GetErrorMessages());
            var modelDto = await _dataService.BuyerRequirements.Create(inputModel).ConfigureAwait(false);
            if (modelDto != null) return Ok(modelDto);
            return BadRequest("Create failed");
        }







    }
}
