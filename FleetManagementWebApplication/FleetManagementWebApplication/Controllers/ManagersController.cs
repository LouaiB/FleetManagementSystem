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
    public class ManagersController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ManagersController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Managers
        public async Task<IActionResult> Index()
        {
            if (!isLogedIn())
                return RedirectToRoute("Home");
            ViewData["Name"] = HttpContext.Session.GetString("Name");
            ViewData["CompanyName"] = HttpContext.Session.GetString("CompanyName");
            ViewData["CompanyId"] = HttpContext.Session.GetInt32("CompanyId");
            ViewData["Id"] = HttpContext.Session.GetInt32("Id");
            return View();
            //return View(await _context.Managers.ToListAsync());
        }

        // GET: Managers/Details/5
        public async Task<IActionResult> Details(long? id)
        {
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

        // GET: Managers/Create
        public IActionResult Create()
        {
            return View();
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
                return RedirectToAction("Index");
            }
            catch (Exception)
            {
                ViewData["Error"] = "Invalid Login";
            }

                return View("/Views/Home/LogIn.cshtml");
        }


        // POST: Managers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Email,Password")] Manager manager,string ConfirmPassword)
        {
            bool valid = true;
         
              if( _context.Managers.Any(m => m.Email == manager.Email)) {         
                ViewData["EmailError"] = "Email already taken";
                valid = false;
            }
            if (manager.Password != ConfirmPassword)
            {
                ViewData["PasswordError"] = "Passwords don't match";
                valid = false;
            }
            
            if (valid&&ModelState.IsValid)
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

            long id =(long) HttpContext.Session.GetInt32("Id");
             var manager = await _context.Managers.FindAsync(id);
             if (manager == null)
            {
                return NotFound();
           }
            return View(manager);
        }

    

        // POST: Managers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([Bind("Id,Name,Email,Password")] Manager manager)
        {
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

        // GET: Managers/Delete/5
        [HttpGet]
        public async Task<IActionResult> Delete()
        {
 
            return View();
        }

        // POST: Managers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed()
        {
            long cid = (long)HttpContext.Session.GetInt32("CompanyId");
            long id = (long)HttpContext.Session.GetInt32("Id");
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
            HttpContext.Session.Clear();
            return RedirectToRoute("Home");
        }

        private bool ManagerExists(long id)
        {
            return _context.Managers.Any(e => e.Id == id);
        }
        private bool isLogedIn()
        {
            if (HttpContext.Session.GetInt32("LoggedIn") == null)
                return false;
            else
                return true;

        }

    }
}
