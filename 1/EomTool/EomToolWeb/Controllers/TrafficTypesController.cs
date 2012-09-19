using System.Linq;
using System.Web.Http;
using DirectAgents.Domain.Concrete;
using DirectAgents.Domain.Entities;
using System.Collections.Generic;
using EomToolWeb.Models;

namespace EomToolWeb.Controllers
{
    public class TrafficTypesController : ApiController
    {
        private EFDbContext db = new EFDbContext();

        public IEnumerable<TrafficTypeViewModel> Get()
        {
            var trafficTypes = db.TrafficTypes.OrderBy(t => t.Name).ToList();
            return trafficTypes.Select(t => new TrafficTypeViewModel(t));
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}
