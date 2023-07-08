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
    public class PaymentsController : Controller
    {
        private readonly IDataService _dataService;
        public PaymentsController(IDataService dataService)
        {
            _dataService = dataService;
        }
        // GET Payments
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var modelVms = await _dataService.Payments.Get().ConfigureAwait(false);
            if (modelVms == null || modelVms.Count <= 0) return NotFound("Buyer Intrests not found");
            return Ok(modelVms);
        }
        // GET Payments/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Get([FromRoute] int id)
        {
            if (id <= 0) return BadRequest("Input not valid or null");
            var modelDto = await _dataService.Payments.Get(id).ConfigureAwait(false);
            if (modelDto == null) return NotFound("Buyer Intrests not found");
            return Ok(modelDto);
        }
        // POST: Payments/Create
        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] PaymentsDTO inputModel)
        {
            if (inputModel == null) return BadRequest("Input not valid or null");
            if (!ModelState.IsValid) return BadRequest(ModelState.GetErrorMessages());
            var IsDuplicate = await _dataService.Payments.CheckDuplicate(inputModel).ConfigureAwait(false);
            if (IsDuplicate) return BadRequest("This record already exists");
            var modelDto = await _dataService.Payments.Create(inputModel).ConfigureAwait(false);
            if (modelDto != null) return Ok(modelDto);
            return BadRequest("Create failed");
        }
        // PUT: Payments/Edit/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Edit([FromRoute] int id, [FromBody] PaymentsDTO inputModel)
        {
            if (inputModel == null) return BadRequest("Input not valid or null");
            if (id != inputModel.Id) return BadRequest("Invalid Id");
            if (!ModelState.IsValid) return BadRequest(ModelState.GetErrorMessages());
            var modelDto = await _dataService.Payments.Update(inputModel).ConfigureAwait(false);
            if (modelDto != null) return Ok(modelDto);
            return BadRequest("Update failed");
        }
        // DELETE: Payments/Delete/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            if (id <= 0) return BadRequest("Input not valid or null");
            var rowsChanged = await _dataService.Payments.Delete(id).ConfigureAwait(false);
            if (rowsChanged > 0) return Ok(rowsChanged);
            return BadRequest("Delete failed. There might be active child records.");
        }
        //Payments bind for admin
        [HttpGet]
        public async Task<IActionResult> GetPaymentsForAdmin()
        {
            var modelVms = await _dataService.Payments.GetPaymentsForAdmin().ConfigureAwait(false);
            if (modelVms == null || modelVms.Count <= 0) return NotFound("Products Payments not found");
            return Ok(modelVms);
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePaymentStatus([FromRoute] int id, [FromBody] PaymentsUpdateDTO inputModel)
        {
            if (inputModel == null) return BadRequest("Input not valid or null");
            if (id != inputModel.Id) return BadRequest("Invalid Id");
            if (!ModelState.IsValid) return BadRequest(ModelState.GetErrorMessages());

            var modelDto = await _dataService.Payments.UpdatePaymentStatus(inputModel).ConfigureAwait(false);
            if (modelDto != 0) return Ok(modelDto);
            return BadRequest("Update failed");
        }
        // GET Products Payments/5/5
        [HttpGet("{buyerid}/{productid}")]
        public async Task<IActionResult> GetProductsPaymentsByBuyerandPid([FromRoute] int buyerid, int productid)
        {
            if (productid <= 0 && buyerid <= 0) return BadRequest("Input not valid or null");
            var modelDto = await _dataService.Payments.GetProductsPaymentsByBuyerandPid(buyerid, productid).ConfigureAwait(false);
            if (modelDto == null) return NotFound("Products payments not found");
            return Ok(modelDto);
        }
        //Get Products Payments By Buyer
        //[AllowAnonymous]
        [HttpGet("{buyerid}")]
        public async Task<IActionResult> GetProductsPaymentsByBuyer([FromRoute] int buyerid)
        {
            var dropDownVms = await _dataService.Payments.GetProductsPaymentsByBuyer(buyerid).ConfigureAwait(false);
            if (dropDownVms == null || dropDownVms.Count <= 0) return NotFound("Payments not found");
            return Ok(dropDownVms);
        }
        [HttpGet]
        public async Task<IActionResult> GetProductsPaymentsForAdmin()
        {
            var modelVms = await _dataService.Payments.GetProductsPaymentsForAdmin().ConfigureAwait(false);
            if (modelVms == null || modelVms.Count <= 0) return NotFound("Payments not found");
            return Ok(modelVms);
        }
        [HttpGet("{productid}")]
        public async Task<IActionResult> GetPaymentsByProductIdForAdmin([FromRoute] int productid)
        {
            var modelVms = await _dataService.Payments.GetPaymentsByProductIdForAdmin(productid).ConfigureAwait(false);
            if (modelVms == null || modelVms.Count <= 0) return NotFound("Products Payments not found");
            return Ok(modelVms);
        }
        [HttpGet("{sellerid}")]
        public async Task<IActionResult> GetProductsPaymentsForSeller(int sellerid)
        {
            var modelVms = await _dataService.Payments.GetProductsPaymentsForSeller(sellerid).ConfigureAwait(false);
            if (modelVms == null || modelVms.Count <= 0) return NotFound("Payments not found");
            return Ok(modelVms);
        }
        [HttpGet("{productid}")]
        public async Task<IActionResult> GetPaymentsByProductIdForSeller([FromRoute] int productid)
        {
            var modelVms = await _dataService.Payments.GetPaymentsByProductIdForSeller(productid).ConfigureAwait(false);
            if (modelVms == null || modelVms.Count <= 0) return NotFound("Products Payments not found");
            return Ok(modelVms);
        }
    }
}
