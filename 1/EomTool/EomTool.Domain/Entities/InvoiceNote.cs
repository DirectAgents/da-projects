using System.ComponentModel.DataAnnotations.Schema;

namespace EomTool.Domain.Entities
{
    public partial class InvoiceNote
    {
        [NotMapped]
        public string AddedBy_IdOnly
        {
            get
            {
                if (added_by != null && added_by.IndexOf("DIRECTAGENTS\\") == 0)
                    return added_by.Substring(13);
                else
                    return added_by;
            }
        }
    }
}
