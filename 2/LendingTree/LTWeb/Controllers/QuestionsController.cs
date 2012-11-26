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
            if (q > 0 && !LendingTreeModel.IsLoanTypeSet()) // if trying to skip ahead or lost session
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

            Questions.AdjustQuestion(question, LendingTreeModel);
            Questions.SetQuestionAnswer(question, LendingTreeModel);
 
            if (test)
                return View(question);

            if (Request.IsAjaxRequest())
                return PartialView("FormFields", question);
            else
                return View("FormFields", question);
        }

        // this is used for ajax requests so the browser differentiates it from /Show (non-ajax) 
        // and the forward and back work smoothly
        public ActionResult Load(int q = 0)
        {
            return Show(q);
        }

        public ActionResult Save(LendingTreeVM model, string[] questionKey, bool test = false)
        {
            // Map properties from LendingTreeVM to ILendingTreeModel
            //

            bool shouldMapProperties = questionKey != null && (questionKey[0] == "LoanType" || LendingTreeModel.IsLoanTypeSet());
            
            if (shouldMapProperties)
            {
                foreach (string propertyName in questionKey)
                {
                    PropertyInfo viewModelProperty = model.GetType().GetProperty(propertyName);
                    PropertyInfo modelProperty = LendingTreeModel.GetType().GetProperty(propertyName);
                    switch (propertyName)
                    {
                        case "IsVetran":
                            LendingTreeModel.IsVetran = Request["IsVetran"] == "True"; // AA: undersatnd this...
                            break;
                        default:
                            modelProperty.SetValue(LendingTreeModel, viewModelProperty.GetValue(model));
                            break;
                    }
                }
            }

            // Produce the view for the next question
            //

            var nextQuestionVM = Questions.GetNextQuestionVM(questionKey, LendingTreeModel);

            if (nextQuestionVM == null)
            {
                // submit to LendingTree & save to db
                //

                // TODO: return THANK YOU
                return Content("no more questions");
            }
            else if (Request.IsAjaxRequest())
            {   
                // there is a nextQuestion
                if (test)
                    return null;
                else
                    return PartialView("FormFields", nextQuestionVM);
            }
            else // non-ajax && there is a nextQuestion
            {
                if (test)
                    return View("Show", nextQuestionVM);
                else
                    return View("FormFields", nextQuestionVM);
            }
        }

        public ActionResult SaveAll()
        {
            return Content("not implemented yet");
        }

        ILendingTreeModel LendingTreeModel { get { return LTWeb.Session.LTModel; } }
    }
}
