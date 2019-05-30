using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using FleetManagementWebAplication.Models;
using FleetManagementWebApplication.Models;

namespace FleetManagementWebApplication.Controllers
{
    public class ClientsController : FleetController
    {
   
        public ClientsController(ApplicationDbContext context):base(context)
        {     
        }

        // GET: Clients
        public async Task<IActionResult> Index()
        {
            if (!LogedIn())
                return RedirectToRoute("/home");
           Client[] clients = _context.Deliveries.Where(d => d.Company.Id == CompanyId).Select(d => d.Client)
                                                         .Union(_context.Clients.Where(c => c.Company.Id == CompanyId)).ToArray();
            return View(clients);
        }

        // GET: Clients/Details/5
        public async Task<IActionResult> Details(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var client = await _context.Clients
                .FirstOrDefaultAsync(m => m.Id == id);
            if (client == null)
            {
                return NotFound();
            }

            return View(client);
        }

        // GET: Clients/Create
        public IActionResult Create()
        {
            if (!LogedIn())
                return RedirectToRoute("/home");
            return View();
        }

        [HttpPost]
        public IActionResult Create(AddClient model)
        {
            if (!LogedIn())
                return RedirectToRoute("/home");

            if (ModelState.IsValid)
            {
                if (_context.Clients.Any(C => C.Username == model.Username))
                {
                    ViewData["message"]= "Email Already Taken";
                    return View();
                }
                Client c = new Client()
                {
                    Name = model.Name,
                    Username = model.Username,
                    Password = model.Password,
                    Phonenumber = model.Phone,
                    Company = _context.Companies.Find(CompanyId)
                };
                _context.Clients.Add(c);
                _context.SaveChanges();

                return RedirectToAction("Index");
            }
            return View();
        }
     
    }
}
