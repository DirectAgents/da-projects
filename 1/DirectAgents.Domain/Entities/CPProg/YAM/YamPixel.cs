namespace DirectAgents.Domain.Entities.CPProg.YAM
{
    public class YamPixel
    {
        public int Id { get; set; }

        public int ExternalId { get; set; }

        public string Name { get; set; }

        public int AccountId { get; set; }

        public virtual ExtAccount Account { get; set; }
    }
}
