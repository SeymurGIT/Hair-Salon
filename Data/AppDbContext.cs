using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using HairSalon.ViewModel;
using HairSalon.Models;

namespace HairSalon.Data
{
    public class AppDbContext : IdentityDbContext<ApplicationUser,IdentityRole,string >
    {
        public AppDbContext(DbContextOptions<AppDbContext>options):base(options)
        {
        }
         public DbSet<Service>Services { get; set; }
        public DbSet<NewAppointment> NewAppointments { get; set; }

        public DbSet<AcceptedAppointments> AcceptedAppointments {get;set;}

        public DbSet<AllAppointments> AllAppointments { get; set; }

        public DbSet<Subscriber> Subscribers { get; set; }

        public DbSet<Customer> Customers { get; set; }
        public DbSet<Contact> Contacts { get; set; }
        
        
    }

      
}
