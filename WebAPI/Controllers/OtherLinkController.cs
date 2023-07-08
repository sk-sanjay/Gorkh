using Application.Dtos;
using Application.ServiceInterfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPI.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]/[action]")]
    public class OtherLinkController : Controller
    {
        private readonly IDataService _dataService;
        public OtherLinkController(IDataService dataService)
        {
            _dataService = dataService;
        }
        // POST: Categories/Create
        [HttpPost]

        public async Task<IActionResult> Create([FromBody] OtherLinkDTO modelDto)
        {
            if (modelDto == null) return BadRequest("Input not valid or null");
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var categoryDto = await _dataService.OtherLinkHeadings.Add(modelDto).ConfigureAwait(false);
            if (categoryDto == null) return BadRequest("Create failed");
            return Ok(modelDto);
        }
        // GET Categories

        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var categories = await _dataService.OtherLinkHeadings.Get().ConfigureAwait(false);
            if (categories == null || categories.Count <= 0) return NotFound("Headings not found");
            return Ok(categories.OrderByDescending(x => x.Id));
        }

        // GET Categories/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Get([FromRoute] int id)
        {
            if (id <= 0) return BadRequest("Input not valid or null");
            var category = await _dataService.OtherLinkHeadings.Get(id).ConfigureAwait(false);
            if (category == null) return NotFound("Menu headings not found");
            return Ok(category);
        }

        [AllowAnonymous]
        // GET Categories/GetCategoriesWithAll
        [HttpGet]
        public async Task<IActionResult> GetMenuHeadingsWithAll()
        {
            var Categories = await _dataService.OtherLinkHeadings.GetCategoriesWithAll().ConfigureAwait(false);
            if (Categories == null || Categories.Count <= 0) return NotFound("Headings not found");
            return Ok(Categories);
        }

        [AllowAnonymous]
        [HttpGet("{Heading}")]
        public async Task<IActionResult> GetMenus([FromRoute] string Heading)
        {
            if (Heading == null) return BadRequest("Input not valid or null");
            var category = await _dataService.OtherLinkHeadings.Get(Heading).ConfigureAwait(false);
            if (category == null) return NotFound("headings not found");
            return Ok(category);
        }

        // PUT: Categories/Edit/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Edit([FromRoute] int id, [FromBody] OtherLinkDTO modelDto)
        {
            if (modelDto == null) return BadRequest("Input not valid or null");
            if (id != modelDto.Id) return BadRequest("Invalid Id");
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var categoryDto = await _dataService.OtherLinkHeadings.Update(modelDto).ConfigureAwait(false);
            if (categoryDto == null) return BadRequest("Update failed");
            return Ok(modelDto);
        }


    }

}
