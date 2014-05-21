using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using DirectAgents.Domain.Contexts;
using EomToolWeb.Models;

namespace EomToolWeb.Controllers
{
    public class TrafficTypesController : ApiController
    {
        private EFDbContext db = new EFDbContext();

        public IEnumerable<TrafficTypeViewModel> Get()
        {
            var trafficTypes = db.TrafficTypes.OrderBy(c => c.Name)
                .Select(t =>
                    new TrafficTypeViewModel
                    {
                        TrafficTypeId = t.TrafficTypeId,
                        Name = t.Name
                    });

            return trafficTypes.ToList();
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}
