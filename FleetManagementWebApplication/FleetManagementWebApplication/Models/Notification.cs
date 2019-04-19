using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FleetManagementWebApplication.Models
{
    public class Notification
    {
        public string Title { get; set; }
        public long Id { get; set; }
        public string Controller { get; set; }

        public Notification(string title,long id,string controller)
        {
            Title = title;
            Id = id;
            Controller = controller;
        }
    }
}
