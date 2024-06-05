﻿using Microsoft.AspNetCore.Mvc;
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

        [HttpPost]
        public async Task<ActionResult> CreateFileAsync(IFormFile file)
        {
            // Validate the input. Put a limit on filesize to avoid large uploads attacks. 
            // Only accept .pdf files (check content-type)
            if (file.Length == 0 || file.Length > 20971520 || file.ContentType != "application/pdf")
            {
                return BadRequest("No file or an invalid one has been inputted.");
            }

            // Create the file path.  Avoid using file.FileName, as an attacker can provide a
            // malicious one, including full paths or relative paths.  
            var path = Path.Combine(
                Directory.GetCurrentDirectory(),
                $"uploaded_file_{Guid.NewGuid()}.pdf");

            using (var stream = new FileStream(path, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            return Ok("Your file has been uploaded successfully.");
        }
    }
}
