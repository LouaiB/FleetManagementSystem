using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using FleetManagementWebApplication.Models;
using Microsoft.AspNetCore.Http;

namespace FleetManagementWebApplication.Controllers
{
    public class CompaniesController : FleetController
    {
        
     
        public CompaniesController(ApplicationDbContext context):base(context)
        {
             
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Address,Type,Size")] Company company)
        {
            bool valid = true;

            if (_context.Companies.Any(m => m.Name == company.Name))
            {
                ViewData["NameError"] = "Name already taken";
                valid = false;
            }
            if (valid&&ModelState.IsValid)
            { 
                HttpContext.Session.SetString("Input-CompanyName", company.Name);
                HttpContext.Session.SetString("Input-CompanyAddress", company.Address);
                HttpContext.Session.SetString("Input-CompanyType", company.Type);
                HttpContext.Session.SetInt32("Input-CompanySize", company.Size);
                return View("Views/Companies/FinishCreate.cshtml");
            }
         
            return View(company);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> FinishCreate(string OrderType, bool AutomaticResponse = false)
        {
            string Name = HttpContext.Session.GetString("Input-Name");
            string Email = HttpContext.Session.GetString("Input-Email");
            string Pass = HttpContext.Session.GetString("Input-Password");
            string CName = HttpContext.Session.GetString("Input-CompanyName");
            string Type = HttpContext.Session.GetString("Input-CompanyType");
            string Address = HttpContext.Session.GetString("Input-CompanyAddress");
            int Size = (int)HttpContext.Session.GetInt32("Input-CompanySize");

            Manager manager = new Manager();
            manager.Name = Name;
            manager.Email = Email;
            manager.Password = Pass;
            Company company = new Company();
            company.Name = CName;
            company.Type = Type;
            company.Address = Address;
            company.Size = Size;
            company.OrderType = OrderType;
            company.AutomaticResponse = AutomaticResponse;
            company.Manager = manager;
            _context.Add(manager);
            _context.SaveChanges();
            HttpContext.Session.SetInt32("Id", (int)manager.Id);
            HttpContext.Session.SetString("Name", manager.Name);

            _context.Add(company);
            _context.SaveChanges();
            HttpContext.Session.SetString("CompanyName", company.Name);
            HttpContext.Session.SetInt32("CompanyId", (int)company.Id);
            HttpContext.Session.SetInt32("LoggedIn", 1);
            HttpContext.Session.SetString("OrderType", company.OrderType);
            await _context.SaveChangesAsync();
            return RedirectToRoute("Manager");

        }
        
    }
}
