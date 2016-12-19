using System.ComponentModel.DataAnnotations.Schema;
using DirectAgents.Domain.Entities.RevTrack;

namespace DirectAgents.Domain.Entities.AB
{
    public class Client
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }
        public string Name { get; set; }
    }

    //TODO
    // relationships with ProgClient, CakeAdvertiser, etc (IDs)
    // do proof-of-concept with ChildFund
    //
    // credit limit
    // plan other stuff... e.g. budgets - by service/dept - each month
}
