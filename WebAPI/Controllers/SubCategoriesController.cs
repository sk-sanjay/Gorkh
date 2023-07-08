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
    [Route("[controller]/[action]")]
    public class SubCategoriesController : Controller
    {
        private readonly IDataService _dataService;
        public SubCategoriesController(IDataService dataService)
        {
            _dataService = dataService;
        }
        // GET SubCategory
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var modelVms = await _dataService.SubCategories.Get().ConfigureAwait(false);
            if (modelVms == null || modelVms.Count <= 0) return NotFound("Sub-Category not found");
            return Ok(modelVms);
        }

        //        [HttpGet]
        //public async Task<IActionResult> GetCategory()
        //{
        //    var modelVms = await _dataService.SubCategories.GetCategory().ConfigureAwait(false);
        //    if (modelVms == null || modelVms.Count <= 0) return NotFound("Sub-Category not found");
        //    return Ok(modelVms);
        //}

        // GET Category/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Get([FromRoute] int id)
        {
            if (id <= 0) return BadRequest("Input not valid or null");
            var modelDto = await _dataService.SubCategories.Get(id).ConfigureAwait(false);
            if (modelDto == null) return NotFound("Category not found");
            return Ok(modelDto);
        }

        // POST: Category/Create
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] SubCategoriesDTO inputModel)
        {
            if (inputModel == null) return BadRequest("Input not valid or null");
            if (!ModelState.IsValid) return BadRequest(ModelState.GetErrorMessages());
            var IsDuplicate = await _dataService.SubCategories.CheckDuplicate(inputModel).ConfigureAwait(false);
            if (IsDuplicate) return BadRequest("This record already exists");
            var modelDto = await _dataService.SubCategories.Create(inputModel).ConfigureAwait(false);
            if (modelDto != null) return Ok(modelDto);
            return BadRequest("Create failed");
        }
        // PUT: Category/Edit/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Edit([FromRoute] int id, [FromBody] SubCategoriesDTO inputModel)
        {
            if (inputModel == null) return BadRequest("Input not valid or null");
            if (id != inputModel.Id) return BadRequest("Invalid Id");
            if (!ModelState.IsValid) return BadRequest(ModelState.GetErrorMessages());
            var IsDuplicate = await _dataService.SubCategories.CheckDuplicate(inputModel).ConfigureAwait(false);
            if (IsDuplicate) return BadRequest("This record already exists");

            var modelDto = await _dataService.SubCategories.Update(inputModel).ConfigureAwait(false);
            if (modelDto != null) return Ok(modelDto);
            return BadRequest("Update failed");
        }
        // DELETE: Category/Delete/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            if (id <= 0) return BadRequest("Input not valid or null");
            var rowsChanged = await _dataService.SubCategories.Delete(id).ConfigureAwait(false);
            if (rowsChanged > 0) return Ok(rowsChanged);
            return BadRequest("Delete failed. There might be active child records.");
        }

        // POST: States/CreateRange
        [HttpPost]
        public async Task<IActionResult> CreateRange([FromBody] List<SubCategoriesDTO> modelDtos)
        {
            if (modelDtos == null || modelDtos.Count == 0) return BadRequest("Input not valid or null");
            var modelDtosTR = await _dataService.SubCategories.CreateRange(modelDtos).ConfigureAwait(false);
            if (modelDtosTR != null) return Ok(modelDtosTR);
            return BadRequest("Bulk insertion failed");
        }
        // POST: States/Upsert
        [HttpPost]
        public async Task<IActionResult> Upsert([FromBody] List<SubCategoriesDTO> modelDtos)
        {
            if (modelDtos == null || modelDtos.Count == 0) return BadRequest("Input not valid or null");
            var modelDtosTR = await _dataService.SubCategories.Upsert(modelDtos).ConfigureAwait(false);
            if (modelDtosTR != null) return Ok(modelDtosTR);
            return BadRequest("Bulk upsertion failed");
        }
        // POST: States/DeleteRange
        [HttpPost]
        public async Task<IActionResult> DeleteRange([FromBody] List<SubCategoriesDTO> inputDtos)
        {
            if (inputDtos == null || inputDtos.Count == 0) return BadRequest("Input not valid or null");
            var rowsChanged = await _dataService.SubCategories.DeleteRange(inputDtos).ConfigureAwait(false);
            if (rowsChanged > 0) return Ok(rowsChanged);
            return BadRequest("Bulk deletion failed");
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
            var dropDownVms = await _dataService.SubCategories.GetSubcategory(maincat).ConfigureAwait(false);
            if (dropDownVms == null || dropDownVms.Count <= 0) return NotFound("Sub-category not found");
            return Ok(dropDownVms);
        }



    }
}
