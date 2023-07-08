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
    public class ProductsBuyerFavoritesController : Controller
    {
        private readonly IDataService _dataService;
        public ProductsBuyerFavoritesController(IDataService dataService)
        {
            _dataService = dataService;
        }
        // GET ProductsBuyerFavorites
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var modelVms = await _dataService.ProductsBuyerFavorites.Get().ConfigureAwait(false);
            if (modelVms == null || modelVms.Count <= 0) return NotFound("Buyer Favourite not found");
            return Ok(modelVms);
        }
        // GET ProductsBuyerFavorites/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Get([FromRoute] int id)
        {
            if (id <= 0) return BadRequest("Input not valid or null");
            var modelDto = await _dataService.ProductsBuyerFavorites.Get(id).ConfigureAwait(false);
            if (modelDto == null) return NotFound("Buyer Favorite not found");
            return Ok(modelDto);
        }
        // POST: ProductsBuyerFavorites/Create
        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] ProductsBuyerFavoritesDTO inputModel)
        {
            if (inputModel == null) return BadRequest("Input not valid or null");
            if (!ModelState.IsValid) return BadRequest(ModelState.GetErrorMessages());
            var IsDuplicate = await _dataService.ProductsBuyerFavorites.CheckDuplicate(inputModel).ConfigureAwait(false);
            if (IsDuplicate) return BadRequest("This record already exists");
            var modelDto = await _dataService.ProductsBuyerFavorites.Create(inputModel).ConfigureAwait(false);
            if (modelDto != null) return Ok(modelDto);
            return BadRequest("Create failed");
        }
        // PUT: ProductsBuyerFavorites/Edit/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Edit([FromRoute] int id, [FromBody] ProductsBuyerFavoritesDTO inputModel)
        {
            if (inputModel == null) return BadRequest("Input not valid or null");
            if (id != inputModel.Id) return BadRequest("Invalid Id");
            if (!ModelState.IsValid) return BadRequest(ModelState.GetErrorMessages());
            var modelDto = await _dataService.ProductsBuyerFavorites.Update(inputModel).ConfigureAwait(false);
            if (modelDto != null) return Ok(modelDto);
            return BadRequest("Update failed");
        }
        // DELETE: ProductsBuyerFavorites/Delete/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            if (id <= 0) return BadRequest("Input not valid or null");
            var rowsChanged = await _dataService.ProductsBuyerFavorites.Delete(id).ConfigureAwait(false);
            if (rowsChanged > 0) return Ok(rowsChanged);
            return BadRequest("Delete failed. There might be active child records.");
        }

        [AllowAnonymous]
        [HttpGet("{buyerid}")]
        public async Task<IActionResult> GetFavoutiteProductsbybuyerid([FromRoute] int buyerid)
        {
            var modelVms = await _dataService.ProductsBuyerFavorites.GetFavoutiteProductsbybuyerid(buyerid).ConfigureAwait(false);
            if (modelVms == null || modelVms.Count <= 0) return NotFound("Products Interest not found");
            return Ok(modelVms);
        }
    }
}
