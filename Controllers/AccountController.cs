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
using Microsoft.AspNetCore.DataProtection;
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
        private readonly IDataProtector dataProtector;

        public AccountController(
            ModelContext db,
            IMapper mapper,
            IEmailService emailService,
            IDataProtectionProvider protectionProvider
        )
        {
            this.db = db;
            this.mapper = mapper;
            this.emailService = emailService;
            this.dataProtector = protectionProvider.CreateProtector("protector_provider_account");
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
            User u = db.Users.FirstOrDefault(n => n.Email == email && n.Password == password && n.IsActive == true);
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
                User user = mapper.Map<User>(registerModel);
                user.IsActive = false;
                user.ActivationCode = dataProtector.Protect(user.Email);

                db.Users.Add(user);
                db.SaveChanges();
                // success message will be added

                

                string body = "Hi, " + user.Fullname +
                "<br /> Follow the link to activate your account: <a target=\"_BLANK\" href=\"" +
                MyHttpContext.AppBaseUrl + "/Account/Activate?activationCode=" + user.ActivationCode +
                "\">Click</a>";

                await emailService.SendEmailAsync(user.Email, "Complate your registration", body);
                return RedirectToAction("Index");
            }
            return View();
        }


        public IActionResult Activate(string activationCode)
        {
            string email = dataProtector.Unprotect(activationCode);
            User user = db.Users.FirstOrDefault(n => 
                n.ActivationCode == activationCode && 
                n.IsActive == false &&
                n.Email == email
            );
            if(user == null)
                return RedirectToAction("Register");
            
            user.ActivationCode = null;
            user.IsActive = true;
            db.Entry(user).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            db.SaveChanges();

            return RedirectToAction("Login", new { message = "activation.complate" });
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

            string resetCode = dataProtector.Protect(user.Email);
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
            string decryptedEmail = dataProtector.Unprotect(resetCode);
            User u = db.Users.FirstOrDefault(n =>
                n.PassResetCode == resetCode &&
                n.ResetCodeExpireDate > DateTime.Now &&
                n.Email == decryptedEmail
            );
            if (u == null)
                return RedirectToAction("Index", "Home");

            ViewBag.resetCode = resetCode;


            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ResetPassword2(string password, string resetCode)
        {
            string decryptedEmail = dataProtector.Unprotect(resetCode);
            User u = db.Users.FirstOrDefault(n =>
                n.PassResetCode == resetCode &&
                n.ResetCodeExpireDate > DateTime.Now &&
                n.Email == decryptedEmail
            );

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