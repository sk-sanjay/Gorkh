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
    public class VisitorRegistrationsController : Controller
    {
        private readonly IDataService _dataService;
        public VisitorRegistrationsController(IDataService dataService)

        {
            _dataService = dataService;

        }
        // GET VisitorRegistrations
        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var modelVms = await _dataService.VisitorRegistrations.Get().ConfigureAwait(false);
            if (modelVms == null || modelVms.Count <= 0) return NotFound("Buyers not found");
            return Ok(modelVms);
        }

        // GET VisitorRegistrations/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Get([FromRoute] int id)
        {
            if (id <= 0) return BadRequest("Input not valid or null");
            var modelDto = await _dataService.VisitorRegistrations.Get(id).ConfigureAwait(false);
            if (modelDto == null) return NotFound("Buyers not found");
            return Ok(modelDto);
        }
        // GET Visitor/email
        [HttpGet("{email}")]
        public async Task<IActionResult> GetbyEmailId([FromRoute] string email)
        {
            if (email == null) return BadRequest("Input not valid or null");
            var modelDto = await _dataService.VisitorRegistrations.GetbyEmailId(email).ConfigureAwait(false);
            if (modelDto == null) return NotFound("Buyers or Seller not found");
            return Ok(modelDto);
        }


        // POST: VisitorRegistrations/Create
        [HttpPost]

        public async Task<IActionResult> Create([FromBody] VisitorRegistrationsDTO inputModel)
        {


            if (inputModel == null) return BadRequest("Input not valid or null");
            if (!ModelState.IsValid) return BadRequest(ModelState.GetErrorMessages());
            var IsDuplicate = await _dataService.VisitorRegistrations.CheckDuplicate(inputModel).ConfigureAwait(false);
            if (IsDuplicate) return BadRequest("This record already exists");
            var modelDto = await _dataService.VisitorRegistrations.Create(inputModel).ConfigureAwait(false);
            if (modelDto != null) return Ok(modelDto);
            return BadRequest("Create failed");
        }
    }
}
   