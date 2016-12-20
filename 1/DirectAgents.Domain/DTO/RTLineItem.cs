
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
        public int? ABId { get; set; }
        public int RTId { get; set; }
        public string Name { get; set; }
        public decimal Revenue { get; set; }
        public decimal Cost { get; set; }
    }
}
