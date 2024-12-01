using HairSalon.Data;
using HairSalon.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace HairSalon.Repositories
{
    public class AppointmentRepository
    {
        private AppDbContext _dbContext;

        public AppointmentRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public List<NewAppointment> GetNewAppointments()
        {
            return _dbContext.NewAppointments
            .OrderByDescending(a=> a.ApplyDate).ToList();
        }
        public List<AllAppointments> GetAllAppointments()
        {
            var appointments = _dbContext.AllAppointments
            .Include(a => a.OriginalNewAppointment) // Eager loading
            .OrderByDescending(a => a.OriginalNewAppointment.ApplyDate)
            .ToList();

            return appointments;
        }
    }
}
