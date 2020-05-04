using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RokuAPI.Models
{
    public class OrderItem
    {
        public string Id { get; set; }

        public string OrderName { get; set; }

        public string Type { get; set; }

        public string FlightDates { get; set; }

        public string Budget { get; set; }

        public string OrderDate { get; set; }
    }
}
