﻿using System;
using System.Web.Mvc;
using Huggies.Web.ModelBinders;

namespace Huggies.Web.Models
{
    [ModelBinder(typeof (HuggiesLeadBinder))]
    public class Lead : ILead
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Zip { get; set; }
        public string Ethnicity { get; set; }
        public bool FirstChild { get; set; }
        public string Language { get; set; }
        public string Gender { get; set; }
        public DateTime? DueDate { get; set; }

        public int Id { get; set; }
        public int AffiliateId { get; set; }
        public string IpAddress { get; set; }
        public DateTime Timestamp { get; set; }
        public bool Success { get; set; }
        public string ValidationErrors { get; set; }
        public string Exception { get; set; }

        public int SourceId { get; set; }
        public bool Test { get; set; }

        public virtual bool Validate(ModelStateDictionary modelState)
        {
            if (FirstChild == false)
            {
                modelState.AddModelError("ValidationErrors", Constants.ModelValidationError_must_be_first_child);
            }

            if (!DueDate.HasValue)
            {
                modelState.AddModelError("ValidationErrors", Constants.ModelValidationError_duedate_must_be_specified);
            }

            var futureCutoff = DateTime.Now.AddMonths(11);
            if (DueDate > futureCutoff)
            {
                modelState.AddModelError("ValidationErrors", Constants.ModelValidationError_duedate_too_far_out);
            }

            var cutoff = DateTime.Now.AddMonths(-4);
            if (DueDate < cutoff)
            {
                modelState.AddModelError("ValidationErrors", Constants.ModelValidationError_child_must_be_lessthan_4_months_old_or_unborn);
            }

            if (DueDate < DateTime.Today && Gender == "N")
            {
                modelState.AddModelError("ValidationErrors", Constants.ModelValidationError_gender_cannot_be_unknown_if_child_is_already_born);
            }

            return modelState.IsValid;
        }

        public void PopulateTestValues()
        {
            this.FirstName = "Jane";
            this.LastName = "Doe";
            this.Email = TicksAsString() + "@x.com";
            this.Ethnicity = "AA";
            this.FirstChild = true;
            this.Language = "FR";
            this.Zip = "90210";
            this.Gender = "M";
            this.DueDate = new DateTime(2013, 4, 10);
        }

        public bool SetTest(bool? test)
        {
            if (test.HasValue)
                this.Test = test.Value;
            else
#if DEBUG
                this.Test = true;
#else
                this.Test = false;
#endif
            return this.Test;
        }

        private string TicksAsString()
        {
            long ticks = DateTime.Now.Ticks;
            byte[] bytes = BitConverter.GetBytes(ticks);
            string id = Convert.ToBase64String(bytes)
                               .Replace('+', '_')
                               .Replace('/', '-')
                               .TrimEnd('=');
            return id;
        }
    }
}