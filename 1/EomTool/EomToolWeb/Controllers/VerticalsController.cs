using System.Linq;
using System.Web.Http;
using DirectAgents.Domain.Contexts;
using DirectAgents.Domain.Entities.Wiki;

namespace EomToolWeb.Controllers
{
    public class VerticalsController : ApiController
    {
        private EFDbContext db = new EFDbContext();

        public IQueryable<Vertical> Get()
        {
            return db.Verticals.AsQueryable();
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}