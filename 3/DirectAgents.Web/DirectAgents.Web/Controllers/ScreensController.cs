using System;
using System.Web.Mvc;
using DirectAgents.Domain.Concrete;
using DirectAgents.Domain.Contexts;
using DirectAgents.Domain.Entities;

namespace DirectAgents.Web.Controllers
{
    public class ScreensController : ControllerBase
    {
        public ScreensController()
        {
            this.mainRepo = new MainRepository(new DAContext());
            //this.securityRepo = new SecurityRepository();
        }

        public ActionResult Variables()
        {
            var variables = mainRepo.GetVariables();
            return View(variables);
        }

        [HttpGet]
        public ActionResult Variable(string name)
        {
            if (String.IsNullOrWhiteSpace(name))
                return Content("No variable name supplied.");

            var variable = mainRepo.GetVariable(name);
            if (variable == null)
            {
                variable = new Variable() { Name = name };
                mainRepo.SaveVariable(variable);
            }
            return View(variable);
        }
        [HttpPost]
        public ActionResult Variable(Variable variable)
        {
            mainRepo.SaveVariable(variable);
            return Content("Saved");
        }

        public ActionResult NewClients(string area)
        {
            var newClientsVar = mainRepo.GetVariable("newClients_" + area);
            string[] newClients = new string[] { };
            if (newClientsVar != null && newClientsVar.StringVal != null)
            {
                newClients = newClientsVar.StringVal.Split(new char[] { '|' });
            }
            var json = Json(newClients, JsonRequestBehavior.AllowGet);
            return json.ToJsonp();
        }

    }
}
