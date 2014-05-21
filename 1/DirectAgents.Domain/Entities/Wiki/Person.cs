using System.Collections.Generic;

namespace DirectAgents.Domain.Entities.Wiki
{
    public class Person
    {
        public int PersonId
        {
            get;
            set;
        }

        public string Name
        {
            get;
            set;
        }

        public virtual ICollection<Campaign> AccountManagerCampaigns
        {
            get;
            set;
        }

        public virtual ICollection<Campaign> AdManagerCampaigns
        {
            get;
            set;
        }
    }
}
