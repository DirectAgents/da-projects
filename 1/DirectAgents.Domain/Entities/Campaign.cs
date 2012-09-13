using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel;

namespace DirectAgents.Domain.Entities
{
    public class Campaign
    {
        public Campaign()
        {
            AccountManagers = new HashSet<Person>();
            AdManagers = new HashSet<Person>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Pid { get; set; }

        public string Name { get; set; } // e.g. US - Casino Pack AT&T 3G/4G - Android Only

        public string Description { get; set; } //e.g. Fun and Easy to download Mobile Game

        public string ImageUrl { get; set; }

        [DisplayName("Payable Action")]
        public string PayableAction { get; set; } // e.g. Subscribe for 3-day free trial 

        [DisplayName("Traffic Type")]
        public string TrafficType { get; set; } // e.g. Mobile traffic only, banners available 

        public string Link { get; set; } // e.g. http://dvs.galaxy.gs/land.php?ms=6&key=76591de61b798d096940fae9e025634708f386e1|105&sid=

        public decimal CostCurrency { get; set; } // e.g. USD

        public decimal Cost { get; set; } // e.g. 1.85 

        public decimal RevenueCurrency { get; set; } // e.g. USD

        public decimal Revenue { get; set; } // e.g. 2.50

        [DisplayName("Important Details")]
        public string ImportantDetails { get; set; } // e.g. Important Details 
        //      Android Only 
        //      No content locking sites 
        //      No free movie download sites 

        [DisplayName("Banned Networks")]
        public string BannedNetworks { get; set; } // e.g. Adgate Media, Adscend Media, Dollarade

        public decimal CampaignCap { get; set; } // e.g. 3,500 downloads per month 

        [DisplayName("Scrub Policy")]
        public string ScrubPolicy { get; set; } // e.g. No Scrub except fraud. 

        [DisplayName("EOM Notes")]
        public string EomNotes { get; set; } // e.g. EOM will be finalized in 5 days. 

        public virtual ICollection<Person> AccountManagers { get; set; }
        public virtual ICollection<Person> AdManagers { get; set; }
        public virtual ICollection<Country> Countries { get; set; }
    }
}
