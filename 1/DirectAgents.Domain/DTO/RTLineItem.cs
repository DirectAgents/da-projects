using System.Collections.Generic;
using System.Linq;

namespace DirectAgents.Domain.DTO
{
    public interface IRTLineItem
    {
        //For the client/advertiser or whoever this lineItem applies to
        int? ABId { get; }
        int RTId { get; } // the Id in the RevTrack system (e.g. Cake)
        string Name { get; }

        decimal Revenue { get; }
        decimal Cost { get; }
    }

    public class RTLineItem : IRTLineItem
    {
        public RTLineItem() { }
        public RTLineItem(IEnumerable<IRTLineItem> lineItems)
        {
            Revenue = lineItems.Sum(li => li.Revenue);
            Cost = lineItems.Sum(li => li.Cost);
        }

        public int? ABId { get; set; }
        public int RTId { get; set; }
        public string Name { get; set; }
        public decimal Revenue { get; set; }
        public decimal Cost { get; set; }
    }
}
