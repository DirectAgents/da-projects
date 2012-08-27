using DAgents.Common;

namespace EomApp1.Screens.MediaBuyerWorkflow
{
    public partial class MediaBuyerEmailTemplate : ITransformText
    {
        public string MediaBuyerName { get; set; }
        public string UrlToOpen { get; set; }
    }
}
