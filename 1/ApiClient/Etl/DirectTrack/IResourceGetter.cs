using System.Collections.Generic;
using System.Xml.Linq;
namespace DirectTrack
{
    interface IResourceGetter
    {
        List<XDocument> Run();
        event ResourceGetter.GotResourceEventHandler GotResource;
    }
}
