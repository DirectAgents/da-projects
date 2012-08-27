using DAgents.Common;
using System.Collections.Generic;

namespace EomApp1.Screens.MediaBuyerWorkflow
{
    public class MediaBuyerEmail : MediaBuyerEmailTemplate, ITransformText
    {
        public MediaBuyerEmail(Dictionary<string, object> data)
        {
            this.Data = data;
        }

        public string MediaBuyerName { get; set; }

        public string UrlToOpen { get; set; }

        public Dictionary<string, object> Data { get; set; }
    }
}
