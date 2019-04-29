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
    public class ManagersController : FleetController
    {


        public ManagersController(ApplicationDbContext context) : base(context)
        {

        }

        // GET: Managers
        public async Task<IActionResult> Index()
        {
            if (!LogedIn())
                return RedirectToRoute("Home");
            return RedirectToRoute("Vehicles");
            //return View(await _context.Managers.ToListAsync());
        }

        // GET: Managers/Details/5
        public async Task<IActionResult> Details(long? id)
        {

            if (!LogedIn())
                return RedirectToRoute("Home");

            if (id == null)
            {
                return NotFound();
            }

            var manager = await _context.Managers
                .FirstOrDefaultAsync(m => m.Id == id);
            if (manager == null)
            {
                return NotFound();
            }

            return View(manager);
        }

        public IActionResult LogIn(string Email, string Password)
        {
            try
            {
                Manager manager = _context.Managers.First(m => m.Email == Email && m.Password == Password);
                Company company = _context.Companies.First(c => c.Manager.Id == manager.Id);
                HttpContext.Session.SetInt32("LoggedIn", 1);
                HttpContext.Session.SetInt32("Id", (int)manager.Id);
                HttpContext.Session.SetString("Name", manager.Name);
                HttpContext.Session.SetInt32("CompanyId", (int)company.Id);
                HttpContext.Session.SetString("CompanyName", company.Name);
                HttpContext.Session.SetString("OrderType", company.OrderType);
                return RedirectToAction("Index");
            }
            catch (Exception)
            {
                ViewData["Error"] = "Invalid Login";
            }

            return View("/Views/Home/LogIn.cshtml");
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Email,Password")] Manager manager, string ConfirmPassword)
        {
            bool valid = true;

            if (_context.Managers.Any(m => m.Email == manager.Email))
            {
                ViewData["EmailError"] = "Email already taken";
                valid = false;
            }
            if (manager.Password != ConfirmPassword)
            {
                ViewData["PasswordError"] = "Passwords don't match";
                valid = false;
            }
            if (valid && ModelState.IsValid)
            {
                HttpContext.Session.SetString("Input-Name", manager.Name);
                HttpContext.Session.SetString("Input-Email", manager.Email);
                HttpContext.Session.SetString("Input-Password", manager.Password);
                return View("Views/Companies/Create.cshtml");
            }
            return View(manager);
        }

        // GET: Managers/Edit
        public async Task<IActionResult> Edit()
        {
            if (!LogedIn())
                return RedirectToRoute("Home");
            long id = (long)HttpContext.Session.GetInt32("Id");
            var manager = await _context.Managers.FindAsync(id);
            if (manager == null)
            {
                return NotFound();
            }
            return View(manager);
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([Bind("Id,Name,Email,Password")] Manager manager)
        {
            if (!LogedIn())
                return RedirectToRoute("Home");
            long id = (long)HttpContext.Session.GetInt32("Id");
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(manager);
                    await _context.SaveChangesAsync();
                    HttpContext.Session.SetInt32("Id", (int)manager.Id);
                    HttpContext.Session.SetString("Name", manager.Name);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ManagerExists(manager.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(manager);
        }

        [HttpGet]
        public async Task<IActionResult> Delete()
        {
            if (!LogedIn())
                return RedirectToRoute("Home");

            return View();
        }

        // POST: Managers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed()
        {
            if (!LogedIn())
                return RedirectToRoute("Home");

            long cid = (long)HttpContext.Session.GetInt32("CompanyId");
            long id = (long)HttpContext.Session.GetInt32("Id");
            List<Vehicle> vehicles = _context.Vehicles.Where(v => v.Company.Id == cid).ToList<Vehicle>();
            _context.Vehicles.RemoveRange(vehicles);
            _context.SaveChanges();
            var company = _context.Companies.Find(cid);
            _context.Companies.Remove(company);
            _context.SaveChanges();
            var manager = await _context.Managers.FindAsync(id);
            _context.Managers.Remove(manager);
            await _context.SaveChangesAsync();
            HttpContext.Session.Clear();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> LogOut()
        {
            if (!LogedIn())
                return RedirectToRoute("Home");

            HttpContext.Session.Clear();
            return RedirectToRoute("Home");
        }

        private bool ManagerExists(long id)
        {
            return _context.Managers.Any(e => e.Id == id);
        }
       

    }
}
