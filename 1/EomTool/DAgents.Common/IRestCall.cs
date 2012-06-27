using System.Collections.Generic;

namespace DAgents.Common
{
    public interface IRestCall
    {
        string Url
        {
            get;
            set;
        }

        string Method
        {
            get;
            set;
        }

        IDictionary<string, string> Headers
        {
            get;
            set;
        }

        string Response
        {
            get;
            set;
        }

        bool NoCache
        {
            get;
            set;
        }

        IRestCall Invoke();
    }
}
