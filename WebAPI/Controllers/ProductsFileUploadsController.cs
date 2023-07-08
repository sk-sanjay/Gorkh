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
    public class ProductsFileUploadsController : Controller
    {
        private readonly IDataService _dataService;
        public ProductsFileUploadsController(IDataService dataService)
        {
            _dataService = dataService;
        }
        // GET ProductsFileUploads
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var modelVms = await _dataService.ProductsFileUploads.Get().ConfigureAwait(false);
            if (modelVms == null || modelVms.Count <= 0) return NotFound("Product Files not found");
            return Ok(modelVms);
        }
        // GET ProductsFileUploads/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Get([FromRoute] int id)
        {
            if (id <= 0) return BadRequest("Input not valid or null");
            var modelDto = await _dataService.ProductsFileUploads.Get(id).ConfigureAwait(false);
            if (modelDto == null) return NotFound("Product Files not found");
            return Ok(modelDto);
        }
        // POST: ProductsFileUploads/Create
        //[HttpPost]
        //public async Task<IActionResult> Create([FromBody] ProductsFileUploadsDTO inputModel)
        //{
        //    if (inputModel == null) return BadRequest("Input not valid or null");
        //    if (!ModelState.IsValid) return BadRequest(ModelState.GetErrorMessages());
        //    //var IsDuplicate = await _dataService.ProductsLocations.CheckDuplicate(inputModel).ConfigureAwait(false);
        //    //if (IsDuplicate) return BadRequest("This record already exists");
        //    var modelDto = await _dataService.ProductsFileUploads.Create(inputModel).ConfigureAwait(false);
        //    if (modelDto != null) return Ok(modelDto);
        //    return BadRequest("Create failed");
        //}
        [AllowAnonymous]
        [HttpPost("CreateRange")]
        public async Task<IActionResult> CreateRange([FromBody] List<ProductsFileUploadsDTO> inputModel)
        {
            if (inputModel == null) return BadRequest("Input not valid or null");
            if (!ModelState.IsValid) return BadRequest(ModelState.GetErrorMessages());
            var modelDto = await _dataService.ProductsFileUploads.CreateRange(inputModel).ConfigureAwait(false);
            if (modelDto != null) return Ok(modelDto);
            return BadRequest("Create failed");
        }
        // PUT: ProductsFileUploads/Edit/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Edit([FromRoute] int id, [FromBody] ProductsFileUploadsDTO inputModel)
        {
            if (inputModel == null) return BadRequest("Input not valid or null");
            if (id != inputModel.Id) return BadRequest("Invalid Id");
            if (!ModelState.IsValid) return BadRequest(ModelState.GetErrorMessages());

            var modelDto = await _dataService.ProductsFileUploads.Update(inputModel).ConfigureAwait(false);
            if (modelDto != null) return Ok(modelDto);
            return BadRequest("Update failed");
        }
        // DELETE: ProductsFileUploads/Delete/5
        [AllowAnonymous]
        [HttpDelete("Delete/{id}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            if (id <= 0) return BadRequest("Input not valid or null");
            var rowsChanged = await _dataService.ProductsFileUploads.Delete(id).ConfigureAwait(false);
            if (rowsChanged > 0) return Ok(rowsChanged);
            return BadRequest("Delete failed. There might be active child records.");
        }
        [AllowAnonymous]
        [HttpGet("GetProductsFileUploadsByProductId/{productid}/{flagimage}")]
        public async Task<IActionResult> GetProductsFileUploadsByProductId([FromRoute] int productid, int flagimage)
        {
            var dropDownVms = await _dataService.ProductsFileUploads.GetProductsFileUploadsByProductId(productid, flagimage).ConfigureAwait(false);
            if (dropDownVms == null || dropDownVms.Count <= 0) return NotFound("Specification not found");
            return Ok(dropDownVms);
        }

        // GET GetProductsFileUploads By Product Id/5
        [AllowAnonymous]
        [HttpGet("GetProductsFileUploadsByProductId/{productid}")]
        public async Task<IActionResult> GetProductsFileUploadsByProductId([FromRoute] int productid)
        {
            if (productid <= 0) return BadRequest("Input not valid or null");
            var modelDto = await _dataService.ProductsFileUploads.GetProductsFileUploadsByProductId(productid).ConfigureAwait(false);
            if (modelDto == null) return NotFound("Products Descriptions not found");
            return Ok(modelDto);
        }
    }
}
