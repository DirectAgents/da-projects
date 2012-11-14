using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using System.Xml;
using System.Xml.Linq;
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

        public ActionResult Show(int q = 0)
        {
            var questions = GetQuestionVMs();
            var model = questions[q];
 
            return View(model);
            //return PartialView("FormFields", model);
        }

        public ActionResult Save(LendingTreeVM model, string questionKey)
        {
            ILendingTreeModel sessionModel = Session["LTModel"] as ILendingTreeModel;
            if (sessionModel == null)
            {
                sessionModel = new LendingTreeModel("Test");
                sessionModel.Initialize();
                Session["LTModel"] = sessionModel;
            }
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
                var nextQuestion = GetNextQuestionVM(questionKey, sessionModel);
                if (nextQuestion == null)
                    return Content("no more questions");
                else
                    return View("Show", nextQuestion);
            }
        }

        public ActionResult SaveAll()
        {
            return Content("thank you");
        }

        // returns null if there is no next question
        private QuestionVM GetNextQuestionVM(string completedKey, ILendingTreeModel ltModel)
        {
            var questions = GetQuestionVMs();
            bool foundCompletedQuestion = false;
            QuestionVM nextQuestion = null;
            int i = 0;
            while ((!foundCompletedQuestion || nextQuestion == null) && i < questions.Length)
            {
                var question = questions[i];
                if (question.Key == completedKey)
                {
                    foundCompletedQuestion = true;
                }
                else if (foundCompletedQuestion)
                {
                    if (string.IsNullOrEmpty(question.DependencyKey))
                    {
                        nextQuestion = question;
                    }
                    else
                    {
                        string qValue = ltModel.GetType().GetProperty(question.DependencyKey).GetValue(ltModel) as string;
                        if (qValue == question.DependencyValue)
                            nextQuestion = question;
                    }
                }
                i++;
            }
            return nextQuestion;
        }

        private QuestionVM[] GetQuestionVMs()
        {
            List<QuestionVM> questionsList = new List<QuestionVM>();
            var xelement = XElement.Load(Request.MapPath("~/App_Data/questions.xml"));
            int i = 0;
            foreach (var questionEl in xelement.Elements("question"))
            {
                var question = new QuestionVM()
                {
                    QuestionIndex = i++,
                    Key = questionEl.Attribute("key").Value,
                    Text = questionEl.Attribute("text").Value,
                    AnswerType = questionEl.Attribute("answertype").Value
                };
                var xattr = questionEl.Attribute("subtext");
                if (xattr != null) question.Subtext = xattr.Value;
                xattr = questionEl.Attribute("dependencykey");
                if (xattr != null)
                {
                    question.DependencyKey = xattr.Value;
                    question.DependencyValue = questionEl.Attribute("dependencyvalue").Value;
                }
                question.Options = new List<OptionVM>();
                foreach (var optionEl in questionEl.Descendants("option"))
                {
                    OptionVM option = new OptionVM()
                    {
                        Text = optionEl.Value,
                        Value = optionEl.Attribute("value").Value
                    };
                    question.Options.Add(option);
                }
                questionsList.Add(question);
            }
            return questionsList.ToArray();
        }
    }
}
