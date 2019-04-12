using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DirectAgents.Domain.Entities.CPProg.Facebook
{
    public class FbActionType
    {
        public int Id { get; set; }

        [MaxLength(100), Index("CodeIndex", IsUnique = true)]
        public string Code { get; set; }

        public string DisplayName { get; set; }
    }
}
