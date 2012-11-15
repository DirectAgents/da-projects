using System.Reflection;
using System.Web.Mvc;
using LTWeb.Models;
using LTWeb.Service;

namespace LTWeb.Controllers
{
    public class QuestionsController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Show(int q = 0, bool test = false)
        {
            var questions = Questions.GetQuestionVMs();
            var model = questions[q];
 
            if (test)
                return View(model);
            else
                return PartialView("FormFields", model);
        }

        public ActionResult Save(LendingTreeVM model, string questionKey)
        {
            ILendingTreeModel sessionModel = Settings.LTModel;
            PropertyInfo sourcePropInfo = model.GetType().GetProperty(questionKey);
            PropertyInfo destPropInfo = sessionModel.GetType().GetProperty(questionKey);

            switch (questionKey)
            {
                case "IsVetran":
                    sessionModel.IsVetran = Request["IsVetran"] == "YES";
                    break;
                default:
                    destPropInfo.SetValue(sessionModel, sourcePropInfo.GetValue(model));
                    break;
            }

            if (Request.IsAjaxRequest())
                return null;
            else
            {
                var nextQuestion = Questions.GetNextQuestionVM(questionKey, sessionModel);
                if (nextQuestion == null)
                    return Content("no more questions");
                else
                    return View("FormFields", nextQuestion);
            }
        }

        public ActionResult SaveAll()
        {
            return Content("thank you");
        }

    }
}
