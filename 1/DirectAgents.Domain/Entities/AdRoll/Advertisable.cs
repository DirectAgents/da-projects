using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace DirectAgents.Domain.Entities.AdRoll
{
    public class Advertisable
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Eid { get; set; }

        [NotMapped]
        public AdRollStat Stats { get; set; }
    }
}
