using HairSalon.Data;
using HairSalon.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace HairSalon.Repositories
{
    public class CustomerRepository
    {
        private AppDbContext _dbContext;

        public CustomerRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public List<Customer> GetAllCustomers()
        {
            var customers = _dbContext.Customers
             .Include(a => a.TheCustomerAppointment) 
             .OrderByDescending(a => a.TheCustomerAppointment.ApplyDate)
             .ToList();

            return customers;
        }
    }
}
