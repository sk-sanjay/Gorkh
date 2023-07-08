using Application.ServiceInterfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace WebAPI.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]/[action]")]
    public class OrganisationTypesController : Controller
    {
        private readonly IDataService _dataService;
        public OrganisationTypesController(IDataService dataService)
        {
            _dataService = dataService;

        }
        // GET OrganisationTypes
        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var modelVms = await _dataService.OrganisationTypes.Get().ConfigureAwait(false);
            if (modelVms == null || modelVms.Count <= 0) return NotFound("OrganisationTypes not found");
            return Ok(modelVms);
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> GetDropdown()
        {
            var dropDownVms = await _dataService.OrganisationTypes.GetDropdown().ConfigureAwait(false);
            if (dropDownVms == null || dropDownVms.Count <= 0) return NotFound("OrganisationTypes not found");
            return Ok(dropDownVms);
        }

    }
}
