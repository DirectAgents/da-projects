namespace DirectAgents.Domain.Entities.CPProg.YAM
{
    public class YamAd : BaseYamEntity
    {
        public int LineId { get; set; }

        public virtual YamLine Line { get; set; }

        public int CreativeId { get; set; }

        public virtual YamCreative Creative { get; set; }
    }
}
