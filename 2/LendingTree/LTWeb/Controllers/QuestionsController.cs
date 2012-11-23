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
            var ltModel = Settings.LTModel;
            if (q > 0 && !ltModel.IsLoanTypeSet()) // if trying to skip ahead or lost session
            {
                if (!Request.IsAjaxRequest())
                {
                    if (test)
                        return RedirectToAction("Show", new { test = test });
                    else
                        return RedirectToAction("Show");
                }
                q = 0; // for ajax requests: return the first question
            }
            var questions = Questions.GetQuestionVMs();
            var question = questions[q];

            Questions.AdjustQuestion(question, ltModel);
            Questions.SetQuestionAnswer(question, ltModel);
 
            if (test)
                return View(question);

            if (Request.IsAjaxRequest())
                return PartialView("FormFields", question);
            else
                return View("FormFields", question);
        }

        // this is used for ajax requests so the browser differentiates it from /Show (non-ajax) and the forward and back work smoothly
        public ActionResult Load(int q = 0)
        {
            return Show(q);
        }

        public ActionResult Save(LendingTreeVM model, string[] questionKey, bool test = false)
        {
            ILendingTreeModel ltModel = Settings.LTModel;

            if (questionKey != null && (questionKey[0] == "LoanType" || ltModel.IsLoanTypeSet()))
            {
                foreach (string qKey in questionKey)
                {
                    PropertyInfo sourcePropInfo = model.GetType().GetProperty(qKey);
                    PropertyInfo destPropInfo = ltModel.GetType().GetProperty(qKey);
                    switch (qKey)
                    {
                        case "IsVetran":
                            ltModel.IsVetran = Request["IsVetran"] == "True";
                            break;
                        default:
                            destPropInfo.SetValue(ltModel, sourcePropInfo.GetValue(model));
                            break;
                    }
                }
            }
            var nextQuestion = Questions.GetNextQuestionVM(questionKey, ltModel);

            if (nextQuestion == null)
            {
                // submit to LendingTree & save to db
                return Content("no more questions");
            }
            else if (Request.IsAjaxRequest())
            {   // there is a nextQuestion
                if (test)
                    return null;
                else
                    return PartialView("FormFields", nextQuestion);
            }
            else // non-ajax && there is a nextQuestion
            {
                if (test)
                    return View("Show", nextQuestion);
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
