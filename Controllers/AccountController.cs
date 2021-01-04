using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoreAdminLTE.Data;
using Microsoft.AspNetCore.Mvc;
//using CoreAdminLTE.Models;

namespace CoreAdminLTE.Controllers
{
    public class AccountController : Controller
    {
        private readonly ModelContext db;

        public AccountController(
            ModelContext db
        )
        {
            this.db = db;
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}