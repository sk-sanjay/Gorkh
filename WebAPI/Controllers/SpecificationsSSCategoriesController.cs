using Application.Dtos;
using Application.Extensions;
using Application.ServiceInterfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
namespace WebAPI.Controllers
{
    [Authorize]
    [ApiController]
    //[Route("[controller]/[action]")]
    [Route("[controller]")]
    public class SpecificationsSSCategoriesController : Controller
    {
        private readonly IDataService _dataService;
        public SpecificationsSSCategoriesController(IDataService dataService)
        {
            _dataService = dataService;
        }
        [AllowAnonymous]
        // GET SpecificationsSSCategories
        [HttpGet("Get")]
        public async Task<IActionResult> Get()
        {
            var modelVms = await _dataService.SpecificationsSSCategories.Get().ConfigureAwait(false);
            if (modelVms == null || modelVms.Count <= 0) return NotFound("Specifications not found");
            return Ok(modelVms);
        }
        // GET SpecificationsSSCategories/5
        //[HttpGet("{id}")]
        [HttpGet("Get/{id}")]
        public async Task<IActionResult> Get([FromRoute] int id)
        {
            if (id <= 0) return BadRequest("Input not valid or null");
            var modelDto = await _dataService.SpecificationsSSCategories.Get(id).ConfigureAwait(false);
            if (modelDto == null) return NotFound("State not found");
            return Ok(modelDto);
        }
        // POST: SpecificationsSSCategories/Create
        //[HttpPost]
        [HttpPost("Create")]
        public async Task<IActionResult> Create([FromBody] SpecificationsSSCategoriesDTO inputModel)
        {
            if (inputModel == null) return BadRequest("Input not valid or null");
            if (!ModelState.IsValid) return BadRequest(ModelState.GetErrorMessages());
            var modelDto = await _dataService.SpecificationsSSCategories.Create(inputModel).ConfigureAwait(false);
            if (modelDto != null) return Ok(modelDto);
            return BadRequest("Create failed");
        }
        // PUT: SpecificationsSSCategories/Edit/5
        //[HttpPut("{id}")]
        [HttpPut("Edit/{id}")]
        public async Task<IActionResult> Edit([FromRoute] int id, [FromBody] SpecificationsSSCategoriesDTO inputModel)
        {
            if (inputModel == null) return BadRequest("Input not valid or null");
            if (id != inputModel.Id) return BadRequest("Invalid Id");
            if (!ModelState.IsValid) return BadRequest(ModelState.GetErrorMessages());
            var modelDto = await _dataService.SpecificationsSSCategories.Update(inputModel).ConfigureAwait(false);
            if (modelDto != null) return Ok(modelDto);
            return BadRequest("Update failed");
        }
        // DELETE: SpecificationsSSCategories/Delete/5
        //[HttpDelete("{id}")]
        [HttpDelete("Delete/{id}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            if (id <= 0) return BadRequest("Input not valid or null");
            var rowsChanged = await _dataService.SpecificationsSSCategories.Delete(id).ConfigureAwait(false);
            if (rowsChanged > 0) return Ok(rowsChanged);
            return BadRequest("Delete failed. There might be active child records.");
        }
        [AllowAnonymous]
        // GET GetDropdown
        [HttpGet("GetDropdown")]
        public async Task<IActionResult> GetDropdown()
        {
            var dropDownVms = await _dataService.Categories.GetDropdown().ConfigureAwait(false);
            if (dropDownVms == null || dropDownVms.Count <= 0) return NotFound("Category not found");
            return Ok(dropDownVms);
        }
        [AllowAnonymous]
        [HttpGet("GetSubcategory/{maincat}")]
        public async Task<IActionResult> GetSubcategory(int maincat)
        {
            var dropDownVms = await _dataService.SubSubCategories.GetSubcategory(maincat).ConfigureAwait(false);
            if (dropDownVms == null || dropDownVms.Count <= 0) return NotFound("Sub-category not found");
            return Ok(dropDownVms);
        }

        // POST: SpecificationsSSCategories/CreateRange
        //[HttpPost]
        [HttpPost("CreateRange")]
        public async Task<IActionResult> CreateRange([FromBody] List<SpecificationsSSCategoriesDTO> modelDtos)
        {
            if (modelDtos == null || modelDtos.Count == 0) return BadRequest("Input not valid or null");
            var modelDtosTR = await _dataService.SpecificationsSSCategories.CreateRange(modelDtos).ConfigureAwait(false);
            if (modelDtosTR != null) return Ok(modelDtosTR);
            return BadRequest("Bulk insertion failed");
        }
        [AllowAnonymous]
        //[HttpGet("{subsubcategoryid}")]
        [HttpGet("GetSpecificationsSSCategories/{subsubcategoryid}")]
        public async Task<IActionResult> GetSpecificationsSSCategories(int subsubcategoryid)
        {
            var dropDownVms = await _dataService.SpecificationsSSCategories.GetSpecificationsSSCategories(subsubcategoryid).ConfigureAwait(false);
            if (dropDownVms == null || dropDownVms.Count <= 0) return NotFound("Sub-Sub-category not found");
            return Ok(dropDownVms);
        }

        // POST: SpecificationsSSCategories/CreateRange
        [HttpPost("UpdateSpecificationsSSCategories")]
        public async Task<IActionResult> UpdateSpecificationsSSCategories([FromBody] List<SpecificationsSSCategoriesDTO> argModelDtos)
        {
            if (argModelDtos == null || argModelDtos.Count <= 0) return BadRequest("Input not valid or null");
            var result = await _dataService.SpecificationsSSCategories.UpdateSpecificationsSSCategories(argModelDtos).ConfigureAwait(false);
            return Ok(result);
        }

        [AllowAnonymous]
        //[HttpGet("{subsubcategoryid}")]
        [HttpGet("GetSpecificationsSSCategoriesjoin/{subsubcategoryid}")]
        public async Task<IActionResult> GetSpecificationsSSCategoriesjoin(int subsubcategoryid)
        {
            var dropDownVms = await _dataService.SpecificationsSSCategories.GetSpecificationsSSCategoriesjoin(subsubcategoryid).ConfigureAwait(false);
            if (dropDownVms == null || dropDownVms.Count <= 0) return NotFound("Specification not found");
            return Ok(dropDownVms);
        }
    }
}
