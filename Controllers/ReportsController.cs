using HairSalon.Data;
using HairSalon.Dtos;
using HairSalon.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

namespace HairSalon.Controllers
{
    [Authorize(Roles = "Admin, SuperAdmin")]
    public class ReportsController : Controller
    {
        private readonly AppDbContext _dbContext;

        public ReportsController(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public IActionResult GenerateReport(DateTime fromDate, DateTime toDate)
        {
            var appointments = _dbContext.AllAppointments
                .Include(appointment => appointment.OriginalNewAppointment)
                .Where(appointment => appointment.OriginalNewAppointment.AppointmentDate >= fromDate && appointment.OriginalNewAppointment.AppointmentDate <= toDate)
                .Select(appointment => new AppointmentDto
                {
                    Id = appointment.OriginalNewAppointment.Id,
                    CustomerName = appointment.OriginalNewAppointment.CustomerName,
                    CustomerSurName = appointment.OriginalNewAppointment.CustomerSurname,
                    AppointmentDate = appointment.OriginalNewAppointment.AppointmentDate,
                    Status = appointment.OriginalNewAppointment.Status
                })
                .ToList();

            var reportsViewModel = new ReportsViewModel
            {
                FromDate = fromDate,
                ToDate = toDate,
                Appointments = appointments
            };

            return RedirectToAction("GetReports", reportsViewModel);
        }

        [HttpGet]
        public IActionResult GetReports(ReportsViewModel model)
        {
           
            return View(model);
        }



    }
}
