using System.ComponentModel.DataAnnotations.Schema;

namespace ClientPortal.Data.Contexts
{
    public partial class SearchAccount
    {
        [NotMapped]
        public bool UseConvertedClicks { get; set; }
    }
}
