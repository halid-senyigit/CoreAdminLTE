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

        //Profile page
        public IActionResult Index()
        {

            string userID = db.Users.FirstOrDefault().UserID.ToString();


            // encrypt et
            string encrypted = CryptologyHelper.EncryptString(userID, configuration["Keys:EncrypyKey1"]);

            // ve database'e kaydet ve oluşan kodu mail gönder // User tablosunda ResetCode alanı gerekecek // migtation
            //.... 


            ViewBag.encrypted = encrypted;
            // mail gönder link = baseUrl/?resetCode={encrypted}



            // maildeki linke tıklayarak gelen kullanıcıdan alınan resetCode'u decrypt et
            string decrypted = CryptologyHelper.DecryptString(encrypted, configuration["Keys:EncrypyKey1"]);

            // mailden gelen userID(decrypted) databasedeki UserID &&
            // mailden gelen resetCode ile databasedeki ResetCode eşleşiyorsa bu kullanıcının artık güncelleme yapabilmesi gerekir.







            ViewBag.decrypted = decrypted;
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