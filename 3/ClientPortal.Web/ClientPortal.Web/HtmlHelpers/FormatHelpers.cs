using System;
using System.Globalization;
using System.Linq.Expressions;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;

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

        public static MvcHtmlString DisplayWithBreaksFor<TModel, TValue>(this HtmlHelper<TModel> html, Expression<Func<TModel, TValue>> expression, bool encode = false)
        {
            var metadata = ModelMetadata.FromLambdaExpression(expression, html.ViewData);
            var model = encode ? html.Encode(metadata.Model) : metadata.Model.ToString();
            model = model.Replace("\r\n", "<br />\r\n");

            if (String.IsNullOrEmpty(model))
                return MvcHtmlString.Empty;

            return MvcHtmlString.Create(model);
        }
    }

    public static class RazorHelpers
    {
        public static AjaxOptions GetAjaxOptions(string updateTargetId, string switchLink = null, string httpMethod = "Get", string confirmMsg = null)
        {
            var ajaxOptions = new AjaxOptions()
            {
                UpdateTargetId = updateTargetId,
                HttpMethod = httpMethod,
                OnBegin = String.Format("$('#{0}').html('Loading...');", updateTargetId)
            };
            if (switchLink != null)
                ajaxOptions.OnBegin += String.Format("$('#{0}').click();", switchLink);
            if (confirmMsg != null)
                ajaxOptions.Confirm = confirmMsg;

            return ajaxOptions;
        }
    }
}