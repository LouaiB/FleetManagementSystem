using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FleetApi1.Models;
using FleetApi1.Models;

namespace FleetApi1.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ClientsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ClientsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Clients
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Client>>> GetClients()
        {
            return await _context.Clients.ToListAsync();
        }

        // GET: api/Clients/5
        [HttpPost]
        public async Task<ActionResult<Client>> GetClient(Login L)
        {
            Client c = null;
            try
            {
                c = await _context.Clients.Where(d => d.Username == L.username && d.Password == L.password).SingleAsync();
            }
            catch (Exception)
            {
                return BadRequest("Invalid Login");
            }

            if (c == null)
            {
                return BadRequest("Invalid Login");
            }

            return c;
        }

        // PUT: api/Clients/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutClient(long id, Client client)
        {
            if (id != client.Id)
            {
                return BadRequest();
            }

            _context.Entry(client).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ClientExists(id))
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

        // POST: api/Clients
        [HttpPost]
        public async Task<ActionResult<Client>> PostClient(Client c)
        {
            bool test = _context.Clients.Any(e => e.Username ==c.Username);
            if (test)
                return BadRequest("Username Already Exists");
            if (!ModelState.IsValid)
                return BadRequest("Invalid Arguments");
            _context.Clients.Add(c);
            await _context.SaveChangesAsync();

            return c;
        }

        // DELETE: api/Clients/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Client>> DeleteClient(long id)
        {
            var client = await _context.Clients.FindAsync(id);
            if (client == null)
            {
                return NotFound();
            }

            _context.Clients.Remove(client);
            await _context.SaveChangesAsync();

            return client;
        }

        private bool ClientExists(long id)
        {
            return _context.Clients.Any(e => e.Id == id);
        }
    }
}
