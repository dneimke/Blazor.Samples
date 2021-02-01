using Blazor.Samples.Core.Files;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;

namespace Blazor.Samples.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImagesController : ControllerBase
    {
        [HttpGet("submit")]
        public IActionResult Submit()
        {
            return Ok("Test");
        }

        [HttpPost("paste/save")]
        public IActionResult SavePastedImage([FromForm] IFormFile pastedImage)
        {
            const int MaxFileSize = 2 * 1024 * 1024; // 2097152; 

            if (pastedImage.Length > MaxFileSize)
            {
                return BadRequest($"File is too big. Maximum size is {MaxFileSize} bytes.");
            }

            var bytes = ConvertToBytes(pastedImage);

            if (!FileValidator.IsValidFileExtension(pastedImage.FileName, bytes, Array.Empty<byte>()))
            {
                return BadRequest($"File data is not valid for the specified file format.");
            }


            return new JsonResult(new
            {
                // In a real-world situation, store the generated image somewhere - e.g. BlobStorage
                // and return the actual Url to it
                ImageUrl = "https://via.placeholder.com/150"
            });
        }

        private static byte[] ConvertToBytes(IFormFile pastedImage)
        {
            var stream = pastedImage?.OpenReadStream();
            using var memoryStream = new MemoryStream();
            stream.CopyTo(memoryStream);
            return memoryStream.ToArray();
        }
    }
}
