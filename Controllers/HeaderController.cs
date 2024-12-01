using HairSalon.Data;
using HairSalon.Models;
using HairSalon.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace HairSalon.Controllers
{
    [Authorize(Roles = "Admin, SuperAdmin")]
    public class HeaderController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly AppDbContext _dbContext;
        
        public HeaderController(UserManager<ApplicationUser> userManager, AppDbContext dbContext)
        {
            _userManager = userManager;
            _dbContext = dbContext;
            
        }
        public IActionResult _adminHeader()
        {
            //var currentUser = await _userManager.GetUserAsync(User);
            DateTime today = DateTime.Today;
            int newNotificationsCount = _dbContext.NewAppointments.Count(appointment => appointment.ApplyDate.Date == today);

            var headerViewModel = new HeaderViewModel
            {
                NewAppointmentsCount = newNotificationsCount
            };

            ViewData["HeaderViewModel"] = headerViewModel;

            // Your other logic for the main view...

            return View();
        }
        //private async Task<HeaderViewModel> GetHeaderViewModelFromDatabase()
        //{
        //    var currentUser = await _userManager.GetUserAsync(User);
        //    DateTime today = DateTime.Today;
        //    int newNotificationsCount = _dbContext.NewAppointments.Count(appointment => appointment.ApplyDate.Date == today);

        //    HeaderViewModel headerViewModel = new HeaderViewModel
        //    {
        //        ImageLink = currentUser.UserImageLink,
        //        Username = currentUser.UserName,
        //    };
        //    return headerViewModel;
        //}
    }
}

