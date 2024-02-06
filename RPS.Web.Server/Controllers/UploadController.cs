using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace RPS.Web.Server.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class UploadController : ControllerBase
    {
        public IWebHostEnvironment HostingEnvironment { get; set; }

        public UploadController(IWebHostEnvironment he) 
        { 
            HostingEnvironment = he;
        }

        [HttpPost]
        public async Task<IActionResult> Save(IFormFile file) // must match SaveField
        {
                try
                {
                    var fileContent = ContentDispositionHeaderValue.Parse(file.ContentDisposition);

                    var fileName = Path.GetFileName(fileContent.FileName.ToString().Trim('"'));

                    var physicalPath = Path.Combine(HostingEnvironment.WebRootPath, fileName);

                    using (var fileStream = new FileStream(physicalPath, FileMode.Create))
                    {
                        await file.CopyToAsync(fileStream);
                    }
                }
                catch
                {
                    Response.StatusCode = 500;
                    await Response.WriteAsync("File save failed");
                }

            return new EmptyResult();

        }
    }
}
