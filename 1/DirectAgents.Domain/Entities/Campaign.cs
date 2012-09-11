using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DirectAgents.Domain.Entities
{
    public class Campaign
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Pid
        {
            get;
            set;
        }

        public string Name
        {
            get;
            set;
        }

        public virtual ICollection<Person> AccountManagers
        {
            get;
            set;
        }

        public virtual ICollection<Person> MediaBuyers
        {
            get;
            set;
        }
    }
}
