using HairSalon.Data;
using HairSalon.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace HairSalon.Controllers
{
    [Authorize(Roles = "Admin, SuperAdmin")]
    public class CustomersController : Controller
    {
        private readonly AppDbContext _dbContext;
        private readonly CustomerRepository _customerRepo;

        public CustomersController(CustomerRepository customerRepo, AppDbContext dbContext)
        {
            _dbContext = dbContext;
            _customerRepo = customerRepo;
        }
        public IActionResult Index()
        {
            var allCustomers = _customerRepo.GetAllCustomers();
            return View(allCustomers);
        }
        [HttpGet]
        public IActionResult SearchCustomer(string q)
        {
            if (string.IsNullOrWhiteSpace(q))
            {
                var customer = _customerRepo.GetAllCustomers();
                return View("Index", customer);
            }
            else
            {
                var foundCustomers = _customerRepo.GetAllCustomers()
                .Where(i => i.TheCustomerAppointment.CustomerName.Contains(q))
                .ToList();
                return View("Index", foundCustomers);
            }
        }
    }
}
