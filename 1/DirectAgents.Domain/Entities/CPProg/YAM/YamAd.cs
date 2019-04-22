namespace DirectAgents.Domain.Entities.CPProg.YAM
{
    public class YamAd
    {
        public int Id { get; set; }

        public int ExternalId { get; set; }

        public string Name { get; set; }

        public int LineId { get; set; }

        public virtual YamLine Line { get; set; }
    }
}
