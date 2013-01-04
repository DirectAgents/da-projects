using System.Collections.Generic;
using System.Globalization;
using EomTool.Domain.Entities;

namespace EomToolWeb.Models
{
    public class PaymentBatchesViewModel
    {
        public IEnumerable<PaymentBatch> Batches { get; set; }
        public bool AllowHold { get; set; }

        Dictionary<string, string> CurrMap = new Dictionary<string, string>
        {
            {"USD", "en-us"},
            {"GBP", "en-gb"},
            {"EUR", "de-de"},
            {"AUD", "en-AU"}
        };

        public string FormatCurrency(string currency, decimal amount)
        {
            if (currency == null || !CurrMap.ContainsKey(currency))
                return string.Format("{0:N2}", amount);
            else
                return string.Format(CultureInfo.CreateSpecificCulture(CurrMap[currency]), "{0:C}", amount);
        }
    }
}