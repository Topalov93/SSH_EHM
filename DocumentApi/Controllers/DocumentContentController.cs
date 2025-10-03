using DocumentApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace DocumentApi.Controllers
{
    [Route("api/documents/{id:guid}/content")]
    public class DocumentContentController : Controller
    {
        private readonly IDocumentService _service;

        public DocumentContentController(IDocumentService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> Download(Guid id)
        {
            var doc = await _service.GetAsync(id);
            if (doc == null) return NotFound();

            var stream = await _service.DownloadAsync(id);
            if (stream == null) return NotFound(new { error = "blob_not_found" });

            return File(stream, doc.ContentType, doc.FileName);
        }

        [HttpPut]
        [RequestSizeLimit(524288000)]
        public async Task<IActionResult> Replace(Guid id, [FromForm] IFormFile file)
        {
            var doc = await _service.UpdateFileAsync(id, file);
            if (doc == null) return NotFound();
            return Json(doc);
        }
    }
}
