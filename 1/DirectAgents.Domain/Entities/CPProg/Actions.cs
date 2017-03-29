using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DirectAgents.Domain.Entities.CPProg
{
    public class ActionType
    {
        public int Id { get; set; }
        [MaxLength(100), Index("CodeIndex", IsUnique = true)]
        public string Code { get; set; }
        public string DisplayName { get; set; }
    }

    public class ActionStats
    {
        public DateTime Date { get; set; }
        public int ActionTypeId { get; set; }
        public virtual ActionType ActionType { get; set; }
        public int PostClick { get; set; }
        public int PostView { get; set; }
    }

    public class StrategyAction : ActionStats
    {
        public int StrategyId { get; set; }
        public virtual Strategy Strategy { get; set; }
    }

    //TODO: StrategyActionValue
}
