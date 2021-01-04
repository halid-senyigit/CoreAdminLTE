using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using CoreAdminLTE.Data;
using CoreAdminLTE.Models;
using CoreAdminLTE.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
//using CoreAdminLTE.Models;

namespace CoreAdminLTE.Controllers
{
    public class AccountController : Controller
    {
        private readonly ModelContext db;
        private readonly IMapper mapper;
        private readonly IEmailService emailService;

        public AccountController(
            ModelContext db,
            IMapper mapper,
            IEmailService emailService
        )
        {
            this.db = db;
            this.mapper = mapper;
            this.emailService = emailService;
        }

        //Profile page
        [Authorize("admin")]
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Register()
        {
            // TODO: Your code here
            return View();
        }

        [HttpPost]
        public IActionResult Register(RegisterModel registerModel)
        {
            if (ModelState.IsValid)
            {
                User u = mapper.Map<User>(registerModel);
                db.Users.Add(u);
                db.SaveChanges();
                // success message will be added
                // confirmation mail will be sent when emailService ready
                emailService.SendMail(new Services.EmailModel()
                {
                    // email activation link UserModel....
                    Body = "email activation link.......",
                    Subject = "Register success",
                    To = registerModel.Email

                });
                return RedirectToAction("Index");
            }
            return View();
        }

    }
}