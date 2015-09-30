using System.Linq;
using System.Web.Mvc;
using DirectAgents.Domain.Abstract;
using DirectAgents.Domain.Entities;

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

        public ActionResult CreateNew()
        {
            var employee = new Employee
            {
                FirstName = "zNew",
                LastName = "Employee"
            };
            if (tdRepo.AddEmployee(employee))
                return RedirectToAction("Index");
            else
                return Content("Error creating Employee");
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
        [HttpPost]
        public ActionResult Edit(Employee emp)
        {
            if (ModelState.IsValid)
            {
                if (tdRepo.SaveEmployee(emp))
                    return RedirectToAction("Index");
                ModelState.AddModelError("", "Employee could not be saved.");
            }
            return View(emp);
        }
	}
}