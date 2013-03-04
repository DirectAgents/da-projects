using System;
using System.ComponentModel;
using System.CodeDom.Compiler;
using KimberlyClark.Services.Abstract;
using System.Diagnostics;
using System.Xml.Serialization;

namespace KimberlyClark.Services.Concrete
{
    [GeneratedCode("xsd", "4.0.30319.17929")]
    [Serializable]
    [DebuggerStepThrough]
    [DesignerCategory("code")]
#if DEBUG
    [XmlType(Namespace = "")]
    [XmlRoot(Namespace = "", IsNullable = true)]
#else
    [XmlType(Namespace = "")]
    [XmlRoot(Namespace = "", IsNullable = true)]
    //[XmlType(Namespace = "http://schemas.datacontract.org/2004/07/CoRegistrationRestService")]
    //[XmlRoot(Namespace = "http://schemas.datacontract.org/2004/07/CoRegistrationRestService", IsNullable = true)]
#endif
    public class Consumer : IConsumer
    {
        private string address1Field;
        private string address2Field;
        private string birthDateField;
        private int brandIDField;
        private bool brandIDFieldSpecified;
        private string cityField;
        private Child[] consumerChildField;
        private string countryField;
        private bool dMPermissionField;
        private bool dMPermissionFieldSpecified;
        private string emailAddressField;
        private bool emailPermissionField;
        private bool emailPermissionFieldSpecified;
        private string ethnicityCodeField;
        private string firstNameField;
        private bool firstTimeParentFlagField;
        private bool firstTimeParentFlagFieldSpecified;
        private string genderField;
        private string homePhoneField;
        private bool kCBrandsDMPermissionField;
        private bool kCBrandsDMPermissionFieldSpecified;
        private bool kCBrandsEmailpermissionField;
        private bool kCBrandsEmailpermissionFieldSpecified;
        private string languageCodeField;
        private string lastNameField;
        private bool mobilePermissionField;
        private bool mobilePermissionFieldSpecified;
        private string mobilePhoneField;
        private int oCDIDField;
        private bool oCDIDFieldSpecified;
        private string postalCodeField;
        private int sourceIDField;
        private bool sourceIDFieldSpecified;
        private string stateProvinceCodeField;
        private bool thirdPartyDMPermissionField;
        private bool thirdPartyDMPermissionFieldSpecified;
        private bool thirdPartyEmailpermissionField;
        private bool thirdPartyEmailpermissionFieldSpecified;
        private int vendorIDField;
        private bool vendorIDFieldSpecified;

        [XmlElement(IsNullable = true, Order = 0)]
        public string Address1
        {
            get { return this.address1Field; }
            set { this.address1Field = value; }
        }

        [XmlElement(IsNullable = true, Order = 1)]
        public string Address2
        {
            get { return this.address2Field; }
            set { this.address2Field = value; }
        }

        [XmlElement(IsNullable = true, Order = 2)]
        public string BirthDate
        {
            get { return this.birthDateField; }
            set { this.birthDateField = value; }
        }

        [XmlElement(Order = 3)]
        public int BrandID
        {
            get { return this.brandIDField; }
            set { this.brandIDField = value; }
        }

        [XmlIgnore]
        public bool BrandIDSpecified
        {
            get { return this.brandIDFieldSpecified; }
            set { this.brandIDFieldSpecified = value; }
        }

        [XmlElement(IsNullable = true, Order = 4)]
        public string City
        {
            get { return this.cityField; }
            set { this.cityField = value; }
        }

        [XmlArray(IsNullable = true, Order = 5)]
        public Child[] ConsumerChild
        {
            get { return this.consumerChildField; }
            set { this.consumerChildField = value; }
        }

        [XmlElement(IsNullable = true, Order = 6)]
        public string Country
        {
            get { return this.countryField; }
            set { this.countryField = value; }
        }

        [XmlElement(Order = 7)]
        public bool DMPermission
        {
            get { return this.dMPermissionField; }
            set { this.dMPermissionField = value; }
        }

        [XmlIgnore]
        public bool DMPermissionSpecified
        {
            get { return this.dMPermissionFieldSpecified; }
            set { this.dMPermissionFieldSpecified = value; }
        }

        [XmlElement(IsNullable = true, Order = 8)]
        public string EmailAddress
        {
            get { return this.emailAddressField; }
            set { this.emailAddressField = value; }
        }

        [XmlElement(Order = 9)]
        public bool EmailPermission
        {
            get { return this.emailPermissionField; }
            set { this.emailPermissionField = value; }
        }

        [XmlIgnore]
        public bool EmailPermissionSpecified
        {
            get { return this.emailPermissionFieldSpecified; }
            set { this.emailPermissionFieldSpecified = value; }
        }

        [XmlElement(IsNullable = true, Order = 10)]
        public string EthnicityCode
        {
            get { return this.ethnicityCodeField; }
            set { this.ethnicityCodeField = value; }
        }

        [XmlElement(IsNullable = true, Order = 11)]
        public string FirstName
        {
            get { return this.firstNameField; }
            set { this.firstNameField = value; }
        }

        [XmlElement(Order = 12)]
        public bool FirstTimeParentFlag
        {
            get { return this.firstTimeParentFlagField; }
            set { this.firstTimeParentFlagField = value; }
        }

        [XmlIgnore]
        public bool FirstTimeParentFlagSpecified
        {
            get { return this.firstTimeParentFlagFieldSpecified; }
            set { this.firstTimeParentFlagFieldSpecified = value; }
        }

        [XmlElement(IsNullable = true, Order = 13)]
        public string Gender
        {
            get { return this.genderField; }
            set { this.genderField = value; }
        }

        [XmlElement(IsNullable = true, Order = 14)]
        public string HomePhone
        {
            get { return this.homePhoneField; }
            set { this.homePhoneField = value; }
        }

        [XmlElement(Order = 15)]
        public bool KCBrandsDMPermission
        {
            get { return this.kCBrandsDMPermissionField; }
            set { this.kCBrandsDMPermissionField = value; }
        }

        [XmlIgnore]
        public bool KCBrandsDMPermissionSpecified
        {
            get { return this.kCBrandsDMPermissionFieldSpecified; }
            set { this.kCBrandsDMPermissionFieldSpecified = value; }
        }

        [XmlElement(Order = 16)]
        public bool KCBrandsEmailpermission
        {
            get { return this.kCBrandsEmailpermissionField; }
            set { this.kCBrandsEmailpermissionField = value; }
        }

        [XmlIgnore]
        public bool KCBrandsEmailpermissionSpecified
        {
            get { return this.kCBrandsEmailpermissionFieldSpecified; }
            set { this.kCBrandsEmailpermissionFieldSpecified = value; }
        }


        [XmlElement(IsNullable = true, Order = 17)]
        public string LanguageCode
        {
            get { return this.languageCodeField; }
            set { this.languageCodeField = value; }
        }

        [XmlElement(IsNullable = true, Order = 18)]
        public string LastName
        {
            get { return this.lastNameField; }
            set { this.lastNameField = value; }
        }

        [XmlElement(Order = 19)]
        public bool MobilePermission
        {
            get { return this.mobilePermissionField; }
            set { this.mobilePermissionField = value; }
        }

        [XmlIgnore]
        public bool MobilePermissionSpecified
        {
            get { return this.mobilePermissionFieldSpecified; }
            set { this.mobilePermissionFieldSpecified = value; }
        }

        [XmlElement(IsNullable = true, Order = 20)]
        public string MobilePhone
        {
            get { return this.mobilePhoneField; }
            set { this.mobilePhoneField = value; }
        }

        [XmlElement(Order = 21)]
        public int OCDID
        {
            get { return this.oCDIDField; }
            set { this.oCDIDField = value; }
        }

        [XmlIgnore]
        public bool OCDIDSpecified
        {
            get { return this.oCDIDFieldSpecified; }
            set { this.oCDIDFieldSpecified = value; }
        }

        [XmlElement(IsNullable = true, Order = 22)]
        public string PostalCode
        {
            get { return this.postalCodeField; }
            set { this.postalCodeField = value; }
        }

        [XmlElement(Order = 23)]
        public int SourceID
        {
            get { return this.sourceIDField; }
            set { this.sourceIDField = value; }
        }

        [XmlIgnore]
        public bool SourceIDSpecified
        {
            get { return this.sourceIDFieldSpecified; }
            set { this.sourceIDFieldSpecified = value; }
        }

        [XmlElement(IsNullable = true, Order = 24)]
        public string StateProvinceCode
        {
            get { return this.stateProvinceCodeField; }
            set { this.stateProvinceCodeField = value; }
        }

        [XmlElement(Order = 25)]
        public bool ThirdPartyDMPermission
        {
            get { return this.thirdPartyDMPermissionField; }
            set { this.thirdPartyDMPermissionField = value; }
        }

        [XmlIgnore]
        public bool ThirdPartyDMPermissionSpecified
        {
            get { return this.thirdPartyDMPermissionFieldSpecified; }
            set { this.thirdPartyDMPermissionFieldSpecified = value; }
        }

        [XmlElement(Order = 26)]
        public bool ThirdPartyEmailpermission
        {
            get { return this.thirdPartyEmailpermissionField; }
            set { this.thirdPartyEmailpermissionField = value; }
        }

        [XmlIgnore]
        public bool ThirdPartyEmailpermissionSpecified
        {
            get { return this.thirdPartyEmailpermissionFieldSpecified; }
            set { this.thirdPartyEmailpermissionFieldSpecified = value; }
        }

        [XmlElement(Order = 27)]
        public int VendorID
        {
            get { return this.vendorIDField; }
            set { this.vendorIDField = value; }
        }

        [XmlIgnore]
        public bool VendorIDSpecified
        {
            get { return this.vendorIDFieldSpecified; }
            set { this.vendorIDFieldSpecified = value; }
        }

        public override string ToString()
        {
            return Utilities.ToXmlString(this);
        }
    }
}