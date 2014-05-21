using System.ComponentModel.DataAnnotations.Schema;

namespace DirectAgents.Domain.Entities.Wiki
{
    public class Status
    {
        public static int Private = 2;
        public static int ApplyToRun = 3;
        public static int Inactive = 4;

        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int StatusId { get; set; }
        public string Name { get; set; }
    }
}
