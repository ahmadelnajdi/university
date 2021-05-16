using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using University.Areas.Identity.Data;
using University.Data;
using University.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace NajdiClinic.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
       
        private readonly ILogger<HomeController> _logger;

       
        public HomeController(ILogger<HomeController> logger )
        {
          
            _logger = logger;
        }

        public IActionResult Index()
        {



            return View();

        }

        
        public  IActionResult Admin()
        {
            String returnUrl = Url.Content("~/");
            if (Name.logClient != "admin@admin.com")
            {
                ViewBag.ErrorMessage = "ACCESS DENIED !";
                return View("Index", null);
            }

            return RedirectToAction("Admin", "Papers");


        }

        
        public IActionResult Contact()
        {
            return View();
        }

        public IActionResult Nolog()
        {

            return View();
        }
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
