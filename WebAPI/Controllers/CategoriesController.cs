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
    public class CategoriesController : Controller
    {
        private readonly IDataService _dataService;
        public CategoriesController(IDataService dataService)
        {
            _dataService = dataService;
        }

        // GET Category
        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var modelVms = await _dataService.Categories.Get().ConfigureAwait(false);
            if (modelVms == null || modelVms.Count <= 0) return NotFound("Category not found");
            return Ok(modelVms);
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> GetDropdown()
        {
            var dropDownVms = await _dataService.Categories.GetDropdown().ConfigureAwait(false);
            if (dropDownVms == null || dropDownVms.Count <= 0) return NotFound("Category not found");
            return Ok(dropDownVms);
        }

        // GET Category/5
        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> Get([FromRoute] int id)
        {
            if (id <= 0) return BadRequest("Input not valid or null");
            var modelDto = await _dataService.Categories.Get(id).ConfigureAwait(false);
            if (modelDto == null) return NotFound("Category not found");
            return Ok(modelDto);
        }



        // POST: Category/Create
        [HttpPost]

        public async Task<IActionResult> Create([FromBody] CategoriesDTO inputModel)
        {
            if (inputModel == null) return BadRequest("Input not valid or null");
            if (!ModelState.IsValid) return BadRequest(ModelState.GetErrorMessages());
            var IsDuplicate = await _dataService.Categories.CheckDuplicate(inputModel).ConfigureAwait(false);
            if (IsDuplicate) return BadRequest("This record already exists");
            var modelDto = await _dataService.Categories.Create(inputModel).ConfigureAwait(false);
            if (modelDto != null) return Ok(modelDto);
            return BadRequest("Create failed");
        }
        // PUT: Category/Edit/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Edit([FromRoute] int id, [FromBody] CategoriesDTO inputModel)
        {
            if (inputModel == null) return BadRequest("Input not valid or null");
            if (id != inputModel.Id) return BadRequest("Invalid Id");
            if (!ModelState.IsValid) return BadRequest(ModelState.GetErrorMessages());
            var IsDuplicate = await _dataService.Categories.CheckDuplicate(inputModel).ConfigureAwait(false);
            if (IsDuplicate) return BadRequest("This record already exists");
            var modelDto = await _dataService.Categories.Update(inputModel).ConfigureAwait(false);
            if (modelDto != null) return Ok(modelDto);
            return BadRequest("Update failed");
        }
        // DELETE: Category/Delete/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            if (id <= 0) return BadRequest("Input not valid or null");
            var rowsChanged = await _dataService.Categories.Delete(id).ConfigureAwait(false);
            if (rowsChanged > 0) return Ok(rowsChanged);
            return BadRequest("Delete failed. There might be active child records.");
        }
        // POST: Categories/CreateRange
        [HttpPost]
        public async Task<IActionResult> CreateRange([FromBody] List<CategoriesDTO> modelDtos)
        {
            if (modelDtos == null || modelDtos.Count == 0) return BadRequest("Input not valid or null");
            var modelDtosTR = await _dataService.Categories.CreateRange(modelDtos).ConfigureAwait(false);
            if (modelDtosTR != null) return Ok(modelDtosTR);
            return BadRequest("Bulk insertion failed");
        }
        // POST: Countries/Upsert
        [HttpPost]
        public async Task<IActionResult> Upsert([FromBody] List<CategoriesDTO> modelDtos)
        {
            if (modelDtos == null || modelDtos.Count == 0) return BadRequest("Input not valid or null");
            var modelDtosTR = await _dataService.Categories.Upsert(modelDtos).ConfigureAwait(false);
            if (modelDtosTR != null) return Ok(modelDtosTR);
            return BadRequest("Bulk upsertion failed");
        }
        // POST: Countries/DeleteRange
        [HttpPost]
        public async Task<IActionResult> DeleteRange([FromBody] List<CategoriesDTO> inputDtos)
        {
            if (inputDtos == null || inputDtos.Count == 0) return BadRequest("Input not valid or null");
            var rowsChanged = await _dataService.Categories.DeleteRange(inputDtos).ConfigureAwait(false);
            if (rowsChanged > 0) return Ok(rowsChanged);
            return BadRequest("Bulk deletion failed");
        }

        //get all category with child
        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> GetAllCategoryWithChild()
        {
            var modelVms = await _dataService.Categories.GetAllCategoryWithChild().ConfigureAwait(false);
            if (modelVms == null || modelVms.Count <= 0) return NotFound("Category not found");
            return Ok(modelVms);
        }


    }
}
