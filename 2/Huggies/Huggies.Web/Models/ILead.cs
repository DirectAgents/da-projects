using System;
using System.Web.Mvc;

namespace Huggies.Web.Models
{
    public interface ILead
    {
        string FirstName { get; set; }
        string LastName { get; set; }
        string Email { get; set; }
        string Zip { get; set; }
        string Ethnicity { get; set; }
        bool FirstChild { get; set; }
        string Language { get; set; }
        string Gender { get; set; }
        DateTime? DueDate { get; set; }
        int Id { get; set; }
        int AffiliateId { get; set; }
        string IpAddress { get; set; }
        DateTime Timestamp { get; set; }
        bool Success { get; set; }
        bool Validate(ModelStateDictionary modelState);
        string ValidationErrors { get; set; }
        string Exception { get; set; }
        int SourceId { get; set; }
        bool Test { get; set; }
    }
}