using System;
using Microsoft.AspNetCore.Mvc;
using second_api.Models;
using System.IO;

namespace second_api.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class fileUploadController : ControllerBase
    {
        public static IWebHostEnvironment _WebHostEnvironment;

        public fileUploadController(IWebHostEnvironment WebHostEnvironment)
        {
            _WebHostEnvironment = WebHostEnvironment;
        }

        [HttpPost]
        public async Task<string> post([FromForm] fileUpload fileUpload)
        {
            try
            {
                if (fileUpload.files.Length > 0)
                {
                    string path = _WebHostEnvironment.WebRootPath + "\\Upload\\";
                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }
                    using (FileStream fileStream = System.IO.File.Create(path + fileUpload.files.FileName))
                    {
                        fileUpload.files.CopyTo(fileStream);
                        fileStream.Flush();
                        return "Upload Success";
                    }
                }
                else
                {
                    return "Upload Failed";
                }
            }
            catch (Exception ex)
            {
                return ex.Message.ToString();
            }
        }

        [HttpGet("{fileName}")]
        public async Task<IActionResult> Get([FromRoute] string fileName)
        {
            string path = _WebHostEnvironment.WebRootPath + "\\Upload\\";
            var filePath = path + fileName + ".png ";

            if(System.IO.File.Exists(filePath))
            {
                byte[] b = System.IO.File.ReadAllBytes(filePath);
               // return new FileContentResult(b, "image/jpeg");
            
                return File(b,"image/jpeg");
            }
            return null;
        }
    }
}
