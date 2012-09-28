using System;
using System.Collections.Generic;
namespace DirectTrack
{
    public interface IRestCall
    {
        string GetXml(string url, out Uri uri, out bool cached);
        Dictionary<string, object> UriVariables { get; set; }
    }
}
