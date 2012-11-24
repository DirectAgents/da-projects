using System.Collections.Generic;

namespace LTWeb.Models
{
    public class QuestionVM
    {
        public string Text { get; set; }
        public string Subtext { get; set; }
        public string Supertext { get; set; }

        public string Key { get; set; }
        public string AnswerType { get; set; }
        public List<OptionVM> Options { get; set; }

        public string DependencyKey { get; set; }
        public string DependencyValue { get; set; }

        public int QuestionIndex { get; set; }
        public int Progress { get; set; }

        public string Answer { get; set; }

        public QuestionVM SamePageQuestion { get; set; }
    }

    public class OptionVM
    {
        public string Text { get; set; }
        public string Value { get; set; }
    }
}