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
    public class CitiesController : Controller
    {
        private readonly IDataService _dataService;
        public CitiesController(IDataService dataService)
        {
            _dataService = dataService;

        }
        // GET Cities
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var modelVms = await _dataService.Cities.Get().ConfigureAwait(false);
            if (modelVms == null || modelVms.Count <= 0) return NotFound("City Name not found");
            return Ok(modelVms);
        }

        // GET Cities/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Get([FromRoute] int id)
        {
            if (id <= 0) return BadRequest("Input not valid or null");
            var modelDto = await _dataService.Cities.Get(id).ConfigureAwait(false);
            if (modelDto == null) return NotFound("Category not found");
            return Ok(modelDto);
        }

        // POST: Cities/Create
        [HttpPost]

        public async Task<IActionResult> Create([FromBody] CitiesDTO inputModel)
        {
            if (inputModel == null) return BadRequest("Input not valid or null");
            if (!ModelState.IsValid) return BadRequest(ModelState.GetErrorMessages());
            var IsDuplicate = await _dataService.Cities.CheckDuplicate(inputModel).ConfigureAwait(false);
            if (IsDuplicate) return BadRequest("This record already exists");
            var modelDto = await _dataService.Cities.Create(inputModel).ConfigureAwait(false);
            if (modelDto != null) return Ok(modelDto);
            return BadRequest("Create failed");
        }
        // PUT: Cities/Edit/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Edit([FromRoute] int id, [FromBody] CitiesDTO inputModel)
        {
            if (inputModel == null) return BadRequest("Input not valid or null");
            if (id != inputModel.Id) return BadRequest("Invalid Id");
            if (!ModelState.IsValid) return BadRequest(ModelState.GetErrorMessages());
            var IsDuplicate = await _dataService.Cities.CheckDuplicate(inputModel).ConfigureAwait(false);
            if (IsDuplicate) return BadRequest("This record already exists");

            var modelDto = await _dataService.Cities.Update(inputModel).ConfigureAwait(false);
            if (modelDto != null) return Ok(modelDto);
            return BadRequest("Update failed");
        }
        // DELETE: Cities/Delete/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            if (id <= 0) return BadRequest("Input not valid or null");
            var rowsChanged = await _dataService.Cities.Delete(id).ConfigureAwait(false);
            if (rowsChanged > 0) return Ok(rowsChanged);
            return BadRequest("Delete failed. There might be active child records.");
        }
        [AllowAnonymous]
        // GET GetDropdown
        [HttpGet]
        public async Task<IActionResult> GetDropdown()
        {
            var dropDownVms = await _dataService.Countries.GetDropdown().ConfigureAwait(false);
            if (dropDownVms == null || dropDownVms.Count <= 0) return NotFound("Country not found");
            return Ok(dropDownVms);
        }

        [AllowAnonymous]
        [HttpGet("{sid}")]
        public async Task<IActionResult> GetCitybystate(int sid)
        {
            var dropDownVms = await _dataService.Cities.GetCitybystate(sid).ConfigureAwait(false);
            if (dropDownVms == null || dropDownVms.Count <= 0) return NotFound("Country not found");
            return Ok(dropDownVms);
        }


        //[AllowAnonymous]
        //[HttpGet("{maincat}")]
        //public async Task<IActionResult> GetSubcategory(int maincat)
        //{
        //    var dropDownVms = await _dataService.SubSubCategories.GetSubcategory(maincat).ConfigureAwait(false);
        //    if (dropDownVms == null || dropDownVms.Count <= 0) return NotFound("Sub-category not found");
        //    return Ok(dropDownVms);
        //}

    }
}

