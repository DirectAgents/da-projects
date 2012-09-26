using System.Collections.Generic;
using System.Xml.Linq;
namespace DirectTrack
{
    interface IResourceGetter
    {
        List<XDocument> GetResources();
        event ResourceGetter.GotResourceEventHandler GotResource;
    }
}
