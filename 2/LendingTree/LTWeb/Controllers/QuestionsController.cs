using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Xml;
using LTWeb.Models;

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
 
            return PartialView(model);
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
                            case "answertype":
                                question.AnswerType = reader.Value;
                                break;
                            case "text":
                                question.Text = reader.Value;
                                break;
                            case "subtext":
                                question.Subtext = reader.Value;
                                break;
                        }
                    }
                    if (question.AnswerType == "dropdown" || question.AnswerType == "radio")
                    {
                        List<string> options = new List<string>();
                        while (reader.Read() && !(reader.NodeType == XmlNodeType.EndElement && reader.Name == "question"))
                        {
                            if (reader.NodeType == XmlNodeType.Element && reader.Name == "option")
                            {
                                if (reader.Read() && reader.NodeType == XmlNodeType.Text)
                                {
                                    options.Add(reader.Value);
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
