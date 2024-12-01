using HairSalon.Data;
using HairSalon.Models;
using HairSalon.ViewModel;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.IO;
using System;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;
using HairSalon.Repositories;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Authorization;

namespace HairSalon.Controllers
{


    public class ServicesController : Controller
    {
        private AppDbContext _dbContext;
        private IWebHostEnvironment _env;
        private readonly ServiceRepository _serviceRepo;
        

        public ServicesController(AppDbContext dbContext, 
               IWebHostEnvironment env, ServiceRepository serviceRepo)
        {
            _dbContext = dbContext;
            _env = env;
            _serviceRepo = serviceRepo;
        }

        [HttpGet]
        public IActionResult AddServices()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> AddServices(Service model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            if (model.Image != null && model.Image.Length > 0)
            {
                string fileName = Guid.NewGuid().ToString() + Path.GetExtension(model.Image.FileName);
                string folderPath = Path.Combine(_env.WebRootPath, "Admin", "images", "service-images");
                string filePath = Path.Combine(folderPath, fileName);

                using (FileStream stream = new FileStream(filePath, FileMode.Create))
                {
                    await model.Image.CopyToAsync(stream);
                }

                model.ServiceImageLink = "/Admin/images/service-images/" + fileName;
            }
            _dbContext.Services.Add(model);
            await _dbContext.SaveChangesAsync();
            return RedirectToAction("ManageServices", "Services");
        }

        [HttpGet]
        public IActionResult ManageServices()
        {
            List<Service> services = _serviceRepo.GetAllServices();
            return View(services);  
        }
        [HttpGet]
        public IActionResult EditServices(int id)
        {
            var obj = _dbContext.Services.FirstOrDefault(s=> s.ServiceId == id);

            if(obj != null)
            {
                ServicesViewModel servicesViewModel = new ServicesViewModel
                {
                    ServiceObj = obj
                   
                };
                return View(servicesViewModel);
            }
            else
            {

            return NotFound();
            }
        }
        [HttpPost]
        public async Task<IActionResult>EditServices(int id, ServicesViewModel model)
        {
            var obj = _dbContext.Services.FirstOrDefault(s=>s.ServiceId== id);

            if (!ModelState.IsValid)
            {
                return View(model);
            }
            if (obj != null)
            {
                obj.ServiceName = model.ServiceObj.ServiceName;
                obj.ServiceDescription = model.ServiceObj.ServiceDescription;
                obj.ServicePrice = model.ServiceObj.ServicePrice;
                obj.ServiceImageLink = model.ServiceObj.ServiceImageLink;
                await _dbContext.SaveChangesAsync();


                if (model.ServiceObj.Image != null && model.ServiceObj.Image.Length > 0)
                {
                    string folderPath = Path.Combine(_env.WebRootPath, "Admin", "images", "service-images");
                    if (!Directory.Exists(folderPath))
                    {
                        Directory.CreateDirectory(folderPath);
                    }
                    string fileName = Guid.NewGuid().ToString() + Path.GetExtension(model.ServiceObj.Image.FileName);
                    string filePath = Path.Combine(folderPath, fileName);

                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await model.ServiceObj.Image.CopyToAsync(fileStream);
                    }
                    obj.ServiceImageLink = "/Admin/images/service-images/" + fileName;
                }
                await _dbContext.SaveChangesAsync();
            }
            return RedirectToAction("ManageServices", "Services");
        }
        [HttpGet]
        public IActionResult DetailService(int id)
        {
            var obj = _dbContext.Services.FirstOrDefault(s => s.ServiceId == id);

            if(obj!=null)
            {
                return View("ManageServices");
            }
            else
            {
                return NotFound();
            }
        }
        [HttpPost]
        public async Task<IActionResult> DeleteService(int id)
        {
            var obj = await _dbContext.Services.FirstOrDefaultAsync(s => s.ServiceId == id);

            if(obj!=null)
            {
                _dbContext.Services.Remove(obj);
                await _dbContext.SaveChangesAsync();
                return RedirectToAction("ManageServices");
            }
            else
            {
                return NotFound();
            }
        }
       
    }
}

