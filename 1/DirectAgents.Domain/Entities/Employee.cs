using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using DirectAgents.Domain.Entities.CPProg;

namespace DirectAgents.Domain.Entities
{
    public class Employee
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        [NotMapped]
        public string FullName
        {
            get { return FirstName + " " + LastName; }
        }

        [NotMapped]
        public IEnumerable<Advertiser> SalesRepFor { get; set; }
        [NotMapped]
        public IEnumerable<Advertiser> AMFor { get; set; }
    }
}
