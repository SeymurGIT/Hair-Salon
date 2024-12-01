using HairSalon.Data;
using HairSalon.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;

namespace HairSalon.Controllers
{

    public class DashboardController : Controller
    {
        private readonly AppDbContext _dbContext;

        public DashboardController(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IActionResult Index()
        {
            var totalCountsViewModel = GetTotalCountsViewModelFromDatabase();
            ViewData["TotalCountsViewModel"] = totalCountsViewModel;
            return View();
        }
        private TotalCountsViewModel GetTotalCountsViewModelFromDatabase()
        {
            var today = DateTime.Today;
            var yesterday = today.AddDays(-1);
            var weekAgo = today.AddDays(-7);
            var WeeksAppointments = _dbContext.AllAppointments
            .Where(a => a.OriginalNewAppointment.ApplyDate >= weekAgo && a.OriginalNewAppointment.ApplyDate < DateTime.Today)
    .ToList();

            var totalCountsViewModel = new TotalCountsViewModel
            {
                TotalServices = _dbContext.Services.Count(),
                TotalAppointments = _dbContext.AllAppointments.Count(),
                TotalAcceptedAppointments = _dbContext.AcceptedAppointments.Count(),
                TodaysAppointments = _dbContext.NewAppointments.Count(a => a.ApplyDate == today),
                WeeksAppointments = _dbContext.AllAppointments
         .Count(a => a.OriginalNewAppointment.ApplyDate >= weekAgo && a.OriginalNewAppointment.ApplyDate < today),

                TotalRejectedAppointments = _dbContext.AllAppointments.Count(a => a.OriginalNewAppointment.Status == "Imtina olunmuş"),

                TotalSubscribers = _dbContext.Subscribers.Count(),

                YesterdaysAppointments = _dbContext.NewAppointments.Count(a => a.ApplyDate == yesterday)
            };
            return totalCountsViewModel;
        }
    }
}
