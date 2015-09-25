using System.Linq;
using System.Web.Mvc;
using DirectAgents.Domain.Abstract;

namespace DirectAgents.Web.Areas.TD.Controllers
{
    public class EmployeesController : DirectAgents.Web.Controllers.ControllerBase
    {
        public EmployeesController(ITDRepository tdRepository)
        {
            this.tdRepo = tdRepository;
        }

        public ActionResult Index()
        {
            var employees = tdRepo.Employees()
                .OrderBy(e => e.FirstName).ThenBy(e => e.LastName);
            return View(employees);
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            var employee = tdRepo.Employee(id);
            if (employee == null)
                return HttpNotFound();
            //setupforedit
            return View(employee);
        }
	}
}