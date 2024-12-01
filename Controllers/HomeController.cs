using Castle.Core.Smtp;
using HairSalon.Data;
using HairSalon.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace HairSalon.Controllers
{
  
    public class HomeController : Controller
    {

        //private readonly ILogger<HomeController> _logger;
        private AppDbContext _dbContext;
        

        public HomeController(AppDbContext dbContext)
        {
     
            _dbContext = dbContext;
        }

        public IActionResult Index()
        {
            return View();
        }
        

        public IActionResult Contact()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Contact(Contact contact)
        {
            if (ModelState.IsValid)
            {
                _dbContext.Contacts.Add(contact);
                await _dbContext.SaveChangesAsync();
                ViewBag.SuccessMessage = "Mesajınız uğurlu şəkildə göndərildi";
                return View("Contact");
            }
            
            return View(contact);
        }
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
