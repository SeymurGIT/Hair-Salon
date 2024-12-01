using HairSalon.Data;
using HairSalon.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace HairSalon.Controllers
{
    public class SubscriptionController : Controller
    {
        private AppDbContext _dbContext;

        public SubscriptionController(AppDbContext dbContext)
        {
            _dbContext = dbContext; 
        }
        [HttpPost]
        public async Task<IActionResult> Subscribe(Subscriber model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var existingSubscriber = await _dbContext.Subscribers.FirstOrDefaultAsync(s => s.EmailAddress == model.EmailAddress);

            if (existingSubscriber != null)
            {
                TempData["failed"] = "Bu email ilə artıq abunə olunub";
                return RedirectToAction("Index", "Home");
                //ModelState.AddModelError("", "Bu email ilə artıq abunə olunub");
                //return BadRequest(ModelState);
            }
            TempData["success"] = "Abunə olduğunuz üçün təşəkkür edirik";
            _dbContext.Subscribers.Add(model);
            await _dbContext.SaveChangesAsync();
            return RedirectToAction("Index", "Home");
        }
    }
}
