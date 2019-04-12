using System;

namespace DirectAgents.Domain.Entities.CPProg.Facebook
{
    public class FbAction
    {
        public DateTime Date { get; set; }

        public int ActionTypeId { get; set; }

        public virtual ActionType ActionType { get; set; }

        public int PostClick { get; set; }

        public int PostView { get; set; }

        public decimal PostClickVal { get; set; }

        public decimal PostViewVal { get; set; }

        public string ClickAttrWindow { get; set; }

        public string ViewAttrWindow { get; set; }
    }
}
