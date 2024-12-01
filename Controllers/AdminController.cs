using HairSalon.Infrastructure;
using HairSalon.Models;
using HairSalon.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.IO;
using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;

namespace HairSalon.Controllers {


    public class AdminController : Controller
    {
        private UserManager<ApplicationUser> _userManager;
        private RoleManager<IdentityRole> _roleManager;
        private IWebHostEnvironment _env;
        //private IPasswordValidator<IdentityUser> _passwordValidator;
        //private IPasswordHasher<IdentityUser> _passwordHasher;

        public AdminController(UserManager<ApplicationUser> userManager, 
        IPasswordValidator<ApplicationUser> passwordValidator, 
        IPasswordHasher<ApplicationUser> passwordHasher, RoleManager<IdentityRole> roleManager, IWebHostEnvironment env)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _env = env;
        }
        //public IActionResult Index()
        //{
        //    //var adminUsers = await _userManager.GetUsersInRoleAsync("Admin");
        //    return View(adminUsers);
        //}

        public IActionResult Index()
        {
            return View(_userManager.Users);
        }
        [HttpGet]
        public IActionResult CreateUser()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> CreateUser(CreateUserViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser
                {
                    UserName = model.UserName,
                    Email = model.Email,
                    FullName = model.FullName,
                    
                    
                };
                if (model.Image != null && model.Image.Length > 0)
                {
                    string fileName = Guid.NewGuid().ToString() + Path.GetExtension(model.Image.FileName);
                    string folderPath = Path.Combine(_env.WebRootPath, "Admin", "images", "user-images");
                    string filePath = Path.Combine(folderPath, fileName);

                    using (FileStream stream = new FileStream(filePath, FileMode.Create))
                    {
                        await model.Image.CopyToAsync(stream);
                    }

                    user.UserImageLink = "/Admin/images/user-images/" + fileName;
                }

                IdentityResult result = await _userManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    return RedirectToAction("Index", "Admin");
                }
                foreach (IdentityError error in result.Errors)
                {
                        ModelState.AddModelError("",error.Description);
                }
            }

            return View(model);
        }
        [HttpGet]
        public async Task<IActionResult>EditUser(string id)
        {
            if(id==null)
            {
                return RedirectToAction("Index","Admin");
            }
            var user = await _userManager.FindByIdAsync(id); 
            
            if(user!=null)
            {
                //Database-den bütün rolları gətirmək
                ViewBag.Roles = await _roleManager.Roles.Select(s => s.Name).ToListAsync();
                return View(new EditViewModel
                {

                    Id = user.Id,
                    FullName = user.FullName,
                    Email = user.Email,
                    SelectedRoles = await _userManager.GetRolesAsync(user),
                    
                }) ;
            }
            //ViewBag(mesaj)
            return RedirectToAction("Index", "Admin");
        }
        [HttpPost]
        public async Task<ActionResult> EditUser(string id, EditViewModel model)
        {
            if(id!=model.Id)
            {
                return RedirectToAction("Index", "Admin");
            }
            if(ModelState.IsValid)
            {
                var user = await _userManager.FindByIdAsync(model.Id);

                if(user!=null )
                {
                    user.Email = model.Email;
                    user.FullName = model.FullName;
                    
                    var res = await _userManager.UpdateAsync(user);

                    //Parol update olunmasi

                    if(!string.IsNullOrEmpty(model.Password) && res.Succeeded)
                    {
                        await _userManager.RemovePasswordAsync(user);
                        await _userManager.AddPasswordAsync(user, model.Password);
                    }

                    if(res.Succeeded)
                    {
                        //İstifadəçidə mövcud olan rolları silirik
                        await _userManager.RemoveFromRolesAsync(user, await _userManager.GetRolesAsync(user));

                        if(model.SelectedRoles!=null) //Rol bilgisi varsa
                        {
                        //İstifadəçiyə yeni rol/rollar promote edirik
                        await _userManager.AddToRolesAsync(user, model.SelectedRoles);  
                        }
                        return RedirectToAction("Index", "Admin");
                    }
                    foreach(var item in res.Errors)
                    {
                        ModelState.AddModelError("", item.Description);
                    }
                }
            }
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteUser(string id)
        {
            var user=await _userManager.FindByIdAsync(id);

            if(user!=null)
            {
                await _userManager.DeleteAsync(user);
            }
            return RedirectToAction("Index", "Admin");
        }

        public IActionResult AdminProfile()
        {
            return View();
        }
    }

}

