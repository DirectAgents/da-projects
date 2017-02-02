using System.Collections.Generic;

namespace EomToolWeb.Models
{
    public class SynchVM
    {
        public string CurrentEomDateString { get; set; }
        public bool Posted { get; set; }
        public IEnumerable<SynchResults> Results { get; set; }

        public class SynchResults
        {
            public int OfferID { get; set; }
            public int NumItemsSynched { get; set; }
            public string Message { get; set; }
        }
    }
}