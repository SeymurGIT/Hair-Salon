using Castle.Core.Smtp;
using HairSalon.Data;
using HairSalon.Models;
using HairSalon.Repositories;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HairSalon
{
    public class Startup
    {
        private IConfiguration _config;
        
        public Startup(IConfiguration config)
        {
            _config = config;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddScoped<IEmailSenders,SmtpEmailSender>(i =>

            new SmtpEmailSender(
               
                _config["EmailSender:Host"],
                _config.GetValue<int>("EmailSender:Port"),
                _config.GetValue<bool>("EmailSender:EnableSSL"),
                _config["EmailSender:Username"],
                _config["EmailSender:Password"])
            );
            services.AddScoped<ServiceRepository>(); //Servisler
            services.AddScoped<AppointmentRepository>(); //Rezervler
            services.AddScoped<CustomerRepository>(); //Musteriler
            services.AddControllersWithViews()
            .AddNewtonsoftJson(options =>
            {
                options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
            });

            services.AddMvc()
            .AddSessionStateTempDataProvider()
            .AddNewtonsoftJson();
            services.AddDbContext<AppDbContext>(options =>
            //services.AddScoped<IEmailSender, YourEmailSenderImplementation>();
            {
                options.UseSqlServer(_config.GetConnectionString("Default"), opt=>
                {
                    opt.CommandTimeout(180);
                });
            });
            services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(30);
            });
            var provider = new FileExtensionContentTypeProvider();
            provider.Mappings[".jpg"] = "image/jpeg";
            provider.Mappings[".jpeg"] = "image/jpeg";
            provider.Mappings[".png"] = "image/png";
            services.AddSingleton<IContentTypeProvider>(provider);
            
            services.ConfigureApplicationCookie(options =>
            {
                options.LoginPath = "/Account/Login";
                options.LogoutPath = "/Account/Login";
                options.AccessDeniedPath = "/Account/AccessDenied";
                options.SlidingExpiration = true;
                options.ExpireTimeSpan = TimeSpan.FromDays(12);
               
                

            });

            services.AddIdentity<ApplicationUser,IdentityRole>(options =>
            {
                options.User.RequireUniqueEmail = true;
                options.Password.RequireUppercase = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireDigit = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(3);
                options.Lockout.MaxFailedAccessAttempts = 6;
                options.SignIn.RequireConfirmedEmail = true;
            })
                .AddEntityFrameworkStores<AppDbContext>()
                .AddDefaultTokenProviders();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            var provider = app.ApplicationServices.GetRequiredService<IContentTypeProvider>();
            app.UseStaticFiles(new StaticFileOptions
            {
                ContentTypeProvider = provider
            }); ;
            app.UseRouting();
            app.UseSession();
            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}"
                );
                endpoints.MapControllerRoute(
                    name: "adminDashboard",
                    pattern: "{controller=Dashboard}/{action=Index}/{id?}"
                );
                endpoints.MapControllerRoute(
                    name: "adminServices",
                    pattern: "{controller=Services}/{action=ManageServices}/{id?}"

                );
                endpoints.MapControllerRoute(
                    name: "adminAddServices",
                    pattern: "{controller=Services}/{action=AddServices}/{id?}"

                );

                endpoints.MapControllerRoute(
                   name: "adminEditServices",
                   pattern: "{controller=Services}/{action=EditServices}/{id?}"

               );
                endpoints.MapControllerRoute(
                    name: "adminUsers",
                    pattern: "{controller=Admin}/{action=Index}/{id?}"
                );

                endpoints.MapControllerRoute(
                    name: "adminCreateUsers",
                    pattern: "{controller=Admin}/{action=CreateUser}/{id?}"
                );

                endpoints.MapControllerRoute(
                   name: "adminEditUsers",
                   pattern: "{controller=Admin}/{action=EditUser}/{id?}"
               );
                endpoints.MapControllerRoute(
                    name: "RoleLists",
                    pattern: "{controller=Roles}/{action=Index}/{id?}"
                );

                endpoints.MapControllerRoute(
                    name: "adminCreateRoles",
                    pattern: "{controller=Roles}/{action=CreateRole}/{id?}"
                );

                endpoints.MapControllerRoute(
                    name: "adminAppointments",
                    pattern: "{controller=Appointments}/{action=Index}/{id?}"
                );

                endpoints.MapControllerRoute(
                    name: "servicesRoute",
                    pattern: "{controller=Service}/{action=Index}/{id?}"
                );

                endpoints.MapControllerRoute(
                    name: "userRoute",
                    pattern: "account/{action=Register}/{id?}",
                    defaults: new { controller = "Account" }
                );

          
                endpoints.MapControllerRoute(
                    name: "appointment",
                    pattern: "Rezerv",
                    defaults: new { controller = "Booking", action = "Index" }
                );

                // Map the root controllers
            });






        }
    }
}
