using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FleetApi1.Models
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
                                                    "Trips"));
            DateTime dateTime = DateTime.Now.AddDays(5);
            foreach (ScheduledActivity a in _context.ScheduledActivities.Where(a => a.CompanyId == CompanyId && a.DueDate <= dateTime)
                                                                       .Include(a => a.Vehicle).Include(a => a.Activity).ToList<ScheduledActivity>())
                notifications.Add(new Notification(
                                                    a.Vehicle.Make + " " + a.Vehicle.Model + " requires " + a.Activity.Type,
                                                    a.Vehicle.Id,
                                                    "VehiclesDetails"));

            return notifications.ToArray();
        }
    }
}
