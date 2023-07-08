using Application.Dtos;
using Application.Extensions;
using Application.ServiceInterfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace WebAPI.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]/[action]")]
    public class OurCustomersController : Controller
    {
        private readonly IDataService _dataService;
        public OurCustomersController(IDataService dataService)
        {
            _dataService = dataService;
        }
        // GET OurCustomers
        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var modelVms = await _dataService.OurCustomers.Get().ConfigureAwait(false);
            if (modelVms == null || modelVms.Count <= 0) return NotFound("OurCustomers not found");
            return Ok(modelVms);
        }
        // GET OurCustomers/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Get([FromRoute] int id)
        {
            if (id <= 0) return BadRequest("Input not valid or null");
            var modelDto = await _dataService.OurCustomers.Get(id).ConfigureAwait(false);
            if (modelDto == null) return NotFound("OurCustomers not found");
            return Ok(modelDto);
        }
        // POST: OurCustomers/Create
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] OurCustomersDTO inputModel)
        {
            if (inputModel == null) return BadRequest("Input not valid or null");
            if (!ModelState.IsValid) return BadRequest(ModelState.GetErrorMessages());
            var modelDto = await _dataService.OurCustomers.Create(inputModel).ConfigureAwait(false);
            if (modelDto != null) return Ok(modelDto);
            return BadRequest("Create failed");
        }
        // PUT: OurCustomers/Edit/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Edit([FromRoute] int id, [FromBody] OurCustomersDTO inputModel)
        {
            if (inputModel == null) return BadRequest("Input not valid or null");
            if (id != inputModel.Id) return BadRequest("Invalid Id");
            if (!ModelState.IsValid) return BadRequest(ModelState.GetErrorMessages());

            var modelDto = await _dataService.OurCustomers.Update(inputModel).ConfigureAwait(false);
            if (modelDto != null) return Ok(modelDto);
            return BadRequest("Update failed");
        }
        // DELETE: OurCustomers/Delete/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            if (id <= 0) return BadRequest("Input not valid or null");
            var rowsChanged = await _dataService.OurCustomers.Delete(id).ConfigureAwait(false);
            if (rowsChanged > 0) return Ok(rowsChanged);
            return BadRequest("Delete failed. There might be active child records.");
        }
        //Get OurCustomers for Home page
        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> GetOurCustomersHomePage()
        {
            var dropDownVms = await _dataService.OurCustomers.GetOurCustomersHomePage().ConfigureAwait(false);
            if (dropDownVms == null || dropDownVms.Count <= 0) return NotFound("OurCustomers not found");
            return Ok(dropDownVms);
        }
    }
}
