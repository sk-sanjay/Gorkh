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
    public class BannersController : Controller
    {
        private readonly IDataService _dataService;
        public BannersController(IDataService dataService)
        {
            _dataService = dataService;
        }
        // GET Banners
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var modelVms = await _dataService.Banners.Get().ConfigureAwait(false);
            if (modelVms == null || modelVms.Count <= 0) return NotFound("Banners not found");
            return Ok(modelVms);
        }
        // GET Banners/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Get([FromRoute] int id)
        {
            if (id <= 0) return BadRequest("Input not valid or null");
            var modelDto = await _dataService.Banners.Get(id).ConfigureAwait(false);
            if (modelDto == null) return NotFound("Banners not found");
            return Ok(modelDto);
        }
        // POST: Banners/Create
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] BannersDTO inputModel)
        {
            if (inputModel == null) return BadRequest("Input not valid or null");
            if (!ModelState.IsValid) return BadRequest(ModelState.GetErrorMessages());
            var modelDto = await _dataService.Banners.Create(inputModel).ConfigureAwait(false);
            if (modelDto != null) return Ok(modelDto);
            return BadRequest("Create failed");
        }
        // PUT: Banners/Edit/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Edit([FromRoute] int id, [FromBody] BannersDTO inputModel)
        {
            if (inputModel == null) return BadRequest("Input not valid or null");
            if (id != inputModel.Id) return BadRequest("Invalid Id");
            if (!ModelState.IsValid) return BadRequest(ModelState.GetErrorMessages());

            var modelDto = await _dataService.Banners.Update(inputModel).ConfigureAwait(false);
            if (modelDto != null) return Ok(modelDto);
            return BadRequest("Update failed");
        }
        // DELETE: Banners/Delete/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            if (id <= 0) return BadRequest("Input not valid or null");
            var rowsChanged = await _dataService.Banners.Delete(id).ConfigureAwait(false);
            if (rowsChanged > 0) return Ok(rowsChanged);
            return BadRequest("Delete failed. There might be active child records.");
        }
        //Get banners for slider
        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> GetBannersForHomeSlider()
        {
            var dropDownVms = await _dataService.Banners.GetBannersForHomeSlider().ConfigureAwait(false);
            if (dropDownVms == null || dropDownVms.Count <= 0) return NotFound("Banners not found");
            return Ok(dropDownVms);
        }
    }
}
