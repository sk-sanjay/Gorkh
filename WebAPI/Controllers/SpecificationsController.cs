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
    public class SpecificationsController : Controller
    {
        private readonly IDataService _dataService;
        public SpecificationsController(IDataService dataService)
        {
            _dataService = dataService;
        }
        // GET Specifications
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var modelVms = await _dataService.Specifications.Get().ConfigureAwait(false);
            if (modelVms == null || modelVms.Count <= 0) return NotFound("Specifications not found");
            return Ok(modelVms);
        }
        // GET Specifications/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Get([FromRoute] int id)
        {
            if (id <= 0) return BadRequest("Input not valid or null");
            var modelDto = await _dataService.Specifications.Get(id).ConfigureAwait(false);
            if (modelDto == null) return NotFound("Specifications not found");
            return Ok(modelDto);
        }
        // POST: Specifications/Create
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] SpecificationsDTO inputModel)
        {
            if (inputModel == null) return BadRequest("Input not valid or null");
            if (!ModelState.IsValid) return BadRequest(ModelState.GetErrorMessages());
            var IsDuplicate = await _dataService.Specifications.CheckDuplicate(inputModel).ConfigureAwait(false);
            if (IsDuplicate) return BadRequest("This record already exists");
            var modelDto = await _dataService.Specifications.Create(inputModel).ConfigureAwait(false);
            if (modelDto != null) return Ok(modelDto);
            return BadRequest("Create failed");
        }
        // PUT: Specifications/Edit/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Edit([FromRoute] int id, [FromBody] SpecificationsDTO inputModel)
        {
            if (inputModel == null) return BadRequest("Input not valid or null");
            if (id != inputModel.Id) return BadRequest("Invalid Id");
            if (!ModelState.IsValid) return BadRequest(ModelState.GetErrorMessages());
            var IsDuplicate = await _dataService.Specifications.CheckDuplicate(inputModel).ConfigureAwait(false);
            if (IsDuplicate) return BadRequest("This record already exists");
            var modelDto = await _dataService.Specifications.Update(inputModel).ConfigureAwait(false);
            if (modelDto != null) return Ok(modelDto);
            return BadRequest("Update failed");
        }
        // DELETE: Specifications/Delete/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            if (id <= 0) return BadRequest("Input not valid or null");
            var rowsChanged = await _dataService.Specifications.Delete(id).ConfigureAwait(false);
            if (rowsChanged > 0) return Ok(rowsChanged);
            return BadRequest("Delete failed. There might be active child records.");
        }
        // GET Specification order by
        [HttpGet]
        public async Task<IActionResult> GetByOrder()
        {
            var modelVms = await _dataService.Specifications.GetByOrder().ConfigureAwait(false);
            if (modelVms == null || modelVms.Count <= 0) return NotFound("Specifications not found");
            return Ok(modelVms);
        }
    }
}
