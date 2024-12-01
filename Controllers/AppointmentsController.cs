using HairSalon.Data;
using HairSalon.Models;
using HairSalon.Repositories;
using HairSalon.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace HairSalon.Controllers
{
    [Authorize(Roles = "Admin, SuperAdmin")]
    public class AppointmentsController : Controller
    {
        private readonly AppDbContext _dbContext;
        private readonly AppointmentRepository _appointmentRepo;
        private readonly IWebHostEnvironment _env;
        private readonly IContentTypeProvider _provider;


        public AppointmentsController(AppDbContext dbContext, AppointmentRepository appointmentRepo, IWebHostEnvironment env, IContentTypeProvider provider)
        {
            _dbContext = dbContext;
            _appointmentRepo = appointmentRepo;
            _env = env;
            _provider = provider;

        }
        public IActionResult Index()
        {
            var newAppointments = _appointmentRepo.GetNewAppointments();   
            return View(newAppointments);
        }
        public IActionResult AllAppointments()
        {
            var allAppointments = _appointmentRepo.GetAllAppointments();
            return View(allAppointments);
        }

        [HttpGet]
        public IActionResult ManageAppointments(int id)
        {
            var obj = _dbContext.NewAppointments.FirstOrDefault(s => s.Id == id);

            if (obj != null)
            {
                AppointmentViewModel appointmentsViewModel = new AppointmentViewModel
                {
                    AppointmentObj = obj
                };
                return View(appointmentsViewModel);
            }
            else
            {
                return NotFound();
            }
        }
        [HttpPost]
        public async Task<IActionResult> ManageAppointments(int id, string status, AppointmentViewModel model)
        {

            //if (!ModelState.IsValid)
            //{
            //    return View(model);
            //}
            var newAppointment = _dbContext.NewAppointments.FirstOrDefault(s => s.Id == id);
            var allAppointment = _dbContext.AllAppointments.FirstOrDefault(a => a.NewAppointmentId == id);

            if (newAppointment == null || allAppointment == null)
            {
                return NotFound();
            }
            if (newAppointment.Status == "Təsdiq olunmuş" && status == "Təsdiq olunmuş")
            {
                return RedirectToAction("Index");
            }
            if ((newAppointment.Status == "Təsdiq olunmuş" && status=="Imtina olunmuş" || (newAppointment.Status == "Təsdiq olunmuş" && status=="Gözləməkdə")))
            {
                var alreadyAccepted = _dbContext.AcceptedAppointments.FirstOrDefault(s => s.NewAppointmentId == id);

                if (alreadyAccepted != null)
                {
                    _dbContext.AcceptedAppointments.Remove(alreadyAccepted);
                }
                await _dbContext.SaveChangesAsync();
                return RedirectToAction("AllAppointments");
            }
            newAppointment.Status = status;
            allAppointment.OriginalNewAppointment.Status = status;

            if (status == "Təsdiq olunmuş")
            {
                var alreadyAccepted = _dbContext.NewAppointments.FirstOrDefault(s=>s.Id == id);
                var acceptedAppointment = new AcceptedAppointments
                {
                    NewAppointmentId = newAppointment.Id,
                    OriginalAppointment = newAppointment,
                    AcceptedDate = DateTime.Now
                 };
                    _dbContext.AcceptedAppointments.Add(acceptedAppointment);
                    await _dbContext.SaveChangesAsync();
                    return RedirectToAction("AcceptedAppointments");
            }
  
            if(status== "İmtina olunmuş")
            {
                newAppointment.Status = status;
                await _dbContext.SaveChangesAsync();
                return RedirectToAction("AllAppointments");
            }
            await _dbContext.SaveChangesAsync();
            return RedirectToAction("Index", "Appointments");
        }
        [HttpGet]
        public IActionResult AcceptedAppointments()
        {
            var acceptedAppointments = _dbContext.AcceptedAppointments
            .Include(a => a.OriginalAppointment)
            .ToList();

            return View(acceptedAppointments);
        }
        [HttpGet]
        public IActionResult Invoice(int id)
        {
            var accepted = _dbContext.AcceptedAppointments
            .Include(a=>a.OriginalAppointment).FirstOrDefault(s => s.OriginalAppointment.Id == id);

            if(accepted!=null)
            {
                var selectedServices = accepted.OriginalAppointment.SelectedServices.Split(',');

                var servicePrices = _dbContext.Services
                .Where(s => selectedServices.Contains(s.ServiceName))
                .ToDictionary(s => s.ServiceName, s => s.ServicePrice);

                AcceptedAppointmentViewModel appointmentsViewModel = new AcceptedAppointmentViewModel
                {
                    AcceptedAppointmentObj = accepted.OriginalAppointment,
                     ServicePrices = servicePrices

                };

                return View(appointmentsViewModel);
            }
            else
            {
                return NotFound();
            }
    }
        [HttpGet]
        public IActionResult Search(string q)
        {
            if (string.IsNullOrWhiteSpace(q))
            {
                var appointments = _appointmentRepo.GetAllAppointments();
                return View("AllAppointments", appointments);
            }
            else
            {
                var filteredAppointments = _appointmentRepo.GetAllAppointments()
                    .Where(i => i.OriginalNewAppointment.QueueNumber.Contains(q))
                    .ToList();
                return View("AllAppointments", filteredAppointments);
            }
        }
        [HttpGet]
        public async Task<IActionResult> DownloadImage(string imagePath)
        {
            if (string.IsNullOrEmpty(imagePath))
            {
                return NotFound();
            }

            var imageFilePath = Path.Combine(_env.WebRootPath, "User", "images", "customer-appointment-images", imagePath);

            if (!System.IO.File.Exists(imageFilePath))
            {
                return NotFound();
            }

            var fileBytes = await System.IO.File.ReadAllBytesAsync(imageFilePath);
            string contentType;
            _provider.TryGetContentType(imagePath, out contentType);

  
            if (string.IsNullOrEmpty(contentType))
            {
                contentType = "application/octet-stream";
            }

            return File(fileBytes, contentType, Path.GetFileName(imageFilePath));
        }



    }
}
