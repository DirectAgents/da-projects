using System;
using System.Collections.Generic;
namespace DirectTrack
{
    public interface IRestCall
    {
        //[CacheResources]
        string GetXml(string url, out Uri uri);
        Dictionary<string, object> UriVariables { get; set; }
    }
}
