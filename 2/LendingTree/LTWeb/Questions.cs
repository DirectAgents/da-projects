using System;
using System.Collections.Generic;
using System.Web;
using System.Xml.Linq;
using LTWeb.Models;
using LTWeb.Service;

namespace LTWeb
{
    public static class Questions
    {
        public static QuestionVM[] GetQuestionVMs()
        {
            List<QuestionVM> questionsList = new List<QuestionVM>();
            var xelement = XElement.Load(HttpContext.Current.Request.MapPath("~/App_Data/questions.xml"));
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
                if (xattr != null) question.DependencyKey = xattr.Value;

                xattr = questionEl.Attribute("dependencyvalue");
                if (xattr != null) question.DependencyValue = xattr.Value;

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
            foreach (var question in questionsList)
            {
                question.Progress = (question.QuestionIndex * 100) / questionsList.Count;
            }
            return questionsList.ToArray();
        }

        // returns null if there is no next question
        public static QuestionVM GetNextQuestionVM(string completedKey, ILendingTreeModel ltModel)
        {
            var questions = Questions.GetQuestionVMs();
            if (!ltModel.IsLoanTypeSet())
            {
                var question = questions[0];
                AdjustQuestion(question, ltModel);
                return question;
                //note: doesn't check dependencies
            }
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
                        object qValue = ltModel.GetType().GetProperty(question.DependencyKey).GetValue(ltModel);
                        if (qValue is string)
                        {
                            if ((string)qValue == question.DependencyValue)
                                nextQuestion = question;
                        }
                        else if (qValue is bool)
                        {
                            if ((bool)qValue == bool.Parse(question.DependencyValue))
                                nextQuestion = question;
                        }
                        else
                        {
                            throw new Exception("invalid value");
                        }
                    }
                }
                i++;
            }
            if (nextQuestion != null)
                AdjustQuestion(nextQuestion, ltModel);
            return nextQuestion;
        }

        public static void AdjustQuestion(QuestionVM question, ILendingTreeModel ltModel)
        {
            switch (question.Key)
            {
                case "CashOut":
                    List<OptionVM> newOptions = new List<OptionVM>();
                    for (int i=0; i < question.Options.Count; i++)
                    {
                        var option = question.Options[i];
                        if (Convert.ToInt32(option.Value) <= ltModel.PropertyApproximateValue)
                            newOptions.Add(option);
                    }
                    question.Options = newOptions;
                    break;
            }
        }

    }
}