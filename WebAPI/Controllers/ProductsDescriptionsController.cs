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
    public class ProductsDescriptionsController : Controller
    {
        private readonly IDataService _dataService;
        public ProductsDescriptionsController(IDataService dataService)
        {
            _dataService = dataService;
        }
        // GET ProductsDescriptions
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var modelVms = await _dataService.ProductsDescriptions.Get().ConfigureAwait(false);
            if (modelVms == null || modelVms.Count <= 0) return NotFound("Products Descriptions not found");
            return Ok(modelVms);
        }
        // GET ProductsDescriptions/5
        [AllowAnonymous]
        [HttpGet("{id}")]
        public async Task<IActionResult> Get([FromRoute] int id)
        {
            if (id <= 0) return BadRequest("Input not valid or null");
            var modelDto = await _dataService.ProductsDescriptions.Get(id).ConfigureAwait(false);
            if (modelDto == null) return NotFound("Products Descriptions not found");
            return Ok(modelDto);
        }
        [AllowAnonymous]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetByProductId([FromRoute] int id)
        {
            if (id <= 0) return BadRequest("Input not valid or null");
            var modelVm = await _dataService.ProductsDescriptions.GetByProductId(id).ConfigureAwait(false);
            if (modelVm == null) return NotFound("Data not found");
            return Ok(modelVm);
        }
        // POST: ProductsDescriptions/Create
        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] ProductsDescriptionsDTO inputModel)
        {
            if (inputModel == null) return BadRequest("Input not valid or null");
            if (!ModelState.IsValid) return BadRequest(ModelState.GetErrorMessages());
            //var IsDuplicate = await _dataService.ProductsLocations.CheckDuplicate(inputModel).ConfigureAwait(false);
            //if (IsDuplicate) return BadRequest("This record already exists");
            var modelDto = await _dataService.ProductsDescriptions.Create(inputModel).ConfigureAwait(false);
            if (modelDto != null) return Ok(modelDto);
            return BadRequest("Create failed");
        }
        // PUT: ProductsDescriptions/Edit/5
        [AllowAnonymous]
        [HttpPut("{id}")]
        public async Task<IActionResult> Edit([FromRoute] int id, [FromBody] ProductsDescriptionsDTO inputModel)
        {
            if (inputModel == null) return BadRequest("Input not valid or null");
            if (id != inputModel.Id) return BadRequest("Invalid Id");
            if (!ModelState.IsValid) return BadRequest(ModelState.GetErrorMessages());

            var modelDto = await _dataService.ProductsDescriptions.Update(inputModel).ConfigureAwait(false);
            if (modelDto != null) return Ok(modelDto);
            return BadRequest("Update failed");
        }
        // DELETE: ProductsDescriptions/Delete/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            if (id <= 0) return BadRequest("Input not valid or null");
            var rowsChanged = await _dataService.ProductsDescriptions.Delete(id).ConfigureAwait(false);
            if (rowsChanged > 0) return Ok(rowsChanged);
            return BadRequest("Delete failed. There might be active child records.");
        }

        // GET ProductsDescriptions By Product Id/5
        [AllowAnonymous]
        [HttpGet("{productid}")]
        public async Task<IActionResult> GetProductsDescriptionsByProductId([FromRoute] int productid)
        {
            if (productid <= 0) return BadRequest("Input not valid or null");
            var modelDto = await _dataService.ProductsDescriptions.GetProductsDescriptionsByProductId(productid).ConfigureAwait(false);
            if (modelDto == null) return NotFound("Products Descriptions not found");
            return Ok(modelDto);
        }
    }
}
