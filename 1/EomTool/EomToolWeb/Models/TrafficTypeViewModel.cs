using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DirectAgents.Domain.Entities;

namespace EomToolWeb.Models
{
    public class TrafficTypeViewModel
    {
        private TrafficType _trafficType;

        public TrafficTypeViewModel()
        {
        }

        public TrafficTypeViewModel(TrafficType trafficType)
        {
            _trafficType = trafficType;
        }

        public int TrafficTypeId { get { return _trafficType.TrafficTypeId; } set { } }
        public string Name { get { return _trafficType.Name; } set { } }
    }
}