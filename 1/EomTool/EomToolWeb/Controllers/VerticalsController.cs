using System.Linq;
using System.Web.Http;
using DirectAgents.Domain.Concrete;
using DirectAgents.Domain.Entities;

namespace EomToolWeb.Controllers
{
    public class VerticalsController : ApiController
    {
        private EFDbContext db = new EFDbContext();

        // GET api/Verticals
        public IQueryable<Campaign> Get()
        {
            return db.Campaigns.AsQueryable();
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}