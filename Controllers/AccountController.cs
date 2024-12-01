using HairSalon.Data;
using HairSalon.Models;
using HairSalon.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Reflection.Metadata.Ecma335;
using System.Threading.Tasks;

namespace HairSalon.Controllers
{
    public class AccountController : Controller
    {
        private UserManager<ApplicationUser> _userManager;
        private SignInManager<ApplicationUser> _signInManager;
        private RoleManager<IdentityRole> _roleManager;
        private IEmailSenders _emailSender;
        public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, RoleManager<IdentityRole> roleManager, IEmailSenders emailSender)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _emailSender = emailSender;
        }
        [AllowAnonymous]
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var defaultRole = "User1"; //Default olaraq qeydiyyatdan keçən istifadəçilərə verilən rol
                var user = new ApplicationUser
                {
                    UserName = model.UserName,
                    Email = model.Email
                };

                var res = await _userManager.CreateAsync(user, model.Password); //Hash olunma burda bash verir
                await _userManager.AddToRoleAsync(user, defaultRole);

                if (res.Succeeded)
                {
                    var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);

                    var url = Url.Action("ConfirmEmail", "Account", new { user.Id, token });

                    await _emailSender.SendEmailAsync(user.Email, "Hesabın təsdiqi", $"Xahiş edirik hesabınızı təsdiq etməyiniz üçün üçün linkə <a href='https://localhost:44370{url}'>klikləyin.</a>");

                    TempData["message"] = "Email hesabınıza gələn təsdiq mailini klikləyin";

                    ModelState.Clear();
                    return View();
                }
                foreach (IdentityError err in res.Errors)
                {
                    ModelState.AddModelError("", err.Description);
                }

            }
            return View(model);
        }

        [AllowAnonymous]
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(model.Email);

                if (user != null)
                {
                    await _signInManager.SignOutAsync();

                    if (!await _userManager.IsEmailConfirmedAsync(user))
                    {
                        ModelState.AddModelError("", "Hesabınızı təsdiq edin!");
                        return View(model);
                    }
                    var res = await _signInManager.PasswordSignInAsync(user, model.Password, model.RememberMe, true);

                    if (res.Succeeded)
                    {
                        await _userManager.ResetAccessFailedCountAsync(user);
                        await _userManager.SetLockoutEndDateAsync(user, null);

                        if (await _userManager.IsInRoleAsync(user, "SuperAdmin"))
                        {
                            return RedirectToAction("Index", "Dashboard");
                        }
                        else if (await _userManager.IsInRoleAsync(user, "Admin"))
                        {
                            return RedirectToAction("Index", "Dashboard");
                        }
                        else
                        {
                            return RedirectToAction("Index", "Home");
                        }
                    }
                    else if (res.IsLockedOut)
                    {
                        var lockOutDate = await _userManager.GetLockoutEndDateAsync(user);
                        var leftTime = lockOutDate.Value - DateTime.UtcNow;
                        ModelState.AddModelError("", $"Hesabınız bloklandı, xahiş edirik {leftTime.Minutes} sonra təkrar sınayın");
                    }
                    else
                    {
                        ModelState.AddModelError("", "Yanlış email ünvanı və ya şifrə");
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Belə bir hesab mövcud deyil");
                }
            }
            return View(model);
        }


        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Login", "Account");
        }

        public async Task<IActionResult> ConfirmEmail(string Id, string token)
        {
            if (Id == null || token == null)
            {
                TempData["warning-message"] = "Etibarsız token bilgisi";
                return View();
            }
            var user = await _userManager.FindByIdAsync(Id);

            if (user != null)
            {
                var result = await _userManager.ConfirmEmailAsync(user, token);

                if (result.Succeeded)
                {
                    TempData["warning-message"] = "Hesabınız təsdiqləndi";
                    return View();
                }

            }
            TempData["warning-message"] = "Istifadəçi tapıla bilmədi";
            return View();
        }

        [HttpGet]
        public IActionResult ForgotPassword()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> ForgotPassword(string Email)
        {
            if (string.IsNullOrEmpty(Email))
            {
                TempData["warning-message"] = "Email adresi yazın.";
                return View();
            }
            var user = await _userManager.FindByEmailAsync(Email);
            if (user == null)
            {
                TempData["warning-message"] = "Bu email adresi ilə uzlaşan bir adres mövcud deyil.";
                return View();
            }
            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var url = Url.Action("ResetPassword", "Account", new { user.Id, token });
            await _emailSender.SendEmailAsync(Email, "Şifrə Sıfırlama", $"Şifrənizi yeniləmək üçün linkə <a href='https://localhost:44370{url}'>klikləyin.</a> ");

            TempData["warning-message"] = "Email adresinizə gələn link ilə şifrənizi sıfırlaya bilərsiniz.";

            return View();


        }
        public IActionResult ResetPassword(string Id, string token)
        {
            if(Id == null || token == null)
            {
                return RedirectToAction("Login");
            }
            var model = new ResetPasswordViewModel { Token = token };
            return View(model);

        }
        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if(ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(model.Email);

                if(user==null)
                {
                      TempData["reset-alert-message"] = "Bu mail adresiylə uzlaşan istifadəçi tapıla bilmədi";
                    return View();
                }
                var result = await _userManager.ResetPasswordAsync(user, model.Token, model.Password);

                if(result.Succeeded)
                {
                    TempData["reset-alert-message"] = "Şifrəniz dəyişdirildi";
                    return View();
                }
                foreach(IdentityError err in result.Errors)
                {
                    ModelState.AddModelError("", err.Description);
                }
            }
            return View(model);
        }
    }
}



