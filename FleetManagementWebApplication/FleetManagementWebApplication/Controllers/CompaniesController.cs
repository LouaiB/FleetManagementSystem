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
    public class CompaniesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CompaniesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Companies
        public async Task<IActionResult> Index()
        {
            return View(await _context.Companies.ToListAsync());
        }

        // GET: Companies/Details/5
        public async Task<IActionResult> Details(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var company = await _context.Companies
                .FirstOrDefaultAsync(m => m.Id == id);
            if (company == null)
            {
                return NotFound();
            }

            return View(company);
        }

        // GET: Companies/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Companies/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
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
            {/*
                _context.Add(company);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
                */
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
        public async Task<IActionResult> FinishCreate(string OrderType, bool AutomaticResponse =false)
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
            HttpContext.Session.SetInt32("Id",(int)manager.Id);
            HttpContext.Session.SetString("Name",manager.Name);
            HttpContext.Session.SetString("Company-Name", company.Name);

            _context.Add(company);
     


            
            await _context.SaveChangesAsync();
            //return RedirectToAction(nameof(Index));





            return RedirectToRoute("Manager");
         
        }



        // GET: Companies/Edit/5
        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var company = await _context.Companies.FindAsync(id);
            if (company == null)
            {
                return NotFound();
            }
            return View(company);
        }

        // POST: Companies/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, [Bind("Id,Name,Address,Type,Size,AutomaticReply")] Company company)
        {
            if (id != company.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(company);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CompanyExists(company.Id))
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
            return View(company);
        }

        // GET: Companies/Delete/5
        public async Task<IActionResult> Delete(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var company = await _context.Companies
                .FirstOrDefaultAsync(m => m.Id == id);
            if (company == null)
            {
                return NotFound();
            }

            return View(company);
        }

        // POST: Companies/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            var company = await _context.Companies.FindAsync(id);
            _context.Companies.Remove(company);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CompanyExists(long id)
        {
            return _context.Companies.Any(e => e.Id == id);
        }
    }
}
