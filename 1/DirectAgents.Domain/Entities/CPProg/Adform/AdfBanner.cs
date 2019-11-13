namespace DirectAgents.Domain.Entities.CPProg.Adform
{
    public class AdfBanner : AdfBaseEntity
    {
        public int LineItemId { get; set; }

        public virtual AdfLineItem LineItem { get; set; }
    }
}
