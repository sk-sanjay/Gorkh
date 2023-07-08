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
    public class ProductsPurchasesController : Controller
    {
        private readonly IDataService _dataService;
        public ProductsPurchasesController(IDataService dataService)
        {
            _dataService = dataService;
        }
        // GET ProductsPurchases
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var modelVms = await _dataService.ProductsPurchases.Get().ConfigureAwait(false);
            if (modelVms == null || modelVms.Count <= 0) return NotFound("Buyer Intrests not found");
            return Ok(modelVms);
        }
        // GET ProductsPurchases/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Get([FromRoute] int id)
        {
            if (id <= 0) return BadRequest("Input not valid or null");
            var modelDto = await _dataService.ProductsPurchases.Get(id).ConfigureAwait(false);
            if (modelDto == null) return NotFound("Buyer Intrests not found");
            return Ok(modelDto);
        }
        // POST: ProductsPurchases/Create
        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] ProductsPurchasesDTO inputModel)
        {
            if (inputModel == null) return BadRequest("Input not valid or null");
            if (!ModelState.IsValid) return BadRequest(ModelState.GetErrorMessages());
            var IsDuplicate = await _dataService.ProductsPurchases.CheckDuplicate(inputModel).ConfigureAwait(false);
            if (IsDuplicate) return BadRequest("This record already exists");
            var modelDto = await _dataService.ProductsPurchases.Create(inputModel).ConfigureAwait(false);
            if (modelDto != null) return Ok(modelDto);
            return BadRequest("Create failed");
        }
        // PUT: ProductsPurchases/Edit/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Edit([FromRoute] int id, [FromBody] ProductsPurchasesDTO inputModel)
        {
            if (inputModel == null) return BadRequest("Input not valid or null");
            if (id != inputModel.Id) return BadRequest("Invalid Id");
            if (!ModelState.IsValid) return BadRequest(ModelState.GetErrorMessages());
            var modelDto = await _dataService.ProductsPurchases.Update(inputModel).ConfigureAwait(false);
            if (modelDto != null) return Ok(modelDto);
            return BadRequest("Update failed");
        }
        // DELETE: ProductsPurchases/Delete/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            if (id <= 0) return BadRequest("Input not valid or null");
            var rowsChanged = await _dataService.ProductsPurchases.Delete(id).ConfigureAwait(false);
            if (rowsChanged > 0) return Ok(rowsChanged);
            return BadRequest("Delete failed. There might be active child records.");
        }
        //Get Products Purchases By Buyer
        [AllowAnonymous]
        [HttpGet("{buyerid}")]
        public async Task<IActionResult> GetProductsPurchasesByBuyer([FromRoute] int buyerid)
        {
            var dropDownVms = await _dataService.ProductsPurchases.GetProductsPurchasesByBuyer(buyerid).ConfigureAwait(false);
            if (dropDownVms == null || dropDownVms.Count <= 0) return NotFound("Products not found");
            return Ok(dropDownVms);
        }
        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> GetProductsPurchasesForAdmin()
        {
            var dropDownVms = await _dataService.ProductsPurchases.GetProductsPurchasesForAdmin().ConfigureAwait(false);
            if (dropDownVms == null || dropDownVms.Count <= 0) return NotFound("Products purchases not found");
            return Ok(dropDownVms);
        }
        // GET ProductsPurchases/5/5
        [HttpGet("{buyerid}/{productid}")]
        public async Task<IActionResult> GetProductsPurchasesByBuyerandPid([FromRoute] int buyerid, int productid)
        {
            if (productid <= 0 && buyerid <= 0) return BadRequest("Input not valid or null");
            var modelDto = await _dataService.ProductsPurchases.GetProductsPurchasesByBuyerandPid(buyerid, productid).ConfigureAwait(false);
            if (modelDto == null) return NotFound("Products purchases not found");
            return Ok(modelDto);
        }
    }
}
