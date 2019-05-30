using FleetApi1.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FleetApi1.Models;

namespace FleetApi1.Models
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
     public DbSet<Manager> Managers { get; set; }
        public DbSet<Driver> Drivers { get; set; }
        public DbSet<Company> Companies { get; set; }
        public DbSet<Client> Clients { get; set; }
        public DbSet<Vehicle> Vehicles { get; set; }
        public DbSet<Plan> Plan { get; set; }
        public DbSet<Activity> Activities { get; set; }
        public DbSet<ScheduledActivity> ScheduledActivities { get; set; }
        public DbSet<Bill> Bills { get; set; }
        public DbSet<Delivery> Deliveries { get; set; }
        public DbSet<DeliverySummary> DeliverySummaries { get; set; }
        public DbSet<Route> Routes { get; set; }
        public DbSet<MapLocation> MapLocations { get; set; }

    }
}
