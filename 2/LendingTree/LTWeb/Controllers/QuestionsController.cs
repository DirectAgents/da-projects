using System.Reflection;
using System.Web.Mvc;
using LTWeb.Models;
using LTWeb.Service;

namespace LTWeb.Controllers
{
    public class QuestionsController : Controller
    {
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

        public ActionResult Save(LendingTreeVM model, string[] questionKey, bool test = false)
        {
            ILendingTreeModel ltModel = Settings.LTModel;

            if (questionKey.Length > 0 && (questionKey[0] == "LoanType" || ltModel.IsLoanTypeSet()))
            {
                foreach (string qKey in questionKey)
                {
                    PropertyInfo sourcePropInfo = model.GetType().GetProperty(qKey);
                    PropertyInfo destPropInfo = ltModel.GetType().GetProperty(qKey);
                    switch (qKey)
                    {
                        case "IsVetran":
                            ltModel.IsVetran = Request["IsVetran"] == "YES";
                            break;
                        default:
                            destPropInfo.SetValue(ltModel, sourcePropInfo.GetValue(model));
                            break;
                    }
                }
            }
            string key = questionKey.Length > 0 ? questionKey[0] : "";
            var nextQuestion = Questions.GetNextQuestionVM(key, ltModel);

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
            return Content("not implemented yet");
        }

    }
}
