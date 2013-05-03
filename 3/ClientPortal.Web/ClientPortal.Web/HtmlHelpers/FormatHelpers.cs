using System;
using System.Globalization;
using System.Web;
using System.Web.Mvc;

namespace ClientPortal.Web.HtmlHelpers
{
    public static class FormatHelpers
    {
        public static HtmlString FormatDate(this HtmlHelper html, DateTime? date, CultureInfo cultureInfo)
        {
            string text = null;
            if (date.HasValue)
                text = date.Value.ToString("d", cultureInfo);
            return new HtmlString(text);
        }
    }
}