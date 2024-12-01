using HairSalon.Models;
using HairSalon.Repositories;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace HairSalon.Controllers
{
    public class ServiceController : Controller
    {
    private readonly IWebHostEnvironment _webHostEnvironment;
        private ServiceRepository _serviceRepository;

        public ServiceController(IWebHostEnvironment webHostEnvironment,ServiceRepository serviceRepository)
        {
            _webHostEnvironment = webHostEnvironment;
            _serviceRepository = serviceRepository;
        }
        [HttpGet]
        public IActionResult Index()
        {
            List<Service> services = _serviceRepository.GetAllServices();

            services = services.ToList();
            return View(services);
        }
        public IActionResult DownloadWord()
        {
            // Get the file path to the PDF file in the "wwwroot" directory
            string filePath = Path.Combine(_webHostEnvironment.WebRootPath, "User/files/Xidmetler.docx");

            // Check if the file exists
            if (System.IO.File.Exists(filePath))
            {
                // Read the file into a byte array
                byte[] fileBytes = System.IO.File.ReadAllBytes(filePath);

                // Set the content type to "application/pdf"
                const string contentType = "application/vnd.openxmlformats-officedocument.wordprocessingml.document";

                // Create a file content result
                FileContentResult result = new FileContentResult(fileBytes, contentType)
                {
                    FileDownloadName = "xidmetlerimiz.docx" // The name that users will see when downloading
                };

                return result;
            }
            else
            {
                return NotFound();
            }
        }
    }
}
