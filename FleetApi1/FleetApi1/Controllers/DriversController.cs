using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FleetApi1.Models;


namespace FleetApi1.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class DriversController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public DriversController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Drivers
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Driver>>> GetDrivers()
        {
            return await _context.Drivers.ToListAsync();
        }

        // GET: api/Drivers/5
       [HttpPost]
        public async Task<ActionResult<Driver>> GetDriver(Login L)
        {
            Driver driver = null;
            try
            {
              driver = await _context.Drivers.Where(d => d.Username == L.username && d.Password == L.password)
                                                                      .Include(d=>d.Company).SingleAsync();
            }
           catch (Exception)
            {
                return BadRequest("Invalid Login");
            }

            if (driver == null)
            {
                return BadRequest("Invalid Login");
            }
            driver.Company.Drivers = null;
            return driver;
        }

        // PUT: api/Drivers/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutDriver(long id, Driver driver)
        {
            if (id != driver.Id)
            {
                return BadRequest();
            }

            _context.Entry(driver).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DriverExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Drivers
        [HttpPost]
        public async Task<ActionResult<Driver>> PostDriver(Driver driver)
        {   bool test= _context.Drivers.Any(e => e.Username == driver.Username);
            if(test)
                 return BadRequest("Username Already Exists");
            if(!ModelState.IsValid)
                return BadRequest("Invalid Arguments");
            _context.Drivers.Add(driver);
            await _context.SaveChangesAsync();

            return driver;
        }

        // DELETE: api/Drivers/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Driver>> DeleteDriver(long id)
        {
            var driver = await _context.Drivers.FindAsync(id);
            if (driver == null)
            {
                return NotFound();
            }

            _context.Drivers.Remove(driver);
            await _context.SaveChangesAsync();

            return driver;
        }

        private bool DriverExists(long id)
        {
            return _context.Drivers.Any(e => e.Id == id);
        }
        
    }
}
