using HairSalon.Data;
using HairSalon.Models;
using System.Collections.Generic;
using System.Linq;

namespace HairSalon.Repositories
{
    public class ServiceRepository
    {
        private AppDbContext _dbContext;

        public ServiceRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public List<Service> GetAllServices()
        {
            return _dbContext.Services.ToList();
        }
        //public Service FindById(int id)
        //{
           
        //}

    }
}
