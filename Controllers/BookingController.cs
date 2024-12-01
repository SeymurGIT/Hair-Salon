using HairSalon.Data;
using HairSalon.Models;
using HairSalon.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;
using System.Threading.Tasks;

namespace HairSalon.Controllers
{
    public class BookingController : Controller
    {
        private readonly AppDbContext _dbContext;
        private readonly IWebHostEnvironment _env;

        public BookingController(AppDbContext dbContext, IWebHostEnvironment env)
        {
            _dbContext = dbContext;
            _env = env;
        }
        [HttpGet]
        public IActionResult Index(string serviceName = null)
        {
            var availableServices = _dbContext.Services.Select(s => s.ServiceName).ToList();
            ViewBag.AllServices = availableServices;

            var newAppointment = new NewAppointment
            {
                AppointmentDate = DateTime.Today,
                AppointmentTime = new TimeSpan(9, 0, 0)
            };
            var selectedService = _dbContext.Services.FirstOrDefault(s => s.ServiceName == serviceName);

            if (selectedService != null)
            {
                ViewBag.SelectedService = selectedService; // Pass the selected service to the view
            }
            else
            {
                ViewBag.SelectedService = null; // Handle case where service is not found
            }
            ViewBag.SelectedServiceName = serviceName;

            return View(newAppointment);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult>Appoint(int id, NewAppointment model, List<string> selectedServices)
        {
            var newAppointment = _dbContext.NewAppointments.FirstOrDefault(s => s.Id == id);
            Customer customer = _dbContext.Customers.FirstOrDefault(c =>
            c.TheCustomerAppointment.CustomerName == model.CustomerName && c.TheCustomerAppointment.CustomerEmail == model.CustomerEmail);

            if (!ModelState.IsValid)
            {
                foreach (var modelState in ModelState.Values)
                {
                    foreach (var error in modelState.Errors)
                    {
                        ModelState.AddModelError("", "Problem oldu");   
                    }
                }
                ViewBag.AllServices = _dbContext.Services.Select(s => s.ServiceName).ToList();
                return View("Index", model);
            }
           

            var options = new JsonSerializerOptions
            {
                Encoder = JavaScriptEncoder.Create(UnicodeRanges.All),
                
            };
            var NoFalseServices = selectedServices.Where(service => service != "false").ToList();
            string selectedServicesJson = JsonSerializer.Serialize(NoFalseServices, options);

            List<string> selectedServicesList = JsonSerializer.Deserialize<List<string>>(selectedServicesJson);
            string combinedServices = string.Join(",",selectedServicesList);

            model.SelectedServices = combinedServices;
            model.Status = "Gözləməkdə";
            model.ApplyDate = DateTime.Now;
            model.QueueNumber = RandomQueueNumber();
       
            if (model.HairStyle != null && model.HairStyle.Length > 0)
            {
                string fileName = Guid.NewGuid().ToString() + Path.GetExtension(model.HairStyle.FileName);
                string folderPath = Path.Combine(_env.WebRootPath, "User", "images", "customer-appointment-images");
                string filePath = Path.Combine(folderPath, fileName);

                using (FileStream stream = new FileStream(filePath, FileMode.Create))
                {
                    await model.HairStyle.CopyToAsync(stream);
                }
                model.AppointmentImageLink = Path.Combine("User", "images", "customer-appointment-images", fileName);

            }
            if (customer == null)
            {
                customer = new Customer
                {
                    TheCustomerAppointment = model,
                    FirstAppliedDate = DateTime.Now,
                };
                _dbContext.Customers.Add(customer);
            }

            var allAppointments = new AllAppointments
            {
                OriginalNewAppointment = model
            };
            _dbContext.NewAppointments.Add(model);
            _dbContext.AllAppointments.Add(allAppointments);
            await _dbContext.SaveChangesAsync();

            return RedirectToAction("ThankYou", new { queueNumber = model.QueueNumber });
        }
        [HttpGet]
        public IActionResult ThankYou(string queueNumber)
        {
            ViewBag.QueueNumber = queueNumber;
            return View();
        }

        private string RandomQueueNumber()
        {
            Random random = new Random();
            int randomNumber = random.Next(100000000, 999999999);
            return randomNumber.ToString();
        }


}
    }
