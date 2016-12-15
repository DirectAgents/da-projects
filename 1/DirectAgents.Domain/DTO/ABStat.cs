using System.ComponentModel.DataAnnotations.Schema;

namespace DirectAgents.Domain.DTO
{
    public class ABStat
    {
        // ? CakeId ?
        public int Id { get; set; }

        public string Client { get; set; }
        //public string Campaign { get; set; }
        //public string Vendor { get; set; }

        public decimal Rev { get; set; }
        public decimal Cost { get; set; }
        public decimal Margin
        {
            get { return Rev - Cost; }
        }

        public decimal StartBal { get; set; }
        public decimal CurrBal
        {
            get { return StartBal - Rev; }
        }
        public decimal CredLim { get; set; }
    }
}
