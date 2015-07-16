using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace DirectAgents.Domain.Entities.Screen
{
    public class SalespersonStat
    {
        public virtual Salesperson Salesperson { get; set; }

        public int SalespersonId { get; set; }
        public DateTime Date { get; set; }

        public int EmailSent { get; set; }
        public int EmailTracked { get; set; }
        public int EmailOpened { get; set; }
        public int EmailReplied { get; set; }

        [NotMapped]
        public double EmailOpenRate
        {
            get { return Math.Round((double)EmailOpened / EmailTracked, 4); }
        }
        [NotMapped]
        public double EmailReplyRate
        {
            get { return Math.Round((double)EmailReplied / EmailTracked, 4); }
        }
    }
}
