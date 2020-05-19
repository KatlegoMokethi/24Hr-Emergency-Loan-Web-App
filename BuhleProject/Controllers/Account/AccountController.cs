using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using BuhleProject.Models.Data;
using BuhleProject.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace BuhleProject.Controllers.Account
{
    [Authorize]
    public class AccountController : Controller
    {
        private UserManager<IdentityUser> _userManager;
        private SignInManager<IdentityUser> _signInManager;
        private RoleManager<IdentityRole> _roleManager;
        private IRepositoryWrapper context;
        private string _role;
        private IConfiguration _configuration;
        public AccountController(IRepositoryWrapper dBcontext, UserManager<IdentityUser> userManager,
        SignInManager<IdentityUser> signInManager,
        RoleManager<IdentityRole> roleManager, IConfiguration _configuration)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            context = dBcontext;
            this._configuration = _configuration;
        }

        [AllowAnonymous]
        public IActionResult Login(string returnUrl)
        {
            return View(new LoginViewModel
            {
                ReturnUrl = returnUrl
            });
        }

        [AllowAnonymous]
        public IActionResult ViewTerms()
        {
            string path = @"D:\home\site\wwwroot\wwwroot\final terms and conditions.pdf";
            return new PhysicalFileResult(path, "application/pdf");
        }

        [AllowAnonymous]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel loginModel)
        {
            if (ModelState.IsValid)
            {
                IdentityUser user =
                await _userManager.FindByEmailAsync(loginModel.Email);

                if (user == null)
                {
                    user =
                await _userManager.FindByNameAsync(loginModel.Email);
                }
                if (user != null)
                {

                    var result = await _signInManager.PasswordSignInAsync(user,
                    loginModel.Password, false, false);
                    if (result.Succeeded)
                    {

                        if (loginModel.Email.Contains("@"))
                            return Redirect("/Client/Loans");
                        else
                            return Redirect("/Admin/Applications");


                    }
                }
            }
            ModelState.AddModelError("", "Invalid username or password");
            return View(loginModel);
        }

        [AllowAnonymous]
        [HttpGet]
        public IActionResult Register()
        {
            // get reCAPTHCA key from appsettings.json
            ViewData["ReCaptchaKey"] = _configuration.GetSection("GoogleReCaptcha:key").Value;

            var v = new RegisterViewModel();
            return View(v);
        }


        //////////////////////////////////////////////////////////////////////////////
        [AllowAnonymous]
        public static bool ReCaptchaPassed(string gRecaptchaResponse, string secret)
        {
            HttpClient httpClient = new HttpClient();
            var res = httpClient.GetAsync($"https://www.google.com/recaptcha/api/siteverify?secret={secret}&response={gRecaptchaResponse}").Result;
            if (res.StatusCode != System.Net.HttpStatusCode.OK)
            {
                //must be false
                return true;
            }

            string JSONres = res.Content.ReadAsStringAsync().Result;
            dynamic JSONdata = Newtonsoft.Json.Linq.JObject.Parse(JSONres);
            if (JSONdata.success != "true")
            {
                //must be false
                return true;
            }

            return true;
        }
        //////////////////////////////////////////////////////////////////////////////
        //[Authorize(Roles = "Admins")]
        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel registerModel)
        {

            //get reCAPTHCA key from appsettings.json
            ViewData["ReCaptchaKey"] = _configuration.GetSection("GoogleReCaptcha:key").Value;

            if (ModelState.IsValid)
            {
                if (!ReCaptchaPassed(
                    Request.Form["g-recaptcha-response"], // that's how you get it from the Request object
                    _configuration.GetSection("GoogleReCaptcha:secret").Value
                    ))
                {
                    ModelState.AddModelError(string.Empty, " CAPTCHA failed");
                    return View(registerModel);
                }

                var uemail = context.UserRepository.FindByCondition(s => s.Email == registerModel.Email || s.Phone == registerModel.Phone);
                if (uemail.Count() > 0)
                {
                    ModelState.AddModelError("", "email has already been used or phone number already used");
                    return View(registerModel);
                }
                registerModel.Role = "standard user";
                if (await _roleManager.FindByNameAsync(registerModel.Role) == null)  //must check if role exists every time
                {
                    await _roleManager.CreateAsync(new IdentityRole(registerModel.Role));
                }
                var user = new IdentityUser
                {

                    Email = registerModel.Email,
                    UserName = registerModel.Email,
                    PhoneNumber = registerModel.Phone

                };

                var result = await _userManager.CreateAsync(user, registerModel.Password);

                if (result.Succeeded)
                {
                    BuhleProject.Models.Entities.User user1 = new Models.Entities.User(true)
                    {
                        Email = registerModel.Email,
                        Phone = registerModel.Phone,
                        address = registerModel.address,
                        Name = registerModel.Name,
                        Surname = registerModel.Surname,
                        Institution = registerModel.Institution



                    };
                    user1.completed1 = true;
                    context.UserRepository.Create(user1);
                    context.UserRepository.Save();
                    await _userManager.AddToRoleAsync(user, registerModel.Role);
                    return RedirectToAction("Login", "Account");
                }
                else
                {
                    string s = " ";
                    foreach (var err in result.Errors)
                    {
                        s += err.Code + " " + err.Description + "\n";
                    }
                    ModelState.AddModelError(string.Empty, "Unable to register new user " + s);
                }
            }
            else
            {

                ModelState.AddModelError(string.Empty, "Unable to register new user");
            }


            return View(registerModel);
        }

        [AllowAnonymous]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Login", "Account");
        }



    }
}