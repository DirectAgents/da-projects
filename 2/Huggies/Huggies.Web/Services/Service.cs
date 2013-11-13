using System;
using Huggies.Web.Models;
using KimberlyClark.Services.Abstract;
using KimberlyClark.Services.Concrete;

namespace Huggies.Web.Services
{
    public class Service : IService
    {
        public bool SendLead(ILead lead, out IProcessResult processResult)
        {
            var service = GetService(lead.Test);
            bool exists;
            try
            {
                exists = service.CheckIfConsumerExists(lead.Email);
            }
            catch (Exception ex)
            {
                lead.Exception = ex.Message;
                processResult = null;
                return false;
            }
            if (exists)
            {
                processResult = null;
                lead.ValidationErrors = "Email already exists.";
                return false;
            }
            var consumer = new Consumer
                {
                    FirstName = lead.FirstName,
                    LastName = lead.LastName,
                    EmailAddress = lead.Email,
                    PostalCode = lead.Zip,
                    EthnicityCode = lead.Ethnicity,
                    FirstTimeParentFlag = lead.FirstChild,
                    LanguageCode = lead.Language,
                    VendorID = 2,
                    VendorIDSpecified = true,
                    DMPermission = true,
                    DMPermissionSpecified = true,
                    EmailPermission = true,
                    EmailPermissionSpecified = true,
                    MobilePermission = false,
                    MobilePermissionSpecified = true,
                    KCBrandsDMPermission = true,
                    KCBrandsDMPermissionSpecified = true,
                    KCBrandsEmailpermission = true,
                    KCBrandsEmailpermissionSpecified = true,
                    ThirdPartyDMPermission = true,
                    ThirdPartyDMPermissionSpecified = true,
                    ThirdPartyEmailpermission = true,
                    ThirdPartyEmailpermissionSpecified = true,
                    Country = char.IsNumber(lead.Zip[0]) ? "US" : "CA",
                    BrandID = 6,
                    BrandIDSpecified = true,
                    SourceID = (lead.Test ? 451 : 637), // if specified by user, will be overwritten below
                    SourceIDSpecified = true,
                    ConsumerChild = new[]
                        {
                            new Child
                                {
                                    Gender = lead.Gender,
                                    BirthDate = lead.DueDate.HasValue
                                                    ? lead.DueDate.Value.ToString("MM/dd/yyyy")
                                                    : string.Empty
                                }
                        },
                };

            if (lead.SourceId > 0)
                consumer.SourceID = lead.SourceId;

            try
            {
                processResult = service.ProcessConsumerInformation(consumer);
            }
            catch (Exception ex)
            {
                lead.Exception = ex.Message;
                processResult = null;
                return false;
            }
            return true;
        }

        public void SaveLead(ILead lead, string[] validationErrors)
        {
            if (validationErrors != null && validationErrors.Length > 0)
            {
                lead.ValidationErrors = string.Join(",", validationErrors);
            }
            using (var context = new Context())
            {
                var repository = new Repository(context);
                repository.Save((Lead) lead);
                context.SaveChanges();
            }
        }

        private static KimberlyClarkCoRegistrationRestService GetService(bool test)
        {
            KimberlyClarkCoRegistrationRestService service;
            if (test)
            {

                service = new KimberlyClarkCoRegistrationRestService(
                    "https://www.qa.coregistrationservice.kimberly-clark.com/CoRegistrationRestService",
                    "kcinet\\inustcap04",
                    "Happy1234567");
            }
            else
            {
                service = new KimberlyClarkCoRegistrationRestService(
                    "https://www.coregistrationservice.kimberly-clark.com/CoRegistrationRestService",
                    "kcinet\\inustcap08",
                    "BU784562qzp");
            }
            return service;
        }
    }
}