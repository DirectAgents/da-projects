using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LTWeb.Models
{
    public class QuestionVM
    {
        public string Text { get; set; }
        public string Subtext { get; set; }

        public string AnswerType { get; set; }
        public List<string> Options { get; set; }

        public int NextQuestionIndex { get; set; }
    }
}