using Application.ServiceInterfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
namespace WebAPI.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]/[action]")]
    public class ConditionsController : Controller
    {
        private readonly IDataService _dataService;
        public ConditionsController(IDataService dataService)
        {
            _dataService = dataService;
        }
        // GET GetDropdown
        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> GetDropdown()
        {
            var dropDownVms = await _dataService.Conditions.GetDropdown().ConfigureAwait(false);
            if (dropDownVms == null || dropDownVms.Count <= 0) return NotFound("Conditions not found");
            return Ok(dropDownVms);
        }
    }
}
