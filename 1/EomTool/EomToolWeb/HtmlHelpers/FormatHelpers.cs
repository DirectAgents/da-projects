using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq.Expressions;
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

        public static MvcHtmlString DisplayWithBreaksFor<TModel, TValue>(this HtmlHelper<TModel> html, Expression<Func<TModel, TValue>> expression, bool encode = false)
        {
            var metadata = ModelMetadata.FromLambdaExpression(expression, html.ViewData);
            string model;
            if (encode)
                model = html.Encode(metadata.Model);
            else
                model = (metadata.Model == null) ? null : metadata.Model.ToString();

            if (String.IsNullOrEmpty(model))
                return MvcHtmlString.Empty;

            model = model.Replace("\r\n", "<br />\r\n");

            return MvcHtmlString.Create(model);
        }
    }
}