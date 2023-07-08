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

    public class TestimonialsController : Controller
    {
        private readonly IDataService _dataService;
        public TestimonialsController(IDataService dataService)
        {
            _dataService = dataService;
        }

        // GET Testimonials
        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var modelVms = await _dataService.Testimonials.Get().ConfigureAwait(false);
            if (modelVms == null || modelVms.Count <= 0) return NotFound("Category not found");
            return Ok(modelVms);
        }
        
        // GET Testimonials/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Get([FromRoute] int id)
        {
            if (id <= 0) return BadRequest("Input not valid or null");
            var modelDto = await _dataService.Testimonials.Get(id).ConfigureAwait(false);
            if (modelDto == null) return NotFound("Category not found");
            return Ok(modelDto);
        }
        
        // POST: Testimonials/Create
        [HttpPost]

        public async Task<IActionResult> Create([FromBody] TestimonialsDTO inputModel)
        {
            if (inputModel == null) return BadRequest("Input not valid or null");
            if (!ModelState.IsValid) return BadRequest(ModelState.GetErrorMessages());
            var IsDuplicate = await _dataService.Testimonials.CheckDuplicate(inputModel).ConfigureAwait(false);
            if (IsDuplicate) return BadRequest("This record already exists");
            var modelDto = await _dataService.Testimonials.Create(inputModel).ConfigureAwait(false);
            if (modelDto != null) return Ok(modelDto);
            return BadRequest("Create failed");
        }
        // PUT: Testimonials/Edit/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Edit([FromRoute] int id, [FromBody] TestimonialsDTO inputModel)
        {
            if (inputModel == null) return BadRequest("Input not valid or null");
            if (id != inputModel.Id) return BadRequest("Invalid Id");
            if (!ModelState.IsValid) return BadRequest(ModelState.GetErrorMessages());
            var IsDuplicate = await _dataService.Testimonials.CheckDuplicate(inputModel).ConfigureAwait(false);
            if (IsDuplicate) return BadRequest("This record already exists");
            var modelDto = await _dataService.Testimonials.Update(inputModel).ConfigureAwait(false);
            if (modelDto != null) return Ok(modelDto);
            return BadRequest("Update failed");
        }
        // DELETE: Testimonials/Delete/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            if (id <= 0) return BadRequest("Input not valid or null");
            var rowsChanged = await _dataService.Testimonials.Delete(id).ConfigureAwait(false);
            if (rowsChanged > 0) return Ok(rowsChanged);
            return BadRequest("Delete failed. There might be active child records.");
        }
        // POST: Testimonials/CreateRange
        [HttpPost]
        public async Task<IActionResult> CreateRange([FromBody] List<TestimonialsDTO> modelDtos)
        {
            if (modelDtos == null || modelDtos.Count == 0) return BadRequest("Input not valid or null");
            var modelDtosTR = await _dataService.Testimonials.CreateRange(modelDtos).ConfigureAwait(false);
            if (modelDtosTR != null) return Ok(modelDtosTR);
            return BadRequest("Bulk insertion failed");
        }
        // POST: Testimonials/Upsert
        [HttpPost]
        public async Task<IActionResult> Upsert([FromBody] List<TestimonialsDTO> modelDtos)
        {
            if (modelDtos == null || modelDtos.Count == 0) return BadRequest("Input not valid or null");
            var modelDtosTR = await _dataService.Testimonials.Upsert(modelDtos).ConfigureAwait(false);
            if (modelDtosTR != null) return Ok(modelDtosTR);
            return BadRequest("Bulk upsertion failed");
        }
        // POST: Testimonials/DeleteRange
        [HttpPost]
        public async Task<IActionResult> DeleteRange([FromBody] List<TestimonialsDTO> inputDtos)
        {
            if (inputDtos == null || inputDtos.Count == 0) return BadRequest("Input not valid or null");
            var rowsChanged = await _dataService.Testimonials.DeleteRange(inputDtos).ConfigureAwait(false);
            if (rowsChanged > 0) return Ok(rowsChanged);
            return BadRequest("Bulk deletion failed");
        }

        //Get all Testimonials
        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> GetTestimonials()
        {
            var dropDownVms = await _dataService.Testimonials.GetTestimonials().ConfigureAwait(false);
            if (dropDownVms == null || dropDownVms.Count <= 0) return NotFound("GetTestimonials not found");
            return Ok(dropDownVms);
        }


    }
}
