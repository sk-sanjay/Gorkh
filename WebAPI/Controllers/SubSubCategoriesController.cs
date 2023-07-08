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
    public class SubSubCategoriesController : Controller
    {
        private readonly IDataService _dataService;
        public SubSubCategoriesController(IDataService dataService)
        {
            _dataService = dataService;
        }
        // GET SubCategory
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var modelVms = await _dataService.SubSubCategories.Get().ConfigureAwait(false);
            if (modelVms == null || modelVms.Count <= 0) return NotFound("Sub-Sub-Category not found");
            return Ok(modelVms);
        }

        // GET Category/5
        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> Get([FromRoute] int id)
        {
            if (id <= 0) return BadRequest("Input not valid or null");
            var modelDto = await _dataService.SubSubCategories.Get(id).ConfigureAwait(false);
            if (modelDto == null) return NotFound("Category not found");
            return Ok(modelDto);
        }

        // POST: Category/Create
        [HttpPost]

        public async Task<IActionResult> Create([FromBody] SubSubCategoriesDTO inputModel)
        {
            if (inputModel == null) return BadRequest("Input not valid or null");
            if (!ModelState.IsValid) return BadRequest(ModelState.GetErrorMessages());
            var IsDuplicate = await _dataService.SubSubCategories.CheckDuplicate(inputModel).ConfigureAwait(false);
            if (IsDuplicate) return BadRequest("This record already exists");
            var modelDto = await _dataService.SubSubCategories.Create(inputModel).ConfigureAwait(false);
            if (modelDto != null) return Ok(modelDto);
            return BadRequest("Create failed");
        }
        // PUT: Category/Edit/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Edit([FromRoute] int id, [FromBody] SubSubCategoriesDTO inputModel)
        {
            if (inputModel == null) return BadRequest("Input not valid or null");
            if (id != inputModel.Id) return BadRequest("Invalid Id");
            if (!ModelState.IsValid) return BadRequest(ModelState.GetErrorMessages());
            var IsDuplicate = await _dataService.SubSubCategories.CheckDuplicate(inputModel).ConfigureAwait(false);
            if (IsDuplicate) return BadRequest("This record already exists");

            var modelDto = await _dataService.SubSubCategories.Update(inputModel).ConfigureAwait(false);
            if (modelDto != null) return Ok(modelDto);
            return BadRequest("Update failed");
        }
        // DELETE: Category/Delete/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            if (id <= 0) return BadRequest("Input not valid or null");
            var rowsChanged = await _dataService.SubSubCategories.Delete(id).ConfigureAwait(false);
            if (rowsChanged > 0) return Ok(rowsChanged);
            return BadRequest("Delete failed. There might be active child records.");
        }
        [AllowAnonymous]
        // GET GetDropdown
        [HttpGet]
        public async Task<IActionResult> GetDropdown()
        {
            var dropDownVms = await _dataService.Categories.GetDropdown().ConfigureAwait(false);
            if (dropDownVms == null || dropDownVms.Count <= 0) return NotFound("States not found");
            return Ok(dropDownVms);
        }
        [AllowAnonymous]
        [HttpGet("{maincat}")]
        public async Task<IActionResult> GetSubcategory(int maincat)
        {
            var dropDownVms = await _dataService.SubSubCategories.GetSubcategory(maincat).ConfigureAwait(false);
            if (dropDownVms == null || dropDownVms.Count <= 0) return NotFound("Sub-category not found");
            return Ok(dropDownVms);
        }

        [AllowAnonymous]
        [HttpGet("{subcategoryid}")]
        public async Task<IActionResult> GetSubSubCategoryBySubCategory2(int subcategoryid)
        {
            var dropDownVms = await _dataService.SubSubCategories.GetSubSubCategoryBySubCategory2(subcategoryid).ConfigureAwait(false);
            if (dropDownVms == null || dropDownVms.Count <= 0) return NotFound("Sub-Sub-category not found");
            return Ok(dropDownVms);
        }
        [AllowAnonymous]
        [HttpGet("{prefix}")]
        public async Task<IActionResult> SearchSubSubCategory(string prefix)
        {
            var modelVms = await _dataService.SubSubCategories.SearchSubSubCategory(prefix).ConfigureAwait(false);
            if (modelVms == null || modelVms.Count <= 0) return NotFound("Sub-Sub-category not found");
            return Ok(modelVms);
        }
    }
}
