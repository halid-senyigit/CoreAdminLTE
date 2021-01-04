using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using CoreAdminLTE.Data;
using CoreAdminLTE.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
//using CoreAdminLTE.Models;

namespace CoreAdminLTE.Controllers
{
    public class AccountController : Controller
    {
        private readonly ModelContext db;
        private readonly IMapper mapper;

        public AccountController(
            ModelContext db,
            IMapper mapper
        )
        {
            this.db = db;
            this.mapper = mapper;
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
            if(ModelState.IsValid){
                User u = mapper.Map<User>(registerModel);
                db.Users.Add(u);
                db.SaveChanges();
                // success message will be added
                // confirmation mail will be sent when emailService ready
                return RedirectToAction("Index");
            }
            return View();
        }
        
    }
}