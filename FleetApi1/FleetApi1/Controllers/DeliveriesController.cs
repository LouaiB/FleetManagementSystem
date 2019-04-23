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
    public class DeliveriesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public DeliveriesController(ApplicationDbContext context)
        {
            _context = context;
        }


        [HttpPost]
        public async Task<ActionResult<Delivery>> GetDelivery(BindingModel B)
        {
            Delivery D = null;
            try
            {
                D = await _context.Deliveries.Where(d=>d.Id==B.id
                                                                                       && d.Driver.Username==B.username
                                                                                       && d.Driver.Password==B.password
                                                                                      ).SingleAsync();
            }
            catch (Exception)
            {
                return BadRequest("Invalid");
            }

            if (D == null)
            {
                return BadRequest("Invalid");
            }

            return D;
        }

        [HttpPost]
        public async Task<ActionResult<IEnumerable<Delivery>>> AnsweredDeliveries(Login L)
        {

            return await _context.Deliveries.Where(d => d.Driver.Username == L.username
                                                                                        && d.Driver.Password == L.password
                                                                                        && d.Answered == true
                                                                                        && d.Started == false).ToListAsync();
        }

        [HttpPost]
        public async Task<ActionResult<IEnumerable<Delivery>>> StartedDeliveries(Login L)
        {

            return await _context.Deliveries.Where(d => d.Driver.Username == L.username
                                                                                        && d.Driver.Password == L.password
                                                                                        && d.Answered == true
                                                                                        && d.Started == true
                                                                                        && d.Finished == false).ToListAsync();
        }
        [HttpPost]
        public async Task<ActionResult<IEnumerable<Delivery>>> FinishedDeliveries(Login L)
        {

            return await _context.Deliveries.Where(d => d.Driver.Username == L.username
                                                                                        && d.Driver.Password == L.password
                                                                                        && d.Answered == true
                                                                                        && d.Started == true
                                                                                        && d.Finished == true).ToListAsync();
        }

        [HttpPost]
        public async Task<ActionResult<Delivery>> MakeAnOrder(Order order)
        {
            Company company = _context.Companies.Where(c=>c.Name==order.CompanyName).Single();

            Delivery delivery = new Delivery
            {
                Client = _context.Clients.Find(order.ClientId),
                Company = company,
                Quantity = order.Quantity,
                SourceCity = order.SourceCity,
                SourceLatitude = order.SourceLatitude,
                SourceLongtitude = order.SourceLongtitude,
                DestinationCity = order.DestinationCity,
                DestinationLatitude = order.DestinationLatitude,
                DestinationLongtitude = order.DestinationLongtitude
            };

            if (company.AutomaticResponse)
            {
                //Calculate shortest route 
                // Assign Vehicle and Driver to delivery
                // Calculate optimized time,distance..
                //Send delivery info to client

            }
            else
            {
                // Add delivery to table 
                // Manager can later select unanswered deliveries
                delivery.Driver = null; //not known yet
                delivery.Vehicle = null; //not known yet
                _context.Deliveries.Add(delivery);

            }
           
            await _context.SaveChangesAsync();
            delivery.Company = null;
            delivery.Client = null;
            return delivery;
        }



        private bool DeliveryExists(long id)
        {
            return _context.Deliveries.Any(e => e.Id == id);
        }
    }
}
