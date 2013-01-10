using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EomToolWeb.HtmlHelpers
{
    public static class FormatHelpers
    {
        static Dictionary<string, string> CurrMap = new Dictionary<string, string>
        {
            {"USD", "en-us"},
            {"GBP", "en-gb"},
            {"EUR", "de-de"},
            {"AUD", "en-AU"}
        };

        public static HtmlString FormatCurrency(this HtmlHelper html, string currency, decimal amount)
        {
            if (currency == null || !CurrMap.ContainsKey(currency))
                return new HtmlString(string.Format("{0:N2}", amount));
            else
                return new HtmlString(string.Format(CultureInfo.CreateSpecificCulture(CurrMap[currency]), "{0:C}", amount));
        }
    }
}