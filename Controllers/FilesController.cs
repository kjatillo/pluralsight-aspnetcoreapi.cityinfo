using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;

namespace Pluralsight.AspNetCoreWebApi.CityInfo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FilesController : ControllerBase
    {
        private readonly FileExtensionContentTypeProvider _fileExtensionContentTypeProvider;

        public FilesController(FileExtensionContentTypeProvider fileExtensionContentTypeProvider)
        {
            _fileExtensionContentTypeProvider = fileExtensionContentTypeProvider
                ?? throw new System.ArgumentException(nameof(fileExtensionContentTypeProvider));
        }

        [HttpGet("{fieldId}")]
        public IActionResult GetFile(string fileId)
        {
            var pathToFile = "getting-started-with-rest-slides.pdf";

            if (!System.IO.File.Exists(pathToFile))
            {
                return NotFound();
            }

            if (!_fileExtensionContentTypeProvider.TryGetContentType(pathToFile, out var contentType))
            {
                contentType = "application/octet-stream";  // default media type for arbitrary binary data
            }

            var bytes = System.IO.File.ReadAllBytes(pathToFile);
            return File(bytes, contentType, Path.GetFileName(pathToFile));
        }
    }
}
