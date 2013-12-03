using System;
using System.Web;
using System.Web.Mvc;
using Huggies.Web.Models;

namespace Huggies.Web.ModelBinders
{
    public class HuggiesLeadBinder : IModelBinder
    {
        private HttpRequestBase request;

        public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            if (bindingContext == null)
                throw new ArgumentNullException("bindingContext");

            this.request = controllerContext.HttpContext.Request;

            var model = new Lead
            {
                FirstName = GetQueryStringValue("fm_firstname"),
                LastName = GetQueryStringValue("fm_lastname"),
                Email = GetQueryStringValue("fm_email"),
                Zip = GetQueryStringValue("fm_zip"),
                Ethnicity = GetQueryStringValue("fm_ethnicity"),
                Language = GetQueryStringValue("fm_language"),
                FirstChild = (GetQueryStringValue("fm_firstchild") == "true"),
                Gender = GetQueryStringValue("fm_gender"),
                Test = (GetQueryStringValue("fm_test") == "true"),
            };

            DateTime dueDate;
            string dateTimeString = GetQueryStringValue("fm_duedate");
            if (DateTime.TryParse(dateTimeString, out dueDate))
                model.DueDate = dueDate;
            else
                model.DueDate = null;

            int affId;
            var a = GetQueryStringValue("a");
            if (Int32.TryParse(a, out affId))
                model.AffiliateId = affId;

            int sourceId;
            var s = GetQueryStringValue("s");
            if (Int32.TryParse(s, out sourceId))
                model.SourceId = sourceId;

            return model;
        }

        private string GetQueryStringValue(string key)
        {
            return this.request.HttpMethod.ToUpper() == "POST" ? this.request.Form[key] : this.request.QueryString[key];
        }
    }
}