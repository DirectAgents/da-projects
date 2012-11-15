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
            Questions.AdjustQuestion(model, Settings.LTModel);
 
            if (test)
                return View(model);
            else
                return View("FormFields", model);
        }

        public ActionResult Save(LendingTreeVM model, string questionKey, bool test = false)
        {
            ILendingTreeModel ltModel = Settings.LTModel;

            if (questionKey == "LoanType" || ltModel.IsLoanTypeSet())
            {
                PropertyInfo sourcePropInfo = model.GetType().GetProperty(questionKey);
                PropertyInfo destPropInfo = ltModel.GetType().GetProperty(questionKey);
                switch (questionKey)
                {
                    case "IsVetran":
                        ltModel.IsVetran = Request["IsVetran"] == "YES";
                        break;
                    default:
                        destPropInfo.SetValue(ltModel, sourcePropInfo.GetValue(model));
                        break;
                }
            }
            var nextQuestion = Questions.GetNextQuestionVM(questionKey, ltModel);

            if (Request.IsAjaxRequest())
            {
                if (test)
                    return null;
                else
                    return PartialView("FormFields", nextQuestion);
            }
            else
            {
                if (test)
                    return View("Show", nextQuestion);

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
