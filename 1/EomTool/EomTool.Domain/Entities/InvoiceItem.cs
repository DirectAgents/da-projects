using System.ComponentModel.DataAnnotations.Schema;

namespace EomTool.Domain.Entities
{
    public partial class InvoiceItem
    {
        public string _currencyName = null;
        [NotMapped]
        public string CurrencyName
        {
            set { _currencyName = value; }
            get
            {
                if (_currencyName == null && Currency != null)
                    _currencyName = Currency.name;
                return _currencyName;
            }
        }

        [NotMapped]
        public decimal TotalAmount0
        {
            get { return (total_amount.HasValue ? total_amount.Value : 0); }
        }

    }
}
