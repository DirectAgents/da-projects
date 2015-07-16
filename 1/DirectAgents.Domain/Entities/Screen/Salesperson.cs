using System.ComponentModel.DataAnnotations.Schema;

namespace DirectAgents.Domain.Entities.Screen
{
    public class Salesperson
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }

        [NotMapped]
        public string FullName
        {
            get { return FirstName + " " + LastName; }
        }
    }
}
