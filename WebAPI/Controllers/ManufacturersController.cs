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
    public class ManufacturersController : Controller
    {
        //public IActionResult Index()
        //{
        //    return View();
        //}
        private readonly IDataService _dataService;
        public ManufacturersController(IDataService dataService)
        {
            _dataService = dataService;
        }
        // GET Manufacturers
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var modelVms = await _dataService.Manufacturers.Get().ConfigureAwait(false);
            if (modelVms == null || modelVms.Count <= 0) return NotFound("Manufacturers not found");
            return Ok(modelVms);
        }
        // GET Manufacturers/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Get([FromRoute] int id)
        {
            if (id <= 0) return BadRequest("Input not valid or null");
            var modelDto = await _dataService.Manufacturers.Get(id).ConfigureAwait(false);
            if (modelDto == null) return NotFound("Manufacturers not found");
            return Ok(modelDto);
        }
        // POST: Manufacturers/Create
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] ManufacturersDTO inputModel)
        {
            if (inputModel == null) return BadRequest("Input not valid or null");
            if (!ModelState.IsValid) return BadRequest(ModelState.GetErrorMessages());
            var IsDuplicate = await _dataService.Manufacturers.CheckDuplicate(inputModel).ConfigureAwait(false);
            if (IsDuplicate) return BadRequest("This record already exists");
            var modelDto = await _dataService.Manufacturers.Create(inputModel).ConfigureAwait(false);
            if (modelDto != null) return Ok(modelDto);
            return BadRequest("Create failed");
        }
        // PUT: Manufacturers/Edit/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Edit([FromRoute] int id, [FromBody] ManufacturersDTO inputModel)
        {
            if (inputModel == null) return BadRequest("Input not valid or null");
            if (id != inputModel.Id) return BadRequest("Invalid Id");
            if (!ModelState.IsValid) return BadRequest(ModelState.GetErrorMessages());
            var IsDuplicate = await _dataService.Manufacturers.CheckDuplicate(inputModel).ConfigureAwait(false);
            if (IsDuplicate) return BadRequest("This record already exists");
            var modelDto = await _dataService.Manufacturers.Update(inputModel).ConfigureAwait(false);
            if (modelDto != null) return Ok(modelDto);
            return BadRequest("Update failed");
        }
        // DELETE: Manufacturers/Delete/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            if (id <= 0) return BadRequest("Input not valid or null");
            var rowsChanged = await _dataService.Manufacturers.Delete(id).ConfigureAwait(false);
            if (rowsChanged > 0) return Ok(rowsChanged);
            return BadRequest("Delete failed. There might be active child records.");
        }
        // GET GetDropdown
        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> GetDropdown()
        {
            var dropDownVms = await _dataService.Manufacturers.GetDropdown().ConfigureAwait(false);
            if (dropDownVms == null || dropDownVms.Count <= 0) return NotFound("Manufacturers not found");
            return Ok(dropDownVms);
        }
    }
}
