using System.ComponentModel.DataAnnotations.Schema;

namespace EomTool.Domain.Entities
{
    public partial class InvoiceItem
    {
        [NotMapped]
        public string CurrencyName { get; set; }
    }
}
