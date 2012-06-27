using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DirectTrack.Rest.Models
{
    /// <summary>
    /// 
    /// </summary>
    public partial class ResourceType
    {
        public enum EType
        {
            Advertiser,
            Campaign
        }

        public EType Type
        {
            get
            {
                EType res;
                if (Enum.TryParse(this.Name, out res))
                    return res;
                else
                    throw new Exception("invalid resource type");
            }
        }
    }
}
