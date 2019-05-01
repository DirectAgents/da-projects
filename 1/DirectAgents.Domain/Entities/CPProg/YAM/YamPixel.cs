namespace DirectAgents.Domain.Entities.CPProg.YAM
{
    public class YamPixel : BaseYamEntity
    {
        public int AccountId { get; set; }

        public virtual ExtAccount Account { get; set; }
    }
}
