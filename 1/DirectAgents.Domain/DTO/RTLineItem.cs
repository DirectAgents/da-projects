
namespace DirectAgents.Domain.DTO
{
    public interface IRTLineItem
    {
        string ClientName { get; }

        decimal Revenue { get; }
        decimal Cost { get; }
    }

    public class RTLineItem : IRTLineItem
    {
        public string ClientName { get; set; }
        public decimal Revenue { get; set; }
        public decimal Cost { get; set; }
    }
}
