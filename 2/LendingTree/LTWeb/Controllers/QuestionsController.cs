using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using System.Xml;
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
/*
            var question = new QuestionVM
            {
                Text = "Type of loan:",
                AnswerType = "radio",
                Options = new List<string>
                {
                    "Refinance Mortgage",
                    "Purchase Home"
                }
            };
*/
            var questions = GetQuestionVMs();
            var model = questions[q];
            model.NextQuestionIndex = q + 1;
 
            return View(model);
            //return PartialView("FormFields", model);
        }

        public ActionResult Save(LendingTreeVM model, string questionKey)
        {
            ILendingTreeModel sessionModel = Session["LTModel"] as ILendingTreeModel;
            //LendingTreeVM sessionModel = Session["LTModel"] as LendingTreeVM;
            if (sessionModel == null)
            {
                sessionModel = new LendingTreeModel("Test");
                sessionModel.Initialize();
                //sessionModel = new LendingTreeVM();
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
                // Determine next question
                var questions = GetQuestionVMs();
                bool foundThisQuestion = false;
                QuestionVM nextQuestion = null;
                int i = 0;
                while ((!foundThisQuestion || nextQuestion == null) && i < questions.Length)
                {
                    var question = questions[i];
                    if (question.Key == questionKey)
                    {
                        foundThisQuestion = true;
                    }
                    else if (foundThisQuestion)
                    {
                        if (string.IsNullOrEmpty(question.DependencyKey))
                        {
                            nextQuestion = question;
                        }
                        else
                        {
                            string qValue = sessionModel.GetType().GetProperty(question.DependencyKey).GetValue(sessionModel) as string;
                            if (qValue == question.DependencyValue)
                                nextQuestion = question;
                        }
                    }
                    i++;
                }
                if (nextQuestion == null)
                    return Content("no more questions");
                else
                    return RedirectToAction("Show", new { q = i - 1 });
                    //return View("Show", nextQuestion); // didn't work... the hidden "QuestionKey" got set to the wrong value. browser cache?
            }
        }

        public ActionResult SaveAll()
        {
            return Content("thank you");
        }

        private QuestionVM[] GetQuestionVMs()
        {
            List<QuestionVM> questions = new List<QuestionVM>();
            XmlTextReader reader = new XmlTextReader(Request.PhysicalApplicationPath + "App_Data\\questions.xml");
            while (reader.Read())
            {
                if (reader.NodeType == XmlNodeType.Element && reader.Name == "question")
                {
                    var question = new QuestionVM();
                    while (reader.MoveToNextAttribute())
                    {
                        switch (reader.Name)
                        {
                            case "text":
                                question.Text = reader.Value;
                                break;
                            case "subtext":
                                question.Subtext = reader.Value;
                                break;
                            case "key":
                                question.Key = reader.Value;
                                break;
                            case "answertype":
                                question.AnswerType = reader.Value;
                                break;
                            case "dependencykey":
                                question.DependencyKey = reader.Value;
                                break;
                            case "dependencyvalue":
                                question.DependencyValue = reader.Value;
                                break;
                        }
                    }
                    if (question.AnswerType == "dropdown" || question.AnswerType == "radio")
                    {
                        List<OptionVM> options = new List<OptionVM>();
                        while (reader.Read() && !(reader.NodeType == XmlNodeType.EndElement && reader.Name == "question"))
                        {
                            if (reader.NodeType == XmlNodeType.Element && reader.Name == "option")
                            {
                                OptionVM option = new OptionVM();
                                while (reader.MoveToNextAttribute())
                                {
                                    switch (reader.Name)
                                    {
                                        case "value":
                                            option.Value = reader.Value;
                                            break;
                                    }
                                }
                                if (reader.Read() && reader.NodeType == XmlNodeType.Text)
                                {
                                    option.Text = reader.Value;
                                    options.Add(option);
                                }
                            }
                        }
                        question.Options = options;
                    }
                    questions.Add(question);
                }
            }
            reader.Dispose();
            return questions.ToArray();
        }
    }
}
