using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using CoreAdminLTE.Data;
using CoreAdminLTE.Extensions;
using CoreAdminLTE.Models;
using CoreAdminLTE.Services.Interfaces;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
//using CoreAdminLTE.Models;

namespace CoreAdminLTE.Controllers
{
    public class AccountController : Controller
    {
        private readonly ModelContext db;
        private readonly IMapper mapper;
        private readonly IEmailService emailService;
        private readonly IConfiguration configuration;

        public AccountController(
            ModelContext db,
            IMapper mapper,
            IEmailService emailService,
            IConfiguration configuration
        )
        {
            this.db = db;
            this.mapper = mapper;
            this.emailService = emailService;
            this.configuration = configuration;
        }

        [Authorize]
        public IActionResult Index()
        {
            // TODO: Your code here
            return View();
        }


        //Profile page
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(string email, string password)
        {
            User u = db.Users.FirstOrDefault(n => n.Email == email && n.Password == password);
            if (u != null)
            {
                ClaimsIdentity identity = new ClaimsIdentity(new[]{
                    new Claim(ClaimTypes.Name, u.Fullname),
                    new Claim(ClaimTypes.Email, u.Email)
                }, CookieAuthenticationDefaults.AuthenticationScheme);

                var principal = new ClaimsPrincipal(identity);
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        public IActionResult Register()
        {
            // TODO: Your code here
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterModel registerModel)
        {
            if (ModelState.IsValid)
            {
                User u = mapper.Map<User>(registerModel);
                db.Users.Add(u);
                db.SaveChanges();
                // success message will be added
                // confirmation mail will be sent when emailService ready
                await emailService.SendEmailAsync(u.Email, "register", "kaydoldun h.o.");
                return RedirectToAction("Index");
            }
            return View();
        }

        public IActionResult ResetPassword()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ResetPassword(string email)
        {
            User user = db.Users.FirstOrDefault(n => n.Email == email);
            if (user == null)
                return RedirectToAction("Index", "Home");

            string resetCode = CryptologyHelper.EncryptString(user.UserID.ToString(), configuration["Keys:EncryptKey1"]);
            string body = "Hi, " + user.Fullname +
            "<br /> Follow the link to reset your password: <a target=\"_BLANK\" href=\"" +
            MyHttpContext.AppBaseUrl + "/Account/ResetPassword2?resetCode=" + resetCode +
            "\">Click</a>";

            user.PassResetCode = resetCode;
            user.ResetCodeExpireDate = DateTime.Now.AddHours(1);
            db.Entry(user).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            db.SaveChanges();
            emailService.SendEmailAsync(user.Email, "Password Reset", body);

            return RedirectToAction("Index", "Home");
        }

        public IActionResult ResetPassword2(string resetCode)
        {
            User u = db.Users.FirstOrDefault(n => n.PassResetCode == resetCode && n.ResetCodeExpireDate > DateTime.Now);
            if (u == null)
                return RedirectToAction("Index", "Home");

            ViewBag.resetCode = resetCode;


            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ResetPassword2(string password, string resetCode)
        {
            User u = db.Users.FirstOrDefault(n => n.PassResetCode == resetCode && n.ResetCodeExpireDate > DateTime.Now);

            if (u == null)
                return RedirectToAction("Index", "Home");

            u.Password = password;
            u.PassResetCode = null;
            db.Entry(u).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            db.SaveChanges();

            return RedirectToAction("Index", "Home");
        }


        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            return RedirectToAction("Index");
        }
        

    }
}