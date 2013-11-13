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

            string dateTimeString = GetQueryStringValue("fm_duedate");
            model.DueDate = string.IsNullOrWhiteSpace(dateTimeString) 
                                        ? default(DateTime?) 
                                        : DateTime.Parse(dateTimeString);

            return model;
        }

        private string GetQueryStringValue(string key)
        {
            return this.request.HttpMethod.ToUpper() == "POST" ? this.request.Form[key] : this.request.QueryString[key];
        }
    }
}