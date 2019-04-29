using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using FleetManagementWebApplication.Models;
using Microsoft.AspNetCore.Http;

namespace FleetManagementWebApplication.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {  if (LogedIn())
                return RedirectToRoute("Manager");
            return View();
        }

        public IActionResult LogIn()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = System.Diagnostics.Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        private bool LogedIn()
        {
            if (HttpContext.Session.GetInt32("LoggedIn") == null)
                return false;
            else
                return true;

        }
    }
}
