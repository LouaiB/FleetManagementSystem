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
    public class FleetController : Controller
    {
        protected readonly ApplicationDbContext _context;
        protected long Id;
        protected string Name = " Account ";
        protected long CompanyId;
        protected string CompanyName = " Company ";
        protected string OrderType = "Order";
        protected readonly NotificationManager NotificationManager;
        public FleetController(ApplicationDbContext context)
        {
            _context = context;
            NotificationManager = new NotificationManager();
            
        }

        protected bool LogedIn()
        {
            if (HttpContext.Session.GetInt32("LoggedIn") == null)
                return false;

            Id =(long) HttpContext.Session.GetInt32("Id");
            CompanyId = (long)HttpContext.Session.GetInt32("CompanyId");
            ViewData["Notifications"] = NotificationManager.GetNotifications(CompanyId, _context);
            Name = HttpContext.Session.GetString("Name");
            CompanyName = HttpContext.Session.GetString("CompanyName");
            OrderType= HttpContext.Session.GetString("OrderType");
            ViewData["Name"] = Name;
            ViewData["CompanyName"] = CompanyName;
            ViewData["OrderType"] = OrderType;
            return true;

        }
    }
}
