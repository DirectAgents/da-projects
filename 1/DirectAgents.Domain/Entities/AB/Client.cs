using System.ComponentModel.DataAnnotations.Schema;
using DirectAgents.Domain.Entities.RevTrack;

namespace DirectAgents.Domain.Entities.AB
{
    public class ABClient
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }
        public string Name { get; set; }
    }

    //TODO
    // do proof-of-concept with ChildFund
    //
    // credit limit
    // plan other stuff... e.g. budgets - by service/dept - each month
}
