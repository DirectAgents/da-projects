using System.ComponentModel.DataAnnotations.Schema;

namespace EomTool.Domain.Entities
{
    public partial class Advertiser
    {
        [NotMapped]
        public AccountManager AccountManager { get; set; }
    }
}
