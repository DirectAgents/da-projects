using System;
using System.Collections.Generic;
using System.Linq;
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
                var question = CreateQuestionVM(questionEl, i);
                questionsList.Add(question);
                i++;
            }
            foreach (var question in questionsList)
            {
                question.Progress = question.QuestionIndex;
                if (question.Progress > 0)
                {
                    question.Progress = 20 + (question.QuestionIndex * 80) / (questionsList.Count - 1); // subtract one because last two questions are SSN
                }
            }
            return questionsList.ToArray();
        }

        private static QuestionVM CreateQuestionVM(XElement questionEl, int index)
        {
            var question = new QuestionVM()
            {
                QuestionIndex = index,
                Key = questionEl.Attribute("key").Value,
                Text = questionEl.Attribute("text").Value,
                AnswerType = questionEl.Attribute("answertype").Value
            };

            var xattr = questionEl.Attribute("subtext");
            if (xattr != null) question.Subtext = xattr.Value;

            xattr = questionEl.Attribute("supertext");
            if (xattr != null) question.Supertext = xattr.Value;

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
            var subquestion = questionEl.Descendants("question").FirstOrDefault();
            if (subquestion != null)
                question.SamePageQuestion = CreateQuestionVM(subquestion, index); //note, the subquestion get the same index

            return question;
        }

        // returns null if there is no next question
        public static QuestionVM GetNextQuestionVM(string[] completedKeys, ILendingTreeModel ltModel)
        {
            QuestionVM nextQuestion = null;
            if (completedKeys != null)
            {
                foreach (var completedKey in completedKeys)
                {
                    nextQuestion = GetNextQuestionVM(completedKey, ltModel);
                    if (nextQuestion != null) return nextQuestion;
                }
            }
            return nextQuestion; // (null)
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
                case "EstimatedMortgageBalance":
                    {
                        decimal max = ltModel.PropertyApproximateValue;
                        var options = (from c in question.Options.Skip(1)
                                       where Convert.ToDecimal(c.Value) <= max
                                       select c).ToList();
                        options.Insert(0, question.Options[0]);
                        question.Options = options;
                    }
                    break;
                case "CashOut":
                    {
                        decimal loanAmountBeforeCashOut = ltModel.EstimatedMortgageBalance;
                        decimal loanValue = ltModel.PropertyApproximateValue;
                        decimal maxLoanToValue = .85m;
                        decimal maxCashOut = (loanValue * maxLoanToValue) - loanAmountBeforeCashOut;
                        var options = from c in question.Options
                                      where Convert.ToDecimal(c.Value) <= maxCashOut
                                      select c;
                        question.Options = options.ToList();
                        question.DefaultValue = question.Options.Last().Value; // default to highest cash out value
                    }
                    break;
                case "SSN":
                    question.State = ltModel.PropertyState;
                    break;
            }
            if (question.SamePageQuestion != null)
                AdjustQuestion(question.SamePageQuestion, ltModel);
            // todo: ?have a QuestionVM.Adjusted booleon to avoid infinite loops?
        }

        public static void SetQuestionAnswer(QuestionVM question, ILendingTreeModel ltModel)
        {
            try
            {
                question.Answer = ltModel.GetType().GetProperty(question.Key).GetValue(ltModel).ToString();
            }
            catch { }

            if (question.Key == "CashOut" && !ltModel.IsCashOutSet)
            {
                question.Answer = question.DefaultValue;
            }

            if (question.SamePageQuestion != null)
                SetQuestionAnswer(question.SamePageQuestion, ltModel);
            // todo: something to avoid infinite loops?
        }
    }
}