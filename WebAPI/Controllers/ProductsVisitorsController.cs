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
    public class ProductsVisitorsController : Controller
    {
        private readonly IDataService _dataService;
        public ProductsVisitorsController(IDataService dataService)
        {
            _dataService = dataService;
        }
        // GET Products Visitors
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var modelVms = await _dataService.ProductsVisitors.Get().ConfigureAwait(false);
            if (modelVms == null || modelVms.Count <= 0) return NotFound("Products Visitors not found");
            return Ok(modelVms);
        }
        // GET ProductsVisitors/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Get([FromRoute] int id)
        {
            if (id <= 0) return BadRequest("Input not valid or null");
            var modelDto = await _dataService.ProductsVisitors.Get(id).ConfigureAwait(false);
            if (modelDto == null) return NotFound("Products Visitors not found");
            return Ok(modelDto);
        }
        // POST: ProductsVisitors/Create
        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] ProductsVisitorsDTO inputModel)
        {
            if (inputModel == null) return BadRequest("Input not valid or null");
            if (!ModelState.IsValid) return BadRequest(ModelState.GetErrorMessages());
            var modelDto = await _dataService.ProductsVisitors.Create(inputModel).ConfigureAwait(false);
            if (modelDto != null) return Ok(modelDto);
            return BadRequest("Create failed");
        }
        // PUT: ProductsVisitors/Edit/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Edit([FromRoute] int id, [FromBody] ProductsVisitorsDTO inputModel)
        {
            if (inputModel == null) return BadRequest("Input not valid or null");
            if (id != inputModel.Id) return BadRequest("Invalid Id");
            if (!ModelState.IsValid) return BadRequest(ModelState.GetErrorMessages());
            var modelDto = await _dataService.ProductsVisitors.Update(inputModel).ConfigureAwait(false);
            if (modelDto != null) return Ok(modelDto);
            return BadRequest("Update failed");
        }
        // DELETE: ProductsVisitors/Delete/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            if (id <= 0) return BadRequest("Input not valid or null");
            var rowsChanged = await _dataService.ProductsVisitors.Delete(id).ConfigureAwait(false);
            if (rowsChanged > 0) return Ok(rowsChanged);
            return BadRequest("Delete failed. There might be active child records.");
        }
    }
}
