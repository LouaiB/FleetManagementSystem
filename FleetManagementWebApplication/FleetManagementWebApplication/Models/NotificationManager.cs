using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FleetManagementWebApplication.Models
{
    public class NotificationManager
    {
        public Notification[] GetNotifications(long CompanyId,ApplicationDbContext _context)
        {
            List<Notification> notifications = new List<Notification>();

            foreach (Delivery d in _context.Deliveries.Where(d => d.Company.Id == CompanyId && d.Answered == false).
                ToList<Delivery>())
                notifications.Add(new Notification(
                                                    "New order from " + d.SourceCity + " to " + d.DestinationCity,
                                                    d.Id,
                                                    "Map"));
            DateTime dateTime = DateTime.Now.AddDays(5);
            foreach (ScheduledActivity a in _context.ScheduledActivities.Where(a => a.CompanyId == CompanyId && a.DueDate <= dateTime)
                                                                       .Include(a => a.Vehicle).Include(a => a.Activity).ThenInclude(a=>a.Service).ToList<ScheduledActivity>())
                notifications.Add(new Notification(
                                                    a.Vehicle.Make + " " + a.Vehicle.Model + " requires " + a.Activity.Service.Name,
                                                    a.Vehicle.Id,
                                                    "VehiclesDetails"));

            return notifications.ToArray();
        }
    }
}
