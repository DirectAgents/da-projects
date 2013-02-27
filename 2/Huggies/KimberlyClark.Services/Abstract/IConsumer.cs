using KimberlyClark.Services.Concrete;

namespace KimberlyClark.Services.Abstract
{
    public interface IConsumer
    {
        string Address1 { get; set; }
        string Address2 { get; set; }
        string BirthDate { get; set; }
        int BrandID { get; set; }
        bool BrandIDSpecified { get; set; }
        string City { get; set; }
        Child[] ConsumerChild { get; set; }
        string Country { get; set; }
        bool DMPermission { get; set; }
        bool DMPermissionSpecified { get; set; }
        string EmailAddress { get; set; }
        bool EmailPermission { get; set; }
        bool EmailPermissionSpecified { get; set; }
        string EthnicityCode { get; set; }
        string FirstName { get; set; }
        bool FirstTimeParentFlag { get; set; }
        bool FirstTimeParentFlagSpecified { get; set; }
        string Gender { get; set; }
        string HomePhone { get; set; }
        bool KCBrandsDMPermission { get; set; }
        bool KCBrandsDMPermissionSpecified { get; set; }
        bool KCBrandsEmailpermission { get; set; }
        bool KCBrandsEmailpermissionSpecified { get; set; }
        string LanguageCode { get; set; }
        string LastName { get; set; }
        bool MobilePermission { get; set; }
        bool MobilePermissionSpecified { get; set; }
        string MobilePhone { get; set; }
        int OCDID { get; set; }
        bool OCDIDSpecified { get; set; }
        string PostalCode { get; set; }
        int SourceID { get; set; }
        bool SourceIDSpecified { get; set; }
        string StateProvinceCode { get; set; }
        bool ThirdPartyDMPermission { get; set; }
        bool ThirdPartyDMPermissionSpecified { get; set; }
        bool ThirdPartyEmailpermission { get; set; }
        bool ThirdPartyEmailpermissionSpecified { get; set; }
        int VendorID { get; set; }
        bool VendorIDSpecified { get; set; }
    }
}