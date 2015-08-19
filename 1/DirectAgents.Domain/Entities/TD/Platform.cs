using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DirectAgents.Domain.Entities.TD
{
    public class Platform
    {
        public int Id { get; set; }
        [MaxLength(50), Index("CodeIndex", IsUnique=true)]
        public string Code { get; set; }
        public string Name { get; set; }

        public virtual List<Account> Accounts { get; set; }

        public const string Code_AdRoll = "adr";
        public const string Code_DBM = "dbm";
    }

    public class Account
    {
        public int Id { get; set; }
        public int PlatformId { get; set; }
        public virtual Platform Platform { get; set; }

        public string ExternalId { get; set; }
        public string Name { get; set; }
    }
}
