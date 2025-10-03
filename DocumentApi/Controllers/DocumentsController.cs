using DocumentApi.Models;
using DocumentApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace DocumentApi.Controllers
{
    [Route("api/[controller]")]
    public class DocumentsController : Controller
    {
        private readonly IDocumentService _service;

        public DocumentsController(IDocumentService service)
        {
            _service = service;
        }

        [HttpPost]
        [RequestSizeLimit(524288000)]
        public async Task<IActionResult> Create(IFormFile? file, [FromForm] string? metadata)
        {
            if (file == null && metadata == null)
                return BadRequest("File or metadata must be provided.");

            var doc = await _service.CreateAsync(file, metadata);
            return CreatedAtAction(nameof(Get), new { id = doc.Id }, doc);
        }

        [HttpGet]
        public async Task<IActionResult> List([FromQuery] int page = 1, [FromQuery] int pageSize = 20)
        {
            var items = await _service.ListAsync(page, pageSize);
            return Json(new { items, page, pageSize });
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var doc = await _service.GetAsync(id);
            if (doc == null) return NotFound();
            return Json(doc);
        }

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] Document updated)
        {
            var doc = await _service.UpdateMetadataAsync(id, updated);
            if (doc == null) return NotFound();
            return Json(doc);
        }

        [HttpPatch("{id:guid}")]
        public async Task<IActionResult> Patch(Guid id, [FromBody] Dictionary<string, string> updates)
        {
            var doc = await _service.PatchMetadataAsync(id, updates);
            if (doc == null) return NotFound();
            return Json(doc);
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var deleted = await _service.DeleteAsync(id);
            if (!deleted) return NotFound();
            return NoContent();
        }
    }
}
