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
    public class ProductsLocationsController : Controller
    {
        private readonly IDataService _dataService;
        public ProductsLocationsController(IDataService dataService)
        {
            _dataService = dataService;
        }
        // GET ProductsLocations
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var modelVms = await _dataService.ProductsLocations.Get().ConfigureAwait(false);
            if (modelVms == null || modelVms.Count <= 0) return NotFound("Products Locations not found");
            return Ok(modelVms);
        }
        // GET ProductsLocations/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Get([FromRoute] int id)
        {
            if (id <= 0) return BadRequest("Input not valid or null");
            var modelDto = await _dataService.ProductsLocations.Get(id).ConfigureAwait(false);
            if (modelDto == null) return NotFound("Products Locations not found");
            return Ok(modelDto);
        }
        // POST: ProductsLocations/Create
        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] ProductsLocationsDTO inputModel)
        {
            if (inputModel == null) return BadRequest("Input not valid or null");
            if (!ModelState.IsValid) return BadRequest(ModelState.GetErrorMessages());
            //var IsDuplicate = await _dataService.ProductsLocations.CheckDuplicate(inputModel).ConfigureAwait(false);
            //if (IsDuplicate) return BadRequest("This record already exists");
            var modelDto = await _dataService.ProductsLocations.Create(inputModel).ConfigureAwait(false);
            if (modelDto != null) return Ok(modelDto);
            return BadRequest("Create failed");
        }

        // GET SkillCourses/5
        [AllowAnonymous]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetByProductId([FromRoute] int id)
        {
            if (id <= 0) return BadRequest("Input not valid or null");
            var modelVm = await _dataService.ProductsLocations.GetByProductId(id).ConfigureAwait(false);
            if (modelVm == null) return NotFound("Data not found");
            return Ok(modelVm);
        }

        // PUT: ProductsLocations/Edit/5
        [AllowAnonymous]
        [HttpPut("{id}")]
        public async Task<IActionResult> Edit([FromRoute] int id, [FromBody] ProductsLocationsDTO inputModel)
        {
            if (inputModel == null) return BadRequest("Input not valid or null");
            if (id != inputModel.Id) return BadRequest("Invalid Id");
            if (!ModelState.IsValid) return BadRequest(ModelState.GetErrorMessages());

            var modelDto = await _dataService.ProductsLocations.Update(inputModel).ConfigureAwait(false);
            if (modelDto != null) return Ok(modelDto);
            return BadRequest("Update failed");
        }
        // DELETE: ProductsLocations/Delete/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            if (id <= 0) return BadRequest("Input not valid or null");
            var rowsChanged = await _dataService.ProductsLocations.Delete(id).ConfigureAwait(false);
            if (rowsChanged > 0) return Ok(rowsChanged);
            return BadRequest("Delete failed. There might be active child records.");
        }
    }
}
