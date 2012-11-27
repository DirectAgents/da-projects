using System;
using System.IO;
using System.Text;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace LTWeb
{
    public partial class LTRequest
    {
        public LTRequest()
        {
            this.Request = new RequestType();
        }

        public override string ToString()
        {
            try
            {
                var serializer = new XmlSerializer(this.GetType());
                string utf8;
                using (StringWriter writer = new Utf8StringWriter())
                {
                    serializer.Serialize(writer, this);
                    utf8 = writer.ToString();
                }
                return utf8;
            }
            catch (System.Exception ex)
            {
                return ex.Message;
            }
        }

        public static void Create(XElement xe, out LTRequest o)
        {
            var serializer = new XmlSerializer(typeof(LTRequest));
            o = (LTRequest)serializer.Deserialize(xe.CreateReader());
        }

        class Utf8StringWriter : StringWriter
        {
            public Utf8StringWriter()
            {
            }

            public Utf8StringWriter(StringBuilder sb)
                : base()
            {
            }

            public override Encoding Encoding
            {
                get
                {
                    return Encoding.UTF8;
                }
            }
        }
    }

    public partial class RequestType
    {
        public RequestType()
        {
            AppID = Guid.Empty.ToString();

            HomeLoanProduct = new HomeLoanProductType();

            Applicant = new ApplicantType[1];
            Applicant[0] = new ApplicantType();

            RequestType rt = this;
            rt.type = "Refinance";
            rt.created = string.Format("{0:yyyy-MM-dd HH:mm:ss}", DateTime.Now);
            rt.updated = string.Format("{0:yyyy-MM-dd HH:mm:ss}", DateTime.Now);
            rt.VisitorSessionID = "0";
            rt.ElectronicDisclosureConsent = "Y";

            ApplicantType at = Applicant[0];
            at.Password = "1234";
            at.RelationshipToApplicant = RelationshipToApplicantType.SELF;
        }

        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public ApplicantType TheApplicant
        {
            get
            {
                return Applicant[0];
            }
        }

        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public ELoanType LoanType
        {
            get
            {
                try
                {
                    if (HomeLoanProduct.Item is RefinanceType) return ELoanType.REFINANCE;
                    else if (HomeLoanProduct.Item is PurchaseType) return ELoanType.PURCHASE;
                    else throw new Exception("invalid LoanType");
                }
                catch (Exception)
                {
                    throw new Exception("error getting LoanType - never set?");
                }
            }
            set
            {
                switch (value)
                {
                    case ELoanType.REFINANCE:
                        this.type = "Refinance";
                        HomeLoanProduct.Item = new RefinanceType();
                        // TEST TEMP TREE.com will FIX
                        RefinanceType rt = HomeLoanProduct.Item as RefinanceType;
                        rt.RequestedProducts = new[] { ProductType.Item30YRF, ProductType.Item15YRF, ProductType.Item5YRARM };
                        break;
                    case ELoanType.PURCHASE:
                        this.type = "Purchase";
                        HomeLoanProduct.Item = new PurchaseType();
                        break;
                    default:
                        break;
                }
            }
        }
    }

    public enum ELoanType
    {
        REFINANCE,
        PURCHASE
    }

    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "", IsNullable = false, ElementName = "LendingTreeAffiliateRequest")]
    public partial class LTRequest : object, System.ComponentModel.INotifyPropertyChanged
    {

        private RequestType requestField;

        private ResponseType responseField;


        public RequestType Request
        {
            get
            {
                return this.requestField;
            }
            set
            {
                this.requestField = value;
                this.RaisePropertyChanged("Request");
            }
        }


        public ResponseType Response
        {
            get
            {
                return this.responseField;
            }
            set
            {
                this.responseField = value;
                this.RaisePropertyChanged("Response");
            }
        }

        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

        protected void RaisePropertyChanged(string propertyName)
        {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null))
            {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }

    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class RequestType : object, System.ComponentModel.INotifyPropertyChanged
    {

        private SourceOfRequestType sourceOfRequestField;

        private HomeLoanProductType homeLoanProductField;

        private ApplicantType[] applicantField;

        private string typeField;

        private string createdField;

        private string updatedField;

        private string visitorSessionIDField;

        private string appIDField;

        private string electronicDisclosureConsentField;


        public SourceOfRequestType SourceOfRequest
        {
            get
            {
                return this.sourceOfRequestField;
            }
            set
            {
                this.sourceOfRequestField = value;
                this.RaisePropertyChanged("SourceOfRequest");
            }
        }


        public HomeLoanProductType HomeLoanProduct
        {
            get
            {
                return this.homeLoanProductField;
            }
            set
            {
                this.homeLoanProductField = value;
                this.RaisePropertyChanged("HomeLoanProduct");
            }
        }


        [System.Xml.Serialization.XmlElementAttribute("Applicant")]
        public ApplicantType[] Applicant
        {
            get
            {
                return this.applicantField;
            }
            set
            {
                this.applicantField = value;
                this.RaisePropertyChanged("Applicant");
            }
        }


        [System.Xml.Serialization.XmlAttributeAttribute(Form = System.Xml.Schema.XmlSchemaForm.Qualified)]
        public string type
        {
            get
            {
                return this.typeField;
            }
            set
            {
                this.typeField = value;
                this.RaisePropertyChanged("type");
            }
        }


        [System.Xml.Serialization.XmlAttributeAttribute(Form = System.Xml.Schema.XmlSchemaForm.Qualified)]
        public string created
        {
            get
            {
                return this.createdField;
            }
            set
            {
                this.createdField = value;
                this.RaisePropertyChanged("created");
            }
        }


        [System.Xml.Serialization.XmlAttributeAttribute(Form = System.Xml.Schema.XmlSchemaForm.Qualified)]
        public string updated
        {
            get
            {
                return this.updatedField;
            }
            set
            {
                this.updatedField = value;
                this.RaisePropertyChanged("updated");
            }
        }


        [System.Xml.Serialization.XmlAttributeAttribute(Form = System.Xml.Schema.XmlSchemaForm.Qualified)]
        public string VisitorSessionID
        {
            get
            {
                return this.visitorSessionIDField;
            }
            set
            {
                this.visitorSessionIDField = value;
                this.RaisePropertyChanged("VisitorSessionID");
            }
        }


        [System.Xml.Serialization.XmlAttributeAttribute(Form = System.Xml.Schema.XmlSchemaForm.Qualified)]
        public string AppID
        {
            get
            {
                return this.appIDField;
            }
            set
            {
                this.appIDField = value;
                this.RaisePropertyChanged("AppID");
            }
        }


        [System.Xml.Serialization.XmlAttributeAttribute(Form = System.Xml.Schema.XmlSchemaForm.Qualified)]
        public string ElectronicDisclosureConsent
        {
            get
            {
                return this.electronicDisclosureConsentField;
            }
            set
            {
                this.electronicDisclosureConsentField = value;
                this.RaisePropertyChanged("ElectronicDisclosureConsent");
            }
        }

        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

        protected void RaisePropertyChanged(string propertyName)
        {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null))
            {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }

    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "", IsNullable = false)]
    public partial class SourceOfRequestType : object, System.ComponentModel.INotifyPropertyChanged
    {

        private string lendingTreeAffiliatePartnerCodeField;

        private string lendingTreeAffiliateUserNameField;

        private string lendingTreeAffiliatePasswordField;

        private string lendingTreeAffiliateEsourceIDField;

        private string lendingTreeAffiliateBrandField;

        private string lendingTreeAffiliateFormVersionField;

        private string visitorIPAddressField;

        private string visitorURLField;

        private string treeSessionIDField;

        private string treeComputerIDField;

        private string v1stCookieField;

        private YesNoType lTLOptinField;

        private string affiliateSiteID;



        public string LendingTreeAffiliatePartnerCode
        {
            get
            {
                return this.lendingTreeAffiliatePartnerCodeField;
            }
            set
            {
                this.lendingTreeAffiliatePartnerCodeField = value;
                this.RaisePropertyChanged("LendingTreeAffiliatePartnerCode");
            }
        }


        public string LendingTreeAffiliateUserName
        {
            get
            {
                return this.lendingTreeAffiliateUserNameField;
            }
            set
            {
                this.lendingTreeAffiliateUserNameField = value;
                this.RaisePropertyChanged("LendingTreeAffiliateUserName");
            }
        }


        public string LendingTreeAffiliatePassword
        {
            get
            {
                return this.lendingTreeAffiliatePasswordField;
            }
            set
            {
                this.lendingTreeAffiliatePasswordField = value;
                this.RaisePropertyChanged("LendingTreeAffiliatePassword");
            }
        }


        public string LendingTreeAffiliateEsourceID
        {
            get
            {
                return this.lendingTreeAffiliateEsourceIDField;
            }
            set
            {
                this.lendingTreeAffiliateEsourceIDField = value;
                this.RaisePropertyChanged("LendingTreeAffiliateEsourceID");
            }
        }


        public string LendingTreeAffiliateBrand
        {
            get
            {
                return this.lendingTreeAffiliateBrandField;
            }
            set
            {
                this.lendingTreeAffiliateBrandField = value;
                this.RaisePropertyChanged("LendingTreeAffiliateBrand");
            }
        }


        public string LendingTreeAffiliateFormVersion
        {
            get
            {
                return this.lendingTreeAffiliateFormVersionField;
            }
            set
            {
                this.lendingTreeAffiliateFormVersionField = value;
                this.RaisePropertyChanged("LendingTreeAffiliateFormVersion");
            }
        }


        public string VisitorIPAddress
        {
            get
            {
                return this.visitorIPAddressField;
            }
            set
            {
                this.visitorIPAddressField = value;
                this.RaisePropertyChanged("VisitorIPAddress");
            }
        }


        public string VisitorURL
        {
            get
            {
                return this.visitorURLField;
            }
            set
            {
                this.visitorURLField = value;
                this.RaisePropertyChanged("VisitorURL");
            }
        }


        public string TreeSessionID
        {
            get
            {
                return this.treeSessionIDField;
            }
            set
            {
                this.treeSessionIDField = value;
                this.RaisePropertyChanged("TreeSessionID");
            }
        }


        public string TreeComputerID
        {
            get
            {
                return this.treeComputerIDField;
            }
            set
            {
                this.treeComputerIDField = value;
                this.RaisePropertyChanged("TreeComputerID");
            }
        }


        public string V1stCookie
        {
            get
            {
                return this.v1stCookieField;
            }
            set
            {
                this.v1stCookieField = value;
                this.RaisePropertyChanged("V1stCookie");
            }
        }


        public YesNoType LTLOptin
        {
            get
            {
                return this.lTLOptinField;
            }
            set
            {
                this.lTLOptinField = value;
                this.RaisePropertyChanged("LTLOptin");
            }
        }


        public string AffiliateSiteID
        {
            get
            {
                return this.affiliateSiteID;
            }
            set
            {
                this.affiliateSiteID = value;
                this.RaisePropertyChanged("AffiliateSiteID");
            }
        }

        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

        protected void RaisePropertyChanged(string propertyName)
        {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null))
            {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }

    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
    [System.SerializableAttribute()]
    public enum YesNoType
    {


        Y,


        N,
    }

    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class ErrorsType : object, System.ComponentModel.INotifyPropertyChanged
    {

        private string errorField;


        public string Error
        {
            get
            {
                return this.errorField;
            }
            set
            {
                this.errorField = value;
                this.RaisePropertyChanged("Error");
            }
        }

        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

        protected void RaisePropertyChanged(string propertyName)
        {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null))
            {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }

    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class ResponseType : object, System.ComponentModel.INotifyPropertyChanged
    {

        private string returnURLField;

        private ErrorsType errorsField;


        public string ReturnURL
        {
            get
            {
                return this.returnURLField;
            }
            set
            {
                this.returnURLField = value;
                this.RaisePropertyChanged("ReturnURL");
            }
        }


        public ErrorsType Errors
        {
            get
            {
                return this.errorsField;
            }
            set
            {
                this.errorsField = value;
                this.RaisePropertyChanged("Errors");
            }
        }

        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

        protected void RaisePropertyChanged(string propertyName)
        {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null))
            {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }

    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class CreditHistoryType : object, System.ComponentModel.INotifyPropertyChanged
    {

        private CreditHistoryTypeCreditSelfRating creditSelfRatingField;

        private YesNoType declaredBankruptcyField;

        private bool declaredBankruptcyFieldSpecified;

        private YesNoType declaredForeclosureField;

        private bool declaredForeclosureFieldSpecified;

        private CreditHistoryTypeBankruptcyDischarged bankruptcyDischargedField;

        private CreditHistoryTypeForeclosureDischarged foreclosureDischargedField;


        public CreditHistoryTypeCreditSelfRating CreditSelfRating
        {
            get
            {
                return this.creditSelfRatingField;
            }
            set
            {
                this.creditSelfRatingField = value;
                this.RaisePropertyChanged("CreditSelfRating");
            }
        }


        public YesNoType DeclaredBankruptcy
        {
            get
            {
                return this.declaredBankruptcyField;
            }
            set
            {
                this.declaredBankruptcyField = value;
                this.RaisePropertyChanged("DeclaredBankruptcy");
            }
        }


        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool DeclaredBankruptcySpecified
        {
            get
            {
                return this.declaredBankruptcyFieldSpecified;
            }
            set
            {
                this.declaredBankruptcyFieldSpecified = value;
                this.RaisePropertyChanged("DeclaredBankruptcySpecified");
            }
        }


        public YesNoType DeclaredForeclosure
        {
            get
            {
                return this.declaredForeclosureField;
            }
            set
            {
                this.declaredForeclosureField = value;
                this.RaisePropertyChanged("DeclaredForeclosure");
            }
        }


        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool DeclaredForeclosureSpecified
        {
            get
            {
                return this.declaredForeclosureFieldSpecified;
            }
            set
            {
                this.declaredForeclosureFieldSpecified = value;
                this.RaisePropertyChanged("DeclaredForeclosureSpecified");
            }
        }


        public CreditHistoryTypeBankruptcyDischarged BankruptcyDischarged
        {
            get
            {
                return this.bankruptcyDischargedField;
            }
            set
            {
                this.bankruptcyDischargedField = value;
                this.RaisePropertyChanged("BankruptcyDischarged");
            }
        }


        public CreditHistoryTypeForeclosureDischarged ForeclosureDischarged
        {
            get
            {
                return this.foreclosureDischargedField;
            }
            set
            {
                this.foreclosureDischargedField = value;
                this.RaisePropertyChanged("ForeclosureDischarged");
            }
        }

        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

        protected void RaisePropertyChanged(string propertyName)
        {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null))
            {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }

    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public enum CreditHistoryTypeCreditSelfRating
    {
        SOMECREDITPROBLEMS,

        EXCELLENT,

        MAJORCREDITPROBLEMS,

        LITTLEORNOCREDITHISTORY,
    }

    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public enum CreditHistoryTypeBankruptcyDischarged
    {
        NEVER,

        NOT_YET_DISCHARGED,


        [System.Xml.Serialization.XmlEnumAttribute("01-12_MONTHS")]
        Item0112_MONTHS,


        [System.Xml.Serialization.XmlEnumAttribute("13-24_MONTHS")]
        Item1324_MONTHS,


        [System.Xml.Serialization.XmlEnumAttribute("25-36_MONTHS")]
        Item2536_MONTHS,


        [System.Xml.Serialization.XmlEnumAttribute("37-48_MONTHS")]
        Item3748_MONTHS,


        [System.Xml.Serialization.XmlEnumAttribute("49-60_MONTHS")]
        Item4960_MONTHS,


        [System.Xml.Serialization.XmlEnumAttribute("61-72_MONTHS")]
        Item6172_MONTHS,


        OVER_72_MONTHS,


        OVER_84_MONTHS,

    }

    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public enum CreditHistoryTypeForeclosureDischarged
    {
        NEVER,

        CURRENTLY_IN_FORECLOSURE,


        [System.Xml.Serialization.XmlEnumAttribute("01-12_MONTHS")]
        Item0112_MONTHS,


        [System.Xml.Serialization.XmlEnumAttribute("13-24_MONTHS")]
        Item1324_MONTHS,


        [System.Xml.Serialization.XmlEnumAttribute("25-36_MONTHS")]
        Item2536_MONTHS,


        [System.Xml.Serialization.XmlEnumAttribute("37-48_MONTHS")]
        Item3748_MONTHS,


        [System.Xml.Serialization.XmlEnumAttribute("49-60_MONTHS")]
        Item4960_MONTHS,


        [System.Xml.Serialization.XmlEnumAttribute("61-72_MONTHS")]
        Item6172_MONTHS,


        OVER_72_MONTHS,


        OVER_84_MONTHS,

    }

    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class EmploymentType : object, System.ComponentModel.INotifyPropertyChanged
    {

        private string employerNameField;

        private string employeeTitleField;

        private string employmentYearsField;

        private EmploymentStausType employmentStatusField;

        private bool employmentStatusFieldSpecified;

        private decimal employmentIncomeField;

        private bool employmentIncomeFieldSpecified;

        private string employmentIndicatorField;


        public string EmployerName
        {
            get
            {
                return this.employerNameField;
            }
            set
            {
                this.employerNameField = value;
                this.RaisePropertyChanged("EmployerName");
            }
        }


        public string EmployeeTitle
        {
            get
            {
                return this.employeeTitleField;
            }
            set
            {
                this.employeeTitleField = value;
                this.RaisePropertyChanged("EmployeeTitle");
            }
        }


        public string EmploymentYears
        {
            get
            {
                return this.employmentYearsField;
            }
            set
            {
                this.employmentYearsField = value;
                this.RaisePropertyChanged("EmploymentYears");
            }
        }


        public EmploymentStausType EmploymentStatus
        {
            get
            {
                return this.employmentStatusField;
            }
            set
            {
                this.employmentStatusField = value;
                this.RaisePropertyChanged("EmploymentStatus");
            }
        }


        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool EmploymentStatusSpecified
        {
            get
            {
                return this.employmentStatusFieldSpecified;
            }
            set
            {
                this.employmentStatusFieldSpecified = value;
                this.RaisePropertyChanged("EmploymentStatusSpecified");
            }
        }


        public decimal EmploymentIncome
        {
            get
            {
                return this.employmentIncomeField;
            }
            set
            {
                this.employmentIncomeField = value;
                this.RaisePropertyChanged("EmploymentIncome");
            }
        }


        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool EmploymentIncomeSpecified
        {
            get
            {
                return this.employmentIncomeFieldSpecified;
            }
            set
            {
                this.employmentIncomeFieldSpecified = value;
                this.RaisePropertyChanged("EmploymentIncomeSpecified");
            }
        }


        [System.Xml.Serialization.XmlAttributeAttribute(Form = System.Xml.Schema.XmlSchemaForm.Qualified)]
        public string EmploymentIndicator
        {
            get
            {
                return this.employmentIndicatorField;
            }
            set
            {
                this.employmentIndicatorField = value;
                this.RaisePropertyChanged("EmploymentIndicator");
            }
        }

        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

        protected void RaisePropertyChanged(string propertyName)
        {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null))
            {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }

    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
    [System.SerializableAttribute()]
    public enum EmploymentStausType
    {


        FULLTIME,


        HOMEMAKER,


        PARTTIME,


        RETIRED,


        SELFEMPLOYED,


        STUDENT,
    }

    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class ContactPreferenceType : object, System.ComponentModel.INotifyPropertyChanged
    {

        private ContactPreferenceTypePreferredContactPlace preferredContactPlaceField;

        private bool preferredContactPlaceFieldSpecified;

        private ContactPreferenceTypePreferredContactMethod preferredContactMethodField;

        private bool preferredContactMethodFieldSpecified;

        private ContactPreferenceTypePreferredContactTime preferredContactTimeField;

        private bool preferredContactTimeFieldSpecified;


        [System.Xml.Serialization.XmlAttributeAttribute(Form = System.Xml.Schema.XmlSchemaForm.Qualified)]
        public ContactPreferenceTypePreferredContactPlace PreferredContactPlace
        {
            get
            {
                return this.preferredContactPlaceField;
            }
            set
            {
                this.preferredContactPlaceField = value;
                this.RaisePropertyChanged("PreferredContactPlace");
            }
        }


        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool PreferredContactPlaceSpecified
        {
            get
            {
                return this.preferredContactPlaceFieldSpecified;
            }
            set
            {
                this.preferredContactPlaceFieldSpecified = value;
                this.RaisePropertyChanged("PreferredContactPlaceSpecified");
            }
        }


        [System.Xml.Serialization.XmlAttributeAttribute(Form = System.Xml.Schema.XmlSchemaForm.Qualified)]
        public ContactPreferenceTypePreferredContactMethod PreferredContactMethod
        {
            get
            {
                return this.preferredContactMethodField;
            }
            set
            {
                this.preferredContactMethodField = value;
                this.RaisePropertyChanged("PreferredContactMethod");
            }
        }


        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool PreferredContactMethodSpecified
        {
            get
            {
                return this.preferredContactMethodFieldSpecified;
            }
            set
            {
                this.preferredContactMethodFieldSpecified = value;
                this.RaisePropertyChanged("PreferredContactMethodSpecified");
            }
        }


        [System.Xml.Serialization.XmlAttributeAttribute(Form = System.Xml.Schema.XmlSchemaForm.Qualified)]
        public ContactPreferenceTypePreferredContactTime PreferredContactTime
        {
            get
            {
                return this.preferredContactTimeField;
            }
            set
            {
                this.preferredContactTimeField = value;
                this.RaisePropertyChanged("PreferredContactTime");
            }
        }


        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool PreferredContactTimeSpecified
        {
            get
            {
                return this.preferredContactTimeFieldSpecified;
            }
            set
            {
                this.preferredContactTimeFieldSpecified = value;
                this.RaisePropertyChanged("PreferredContactTimeSpecified");
            }
        }

        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

        protected void RaisePropertyChanged(string propertyName)
        {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null))
            {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }

    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public enum ContactPreferenceTypePreferredContactPlace
    {


        Home,


        Work,
    }

    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public enum ContactPreferenceTypePreferredContactMethod
    {


        Any,


        Email,


        Fax,


        Phone,
    }

    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public enum ContactPreferenceTypePreferredContactTime
    {


        Anytime,


        Morning,


        Afternoon,


        Evening,
    }

    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class ApplicantType : object, System.ComponentModel.INotifyPropertyChanged
    {

        private string firstNameField;

        private string middleNameField;

        private string lastNameField;

        private NameSuffixType nameSuffixField;

        private bool nameSuffixFieldSpecified;

        private string streetField;

        private string cityField;

        private string countyField;

        private StateType stateField;

        private bool stateFieldSpecified;

        private string zipField;

        private string dateOfBirthField;

        private string homePhoneField;

        private string mobilePhoneField;

        private string workPhoneField;

        private int workPhoneExtField;

        private bool workPhoneExtFieldSpecified;

        private string emailAddressField;

        private string passwordField;

        private string sSNField;

        private YesNoType isVeteranField;

        private MaritalStatusType maritalStatusField;

        private bool maritalStatusFieldSpecified;

        private CitizenshipStatusType isUSCitizenField;

        private bool isUSCitizenFieldSpecified;

        private RelationshipToApplicantType relationshipToApplicantField;

        private bool relationshipToApplicantFieldSpecified;

        private ContactPreferenceType contactPreferenceField;

        private EmploymentType employmentField;

        private CreditHistoryType creditHistoryField;

        private ApplicantTypePrimary primaryField;

        private ApplicantTypePrimaryContact primaryContactField;

        public ApplicantType()
        {
            this.isVeteranField = YesNoType.N;
        }


        [System.Xml.Serialization.XmlElementAttribute(DataType = "normalizedString")]
        public string FirstName
        {
            get
            {
                return this.firstNameField;
            }
            set
            {
                this.firstNameField = value;
                this.RaisePropertyChanged("FirstName");
            }
        }


        [System.Xml.Serialization.XmlElementAttribute(DataType = "normalizedString")]
        public string MiddleName
        {
            get
            {
                return this.middleNameField;
            }
            set
            {
                this.middleNameField = value;
                this.RaisePropertyChanged("MiddleName");
            }
        }


        [System.Xml.Serialization.XmlElementAttribute(DataType = "normalizedString")]
        public string LastName
        {
            get
            {
                return this.lastNameField;
            }
            set
            {
                this.lastNameField = value;
                this.RaisePropertyChanged("LastName");
            }
        }


        public NameSuffixType NameSuffix
        {
            get
            {
                return this.nameSuffixField;
            }
            set
            {
                this.nameSuffixField = value;
                this.RaisePropertyChanged("NameSuffix");
            }
        }


        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool NameSuffixSpecified
        {
            get
            {
                return this.nameSuffixFieldSpecified;
            }
            set
            {
                this.nameSuffixFieldSpecified = value;
                this.RaisePropertyChanged("NameSuffixSpecified");
            }
        }


        [System.Xml.Serialization.XmlElementAttribute(DataType = "normalizedString")]
        public string Street
        {
            get
            {
                return this.streetField;
            }
            set
            {
                this.streetField = value;
                this.RaisePropertyChanged("Street");
            }
        }


        [System.Xml.Serialization.XmlElementAttribute(DataType = "normalizedString")]
        public string City
        {
            get
            {
                return this.cityField;
            }
            set
            {
                this.cityField = value;
                this.RaisePropertyChanged("City");
            }
        }


        [System.Xml.Serialization.XmlElementAttribute(DataType = "normalizedString")]
        public string County
        {
            get
            {
                return this.countyField;
            }
            set
            {
                this.countyField = value;
                this.RaisePropertyChanged("County");
            }
        }


        public StateType State
        {
            get
            {
                return this.stateField;
            }
            set
            {
                this.stateField = value;
                this.stateFieldSpecified = true;
                this.RaisePropertyChanged("State");
            }
        }


        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool StateSpecified
        {
            get
            {
                return this.stateFieldSpecified;
            }
            set
            {
                this.stateFieldSpecified = value;
                this.RaisePropertyChanged("StateSpecified");
            }
        }


        public string Zip
        {
            get
            {
                return this.zipField;
            }
            set
            {
                this.zipField = value;
                this.RaisePropertyChanged("Zip");
            }
        }


        public string DateOfBirth
        {
            get
            {
                return this.dateOfBirthField;
            }
            set
            {
                this.dateOfBirthField = value;
                this.RaisePropertyChanged("DateOfBirth");
            }
        }


        public string HomePhone
        {
            get
            {
                return this.homePhoneField;
            }
            set
            {
                this.homePhoneField = value;
                this.RaisePropertyChanged("HomePhone");
            }
        }


        public string MobilePhone
        {
            get
            {
                return this.mobilePhoneField;
            }
            set
            {
                this.mobilePhoneField = value;
                this.RaisePropertyChanged("MobilePhone");
            }
        }


        public string WorkPhone
        {
            get
            {
                return this.workPhoneField;
            }
            set
            {
                this.workPhoneField = value;
                this.RaisePropertyChanged("WorkPhone");
            }
        }


        public int WorkPhoneExt
        {
            get
            {
                return this.workPhoneExtField;
            }
            set
            {
                this.workPhoneExtField = value;
                this.RaisePropertyChanged("WorkPhoneExt");
            }
        }


        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool WorkPhoneExtSpecified
        {
            get
            {
                return this.workPhoneExtFieldSpecified;
            }
            set
            {
                this.workPhoneExtFieldSpecified = value;
                this.RaisePropertyChanged("WorkPhoneExtSpecified");
            }
        }


        public string EmailAddress
        {
            get
            {
                return this.emailAddressField;
            }
            set
            {
                this.emailAddressField = value;
                this.RaisePropertyChanged("EmailAddress");
            }
        }


        public string Password
        {
            get
            {
                return this.passwordField;
            }
            set
            {
                this.passwordField = value;
                this.RaisePropertyChanged("Password");
            }
        }


        public string SSN
        {
            get
            {
                return this.sSNField;
            }
            set
            {
                this.sSNField = value;
                this.RaisePropertyChanged("SSN");
            }
        }


        public YesNoType IsVeteran
        {
            get
            {
                return this.isVeteranField;
            }
            set
            {
                this.isVeteranField = value;
                this.RaisePropertyChanged("IsVeteran");
            }
        }


        public MaritalStatusType MaritalStatus
        {
            get
            {
                return this.maritalStatusField;
            }
            set
            {
                this.maritalStatusField = value;
                this.RaisePropertyChanged("MaritalStatus");
            }
        }


        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool MaritalStatusSpecified
        {
            get
            {
                return this.maritalStatusFieldSpecified;
            }
            set
            {
                this.maritalStatusFieldSpecified = value;
                this.RaisePropertyChanged("MaritalStatusSpecified");
            }
        }


        public CitizenshipStatusType IsUSCitizen
        {
            get
            {
                return this.isUSCitizenField;
            }
            set
            {
                this.isUSCitizenField = value;
                this.RaisePropertyChanged("IsUSCitizen");
            }
        }


        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool IsUSCitizenSpecified
        {
            get
            {
                return this.isUSCitizenFieldSpecified;
            }
            set
            {
                this.isUSCitizenFieldSpecified = value;
                this.RaisePropertyChanged("IsUSCitizenSpecified");
            }
        }


        public RelationshipToApplicantType RelationshipToApplicant
        {
            get
            {
                return this.relationshipToApplicantField;
            }
            set
            {
                this.relationshipToApplicantField = value;
                this.RaisePropertyChanged("RelationshipToApplicant");
            }
        }


        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool RelationshipToApplicantSpecified
        {
            get
            {
                return this.relationshipToApplicantFieldSpecified;
            }
            set
            {
                this.relationshipToApplicantFieldSpecified = value;
                this.RaisePropertyChanged("RelationshipToApplicantSpecified");
            }
        }


        public ContactPreferenceType ContactPreference
        {
            get
            {
                return this.contactPreferenceField;
            }
            set
            {
                this.contactPreferenceField = value;
                this.RaisePropertyChanged("ContactPreference");
            }
        }


        public EmploymentType Employment
        {
            get
            {
                return this.employmentField;
            }
            set
            {
                this.employmentField = value;
                this.RaisePropertyChanged("Employment");
            }
        }


        public CreditHistoryType CreditHistory
        {
            get
            {
                if (creditHistoryField == null)
                    creditHistoryField = new CreditHistoryType();

                return this.creditHistoryField;
            }
            set
            {
                this.creditHistoryField = value;
                this.RaisePropertyChanged("CreditHistory");
            }
        }


        [System.Xml.Serialization.XmlAttributeAttribute(Form = System.Xml.Schema.XmlSchemaForm.Qualified)]
        public ApplicantTypePrimary Primary
        {
            get
            {
                return this.primaryField;
            }
            set
            {
                this.primaryField = value;
                this.RaisePropertyChanged("Primary");
            }
        }


        [System.Xml.Serialization.XmlAttributeAttribute(Form = System.Xml.Schema.XmlSchemaForm.Qualified)]
        public ApplicantTypePrimaryContact PrimaryContact
        {
            get
            {
                return this.primaryContactField;
            }
            set
            {
                this.primaryContactField = value;
                this.RaisePropertyChanged("PrimaryContact");
            }
        }

        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

        protected void RaisePropertyChanged(string propertyName)
        {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null))
            {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }

    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
    [System.SerializableAttribute()]
    public enum NameSuffixType
    {


        I,


        II,


        III,


        IV,


        JR,


        SR,


        V,
    }


    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
    [System.SerializableAttribute()]
    public enum StateType
    {


        AK,


        AL,


        AR,


        AZ,


        CA,


        CO,


        CT,


        DE,


        DC,


        FL,


        GA,


        HI,


        ID,


        IL,


        IN,


        IA,


        KS,


        KY,


        LA,


        ME,


        MD,


        MA,


        MI,


        MN,


        MS,


        MO,


        MT,


        NE,


        NV,


        NH,


        NJ,


        NM,


        NY,


        NC,


        ND,


        OH,


        OK,


        OR,


        PA,


        RI,


        SC,


        SD,


        TN,


        TX,


        UT,


        VT,


        VA,


        WA,


        WV,


        WI,


        WY,
    }


    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
    [System.SerializableAttribute()]
    public enum MaritalStatusType
    {


        MARRIED,


        UNMARRIED,


        SEPARATED,
    }


    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
    [System.SerializableAttribute()]
    public enum CitizenshipStatusType
    {


        NONPERMANENTRESIDENTALIEN,


        NONRESIDENTALIEN,


        PERMANENTRESIDENTALIEN,


        USCITIZEN,
    }


    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
    [System.SerializableAttribute()]
    public enum RelationshipToApplicantType
    {


        SPOUSE,


        PARENT,


        SELF,


        OTHER,
    }


    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public enum ApplicantTypePrimary
    {


        Y,


        N,
    }


    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public enum ApplicantTypePrimaryContact
    {


        Y,


        N,
    }


    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class HomeEquitySubjectPropertyType : object, System.ComponentModel.INotifyPropertyChanged
    {

        private PropertyType propertyTypeField;

        private PropertyUseType propertyUseField;

        private string propertyAddressField;

        private StateType propertyStateField;

        private bool propertyStateFieldSpecified;

        private string propertyCountyField;

        private string propertyCityField;

        private string propertyZipField;

        private UnitsType unitsField;

        private bool unitsFieldSpecified;

        public HomeEquitySubjectPropertyType()
        {
            this.propertyTypeField = PropertyType.SINGLEFAMDET;
            this.propertyUseField = PropertyUseType.OWNEROCCUPIED;
        }


        public PropertyType PropertyType
        {
            get
            {
                return this.propertyTypeField;
            }
            set
            {
                this.propertyTypeField = value;
                this.RaisePropertyChanged("PropertyType");
            }
        }


        public PropertyUseType PropertyUse
        {
            get
            {
                return this.propertyUseField;
            }
            set
            {
                this.propertyUseField = value;
                this.RaisePropertyChanged("PropertyUse");
            }
        }


        public string PropertyAddress
        {
            get
            {
                return this.propertyAddressField;
            }
            set
            {
                this.propertyAddressField = value;
                this.RaisePropertyChanged("PropertyAddress");
            }
        }


        public StateType PropertyState
        {
            get
            {
                return this.propertyStateField;
            }
            set
            {
                this.propertyStateField = value;
                this.RaisePropertyChanged("PropertyState");
            }
        }


        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool PropertyStateSpecified
        {
            get
            {
                return this.propertyStateFieldSpecified;
            }
            set
            {
                this.propertyStateFieldSpecified = value;
                this.RaisePropertyChanged("PropertyStateSpecified");
            }
        }


        public string PropertyCounty
        {
            get
            {
                return this.propertyCountyField;
            }
            set
            {
                this.propertyCountyField = value;
                this.RaisePropertyChanged("PropertyCounty");
            }
        }


        public string PropertyCity
        {
            get
            {
                return this.propertyCityField;
            }
            set
            {
                this.propertyCityField = value;
                this.RaisePropertyChanged("PropertyCity");
            }
        }


        public string PropertyZip
        {
            get
            {
                return this.propertyZipField;
            }
            set
            {
                this.propertyZipField = value;
                this.RaisePropertyChanged("PropertyZip");
            }
        }


        public UnitsType Units
        {
            get
            {
                return this.unitsField;
            }
            set
            {
                this.unitsField = value;
                this.RaisePropertyChanged("Units");
            }
        }


        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool UnitsSpecified
        {
            get
            {
                return this.unitsFieldSpecified;
            }
            set
            {
                this.unitsFieldSpecified = value;
                this.RaisePropertyChanged("UnitsSpecified");
            }
        }

        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

        protected void RaisePropertyChanged(string propertyName)
        {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null))
            {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }


    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
    [System.SerializableAttribute()]
    public enum PropertyType
    {


        SINGLEFAMDET,


        SINGLEFAMATT,


        LOWRISECONDO,


        HIGHRISECONDO,


        [System.Xml.Serialization.XmlEnumAttribute("2TO4UNITFAM")]
        Item2TO4UNITFAM,


        COOP,


        MODULAR,


        MOBILEPERMANENT,


        MOBILEMOVEABLE,


        MANUFACTURED,
    }


    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
    [System.SerializableAttribute()]
    public enum PropertyUseType
    {


        OWNEROCCUPIED,


        SECONDHOME,


        INVESTMENTPROPERTY,
    }


    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
    [System.SerializableAttribute()]
    public enum UnitsType
    {


        [System.Xml.Serialization.XmlEnumAttribute("1UNIT")]
        Item1UNIT,


        [System.Xml.Serialization.XmlEnumAttribute("2UNITS")]
        Item2UNITS,


        [System.Xml.Serialization.XmlEnumAttribute("3UNITS")]
        Item3UNITS,


        [System.Xml.Serialization.XmlEnumAttribute("4UNITS")]
        Item4UNITS,


        [System.Xml.Serialization.XmlEnumAttribute("5ORMOREUNITS")]
        Item5ORMOREUNITS,
    }


    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class HomeEquityType : object, System.ComponentModel.INotifyPropertyChanged
    {

        private YesNoType firstHomeField;

        private bool firstHomeFieldSpecified;

        private decimal propertyEstimatedValueField;

        private decimal monthlyPaymentField;

        private bool monthlyPaymentFieldSpecified;

        private decimal cashOutField;

        private ProductType[] requestedProductsField;

        private HomeEquityPurposesType[] homeEquityPurposeField;

        private HomeEquitySubjectPropertyType subjectPropertyField;


        public YesNoType FirstHome
        {
            get
            {
                return this.firstHomeField;
            }
            set
            {
                this.firstHomeField = value;
                this.RaisePropertyChanged("FirstHome");
            }
        }


        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool FirstHomeSpecified
        {
            get
            {
                return this.firstHomeFieldSpecified;
            }
            set
            {
                this.firstHomeFieldSpecified = value;
                this.RaisePropertyChanged("FirstHomeSpecified");
            }
        }


        public decimal PropertyEstimatedValue
        {
            get
            {
                return this.propertyEstimatedValueField;
            }
            set
            {
                this.propertyEstimatedValueField = value;
                this.RaisePropertyChanged("PropertyEstimatedValue");
            }
        }


        public decimal MonthlyPayment
        {
            get
            {
                return this.monthlyPaymentField;
            }
            set
            {
                this.monthlyPaymentField = value;
                this.RaisePropertyChanged("MonthlyPayment");
            }
        }


        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool MonthlyPaymentSpecified
        {
            get
            {
                return this.monthlyPaymentFieldSpecified;
            }
            set
            {
                this.monthlyPaymentFieldSpecified = value;
                this.RaisePropertyChanged("MonthlyPaymentSpecified");
            }
        }


        public decimal CashOut
        {
            get
            {
                return this.cashOutField;
            }
            set
            {
                this.cashOutField = value;
                this.RaisePropertyChanged("CashOut");
            }
        }


        [System.Xml.Serialization.XmlArrayItemAttribute("Product", IsNullable = false)]
        public ProductType[] RequestedProducts
        {
            get
            {
                return this.requestedProductsField;
            }
            set
            {
                this.requestedProductsField = value;
                this.RaisePropertyChanged("RequestedProducts");
            }
        }


        [System.Xml.Serialization.XmlArrayItemAttribute("Purpose", IsNullable = false)]
        public HomeEquityPurposesType[] HomeEquityPurpose
        {
            get
            {
                return this.homeEquityPurposeField;
            }
            set
            {
                this.homeEquityPurposeField = value;
                this.RaisePropertyChanged("HomeEquityPurpose");
            }
        }


        public HomeEquitySubjectPropertyType SubjectProperty
        {
            get
            {
                return this.subjectPropertyField;
            }
            set
            {
                this.subjectPropertyField = value;
                this.RaisePropertyChanged("SubjectProperty");
            }
        }

        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

        protected void RaisePropertyChanged(string propertyName)
        {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null))
            {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }


    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
    [System.SerializableAttribute()]
    public enum ProductType
    {


        [System.Xml.Serialization.XmlEnumAttribute("5YRFHELOAN")]
        Item5YRFHELOAN,


        [System.Xml.Serialization.XmlEnumAttribute("5YRVHELOAN")]
        Item5YRVHELOAN,


        [System.Xml.Serialization.XmlEnumAttribute("5YRFHELOC")]
        Item5YRFHELOC,


        [System.Xml.Serialization.XmlEnumAttribute("5YRVHELOC")]
        Item5YRVHELOC,


        [System.Xml.Serialization.XmlEnumAttribute("7YRFHELOAN")]
        Item7YRFHELOAN,


        [System.Xml.Serialization.XmlEnumAttribute("7YRVHELOAN")]
        Item7YRVHELOAN,


        [System.Xml.Serialization.XmlEnumAttribute("7YRFHELOC")]
        Item7YRFHELOC,


        [System.Xml.Serialization.XmlEnumAttribute("7YRVHELOC")]
        Item7YRVHELOC,


        [System.Xml.Serialization.XmlEnumAttribute("10YRFHELOAN")]
        Item10YRFHELOAN,


        [System.Xml.Serialization.XmlEnumAttribute("10YRVHELOAN")]
        Item10YRVHELOAN,


        [System.Xml.Serialization.XmlEnumAttribute("10YRFHELOC")]
        Item10YRFHELOC,


        [System.Xml.Serialization.XmlEnumAttribute("10YRVHELOC")]
        Item10YRVHELOC,


        [System.Xml.Serialization.XmlEnumAttribute("15YRFHELOAN")]
        Item15YRFHELOAN,


        [System.Xml.Serialization.XmlEnumAttribute("15YRVHELOAN")]
        Item15YRVHELOAN,


        [System.Xml.Serialization.XmlEnumAttribute("15YRFHELOC")]
        Item15YRFHELOC,


        [System.Xml.Serialization.XmlEnumAttribute("15YRVHELOC")]
        Item15YRVHELOC,


        [System.Xml.Serialization.XmlEnumAttribute("20YRFHELOAN")]
        Item20YRFHELOAN,


        [System.Xml.Serialization.XmlEnumAttribute("20YRVHELOAN")]
        Item20YRVHELOAN,


        [System.Xml.Serialization.XmlEnumAttribute("20YRFHELOC")]
        Item20YRFHELOC,


        [System.Xml.Serialization.XmlEnumAttribute("20YRVHELOC")]
        Item20YRVHELOC,


        [System.Xml.Serialization.XmlEnumAttribute("25YRFHELOAN")]
        Item25YRFHELOAN,


        [System.Xml.Serialization.XmlEnumAttribute("25YRVHELOAN")]
        Item25YRVHELOAN,


        [System.Xml.Serialization.XmlEnumAttribute("25YRFHELOC")]
        Item25YRFHELOC,


        [System.Xml.Serialization.XmlEnumAttribute("25YRVHELOC")]
        Item25YRVHELOC,


        [System.Xml.Serialization.XmlEnumAttribute("30YRFHELOAN")]
        Item30YRFHELOAN,


        [System.Xml.Serialization.XmlEnumAttribute("30YRVHELOAN")]
        Item30YRVHELOAN,


        [System.Xml.Serialization.XmlEnumAttribute("30YRFHELOC")]
        Item30YRFHELOC,


        [System.Xml.Serialization.XmlEnumAttribute("30YRVHELOC")]
        Item30YRVHELOC,


        OTHERINNOVATIVEHEPRODUCTS,


        MORTGAGEOTHER,


        [System.Xml.Serialization.XmlEnumAttribute("10YRF")]
        Item10YRF,


        [System.Xml.Serialization.XmlEnumAttribute("15YRF")]
        Item15YRF,


        [System.Xml.Serialization.XmlEnumAttribute("20YRF")]
        Item20YRF,


        [System.Xml.Serialization.XmlEnumAttribute("25YRF")]
        Item25YRF,


        [System.Xml.Serialization.XmlEnumAttribute("30YRF")]
        Item30YRF,


        [System.Xml.Serialization.XmlEnumAttribute("40YRF")]
        Item40YRF,


        [System.Xml.Serialization.XmlEnumAttribute("5YRB")]
        Item5YRB,


        [System.Xml.Serialization.XmlEnumAttribute("7YRB")]
        Item7YRB,


        [System.Xml.Serialization.XmlEnumAttribute("10YRB")]
        Item10YRB,


        [System.Xml.Serialization.XmlEnumAttribute("6MONTHARM")]
        Item6MONTHARM,


        [System.Xml.Serialization.XmlEnumAttribute("1YRARM")]
        Item1YRARM,


        [System.Xml.Serialization.XmlEnumAttribute("2YRARM")]
        Item2YRARM,


        [System.Xml.Serialization.XmlEnumAttribute("3YRARM")]
        Item3YRARM,


        [System.Xml.Serialization.XmlEnumAttribute("5YRARM")]
        Item5YRARM,


        [System.Xml.Serialization.XmlEnumAttribute("7YRARM")]
        Item7YRARM,


        [System.Xml.Serialization.XmlEnumAttribute("10YRARM")]
        Item10YRARM,


        [System.Xml.Serialization.XmlEnumAttribute("5YRARM-INTONLY")]
        Item5YRARMINTONLY,


        [System.Xml.Serialization.XmlEnumAttribute("10YRARM-INTONLY")]
        Item10YRARMINTONLY,


        [System.Xml.Serialization.XmlEnumAttribute("15YRARM-INTONLY")]
        Item15YRARMINTONLY,


        [System.Xml.Serialization.XmlEnumAttribute("20YRARM-INTONLY")]
        Item20YRARMINTONLY,


        [System.Xml.Serialization.XmlEnumAttribute("25YRARM-INTONLY")]
        Item25YRARMINTONLY,


        [System.Xml.Serialization.XmlEnumAttribute("30YRARM-INTONLY")]
        Item30YRARMINTONLY,
    }


    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
    [System.SerializableAttribute()]
    public enum HomeEquityPurposesType
    {


        PAYOFFHELOAN,


        PAYOFFHELOC,


        HOMEIMP,


        DEBTCONSOLIDATION,


        AUTOPURCHASE,


        BOATPURCHASE,


        RVPURCHASE,


        MOTORCYCLEPURCHASE,


        OTHER,
    }


    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class RefinanceSubjectPropertyType : object, System.ComponentModel.INotifyPropertyChanged
    {

        private PropertyType propertyTypeField;

        private PropertyUseType propertyUseField;

        private string propertyAddressField;

        private StateType propertyStateField;

        private bool propertyStateFieldSpecified;

        private string propertyCountyField;

        private string propertyCityField;

        private string propertyZipField;

        private UnitsType unitsField;

        private bool unitsFieldSpecified;

        public RefinanceSubjectPropertyType()
        {
            this.propertyTypeField = PropertyType.SINGLEFAMDET;
            this.propertyUseField = PropertyUseType.OWNEROCCUPIED;
        }


        public PropertyType PropertyType
        {
            get
            {
                return this.propertyTypeField;
            }
            set
            {
                this.propertyTypeField = value;
                this.RaisePropertyChanged("PropertyType");
            }
        }


        public PropertyUseType PropertyUse
        {
            get
            {
                return this.propertyUseField;
            }
            set
            {
                this.propertyUseField = value;
                this.RaisePropertyChanged("PropertyUse");
            }
        }


        public string PropertyAddress
        {
            get
            {
                return this.propertyAddressField;
            }
            set
            {
                this.propertyAddressField = value;
                this.RaisePropertyChanged("PropertyAddress");
            }
        }


        public StateType PropertyState
        {
            get
            {
                return this.propertyStateField;
            }
            set
            {
                this.propertyStateField = value;
                this.RaisePropertyChanged("PropertyState");
            }
        }


        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool PropertyStateSpecified
        {
            get
            {
                return this.propertyStateFieldSpecified;
            }
            set
            {
                this.propertyStateFieldSpecified = value;
                this.RaisePropertyChanged("PropertyStateSpecified");
            }
        }


        public string PropertyCounty
        {
            get
            {
                return this.propertyCountyField;
            }
            set
            {
                this.propertyCountyField = value;
                this.RaisePropertyChanged("PropertyCounty");
            }
        }


        public string PropertyCity
        {
            get
            {
                return this.propertyCityField;
            }
            set
            {
                this.propertyCityField = value;
                this.RaisePropertyChanged("PropertyCity");
            }
        }


        public string PropertyZip
        {
            get
            {
                return this.propertyZipField;
            }
            set
            {
                this.propertyZipField = value;
                this.RaisePropertyChanged("PropertyZip");
            }
        }


        public UnitsType Units
        {
            get
            {
                return this.unitsField;
            }
            set
            {
                this.unitsField = value;
                this.RaisePropertyChanged("Units");
            }
        }


        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool UnitsSpecified
        {
            get
            {
                return this.unitsFieldSpecified;
            }
            set
            {
                this.unitsFieldSpecified = value;
                this.RaisePropertyChanged("UnitsSpecified");
            }
        }

        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

        protected void RaisePropertyChanged(string propertyName)
        {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null))
            {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }


    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class RefinanceType : object, System.ComponentModel.INotifyPropertyChanged
    {

        private decimal requestedLoanAmountField;

        private bool requestedLoanAmountFieldSpecified;

        private decimal propertyEstimatedValueField;

        private decimal monthlyPaymentField;

        private decimal estimatedMortgageBalanceField;

        private decimal firstMortgageInterestRateField;

        private bool firstMortgageInterestRateFieldSpecified;

        private YesNoType haveMultipleMortagesField;

        private decimal secondMortgageBalanceField;

        private bool secondMortgageBalanceFieldSpecified;

        private decimal secondMortgageMonthlyPaymentField;

        private bool secondMortgageMonthlyPaymentFieldSpecified;

        private decimal secondMortgageInterestRateField;

        private bool secondMortgageInterestRateFieldSpecified;

        private ProductType[] requestedProductsField;

        private RefinancePurposesType[] refinancePurposeField;

        private decimal cashOutField;

        private RefinanceSubjectPropertyType subjectPropertyField;

        public RefinanceType()
        {
            this.haveMultipleMortagesField = YesNoType.N;
            this.cashOutField = ((decimal)(0m));
        }


        public decimal RequestedLoanAmount
        {
            get
            {
                return this.requestedLoanAmountField;
            }
            set
            {
                this.requestedLoanAmountField = value;
                this.RaisePropertyChanged("RequestedLoanAmount");
            }
        }


        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool RequestedLoanAmountSpecified
        {
            get
            {
                return this.requestedLoanAmountFieldSpecified;
            }
            set
            {
                this.requestedLoanAmountFieldSpecified = value;
                this.RaisePropertyChanged("RequestedLoanAmountSpecified");
            }
        }


        public decimal PropertyEstimatedValue
        {
            get
            {
                return this.propertyEstimatedValueField;
            }
            set
            {
                this.propertyEstimatedValueField = value;
                this.RaisePropertyChanged("PropertyEstimatedValue");
            }
        }


        public decimal MonthlyPayment
        {
            get
            {
                return this.monthlyPaymentField;
            }
            set
            {
                this.monthlyPaymentField = value;
                this.RaisePropertyChanged("MonthlyPayment");
            }
        }


        public decimal EstimatedMortgageBalance
        {
            get
            {
                return this.estimatedMortgageBalanceField;
            }
            set
            {
                this.estimatedMortgageBalanceField = value;
                this.RaisePropertyChanged("EstimatedMortgageBalance");
            }
        }


        public decimal FirstMortgageInterestRate
        {
            get
            {
                return this.firstMortgageInterestRateField;
            }
            set
            {
                this.firstMortgageInterestRateField = value;
                this.RaisePropertyChanged("FirstMortgageInterestRate");
            }
        }


        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool FirstMortgageInterestRateSpecified
        {
            get
            {
                return this.firstMortgageInterestRateFieldSpecified;
            }
            set
            {
                this.firstMortgageInterestRateFieldSpecified = value;
                this.RaisePropertyChanged("FirstMortgageInterestRateSpecified");
            }
        }


        public YesNoType HaveMultipleMortages
        {
            get
            {
                return this.haveMultipleMortagesField;
            }
            set
            {
                this.haveMultipleMortagesField = value;
                this.RaisePropertyChanged("HaveMultipleMortages");
            }
        }


        public decimal SecondMortgageBalance
        {
            get
            {
                return this.secondMortgageBalanceField;
            }
            set
            {
                this.secondMortgageBalanceField = value;
                this.RaisePropertyChanged("SecondMortgageBalance");
            }
        }


        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool SecondMortgageBalanceSpecified
        {
            get
            {
                return this.secondMortgageBalanceFieldSpecified;
            }
            set
            {
                this.secondMortgageBalanceFieldSpecified = value;
                this.RaisePropertyChanged("SecondMortgageBalanceSpecified");
            }
        }


        public decimal SecondMortgageMonthlyPayment
        {
            get
            {
                return this.secondMortgageMonthlyPaymentField;
            }
            set
            {
                this.secondMortgageMonthlyPaymentField = value;
                this.RaisePropertyChanged("SecondMortgageMonthlyPayment");
            }
        }


        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool SecondMortgageMonthlyPaymentSpecified
        {
            get
            {
                return this.secondMortgageMonthlyPaymentFieldSpecified;
            }
            set
            {
                this.secondMortgageMonthlyPaymentFieldSpecified = value;
                this.RaisePropertyChanged("SecondMortgageMonthlyPaymentSpecified");
            }
        }


        public decimal SecondMortgageInterestRate
        {
            get
            {
                return this.secondMortgageInterestRateField;
            }
            set
            {
                this.secondMortgageInterestRateField = value;
                this.RaisePropertyChanged("SecondMortgageInterestRate");
            }
        }


        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool SecondMortgageInterestRateSpecified
        {
            get
            {
                return this.secondMortgageInterestRateFieldSpecified;
            }
            set
            {
                this.secondMortgageInterestRateFieldSpecified = value;
                this.RaisePropertyChanged("SecondMortgageInterestRateSpecified");
            }
        }


        [System.Xml.Serialization.XmlArrayItemAttribute("Product", IsNullable = false)]
        public ProductType[] RequestedProducts
        {
            get
            {
                return this.requestedProductsField;
            }
            set
            {
                this.requestedProductsField = value;
                this.RaisePropertyChanged("RequestedProducts");
            }
        }


        [System.Xml.Serialization.XmlArrayItemAttribute("Purpose", IsNullable = false)]
        public RefinancePurposesType[] RefinancePurpose
        {
            get
            {
                return this.refinancePurposeField;
            }
            set
            {
                this.refinancePurposeField = value;
                this.RaisePropertyChanged("RefinancePurpose");
            }
        }


        public decimal CashOut
        {
            get
            {
                return this.cashOutField;
            }
            set
            {
                this.cashOutField = value;
                this.RaisePropertyChanged("CashOut");
            }
        }


        public RefinanceSubjectPropertyType SubjectProperty
        {
            get
            {
                if (this.subjectPropertyField == null) this.subjectPropertyField = new RefinanceSubjectPropertyType();
                return this.subjectPropertyField;
            }
            set
            {
                this.subjectPropertyField = value;
                this.RaisePropertyChanged("SubjectProperty");
            }
        }

        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

        protected void RaisePropertyChanged(string propertyName)
        {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null))
            {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }


    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
    [System.SerializableAttribute()]
    public enum RefinancePurposesType
    {


        REFIPRIMARY,


        PAYOFFSECOND,


        PAYOFFHELOC,


        CASHOUT,


        PAYOFFSPOUSE,


        HOMEIMP,
    }


    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class PurchaseSubjectPropertyType : object, System.ComponentModel.INotifyPropertyChanged
    {

        private PropertyType propertyTypeField;

        private PropertyUseType propertyUseField;

        private string propertyAddressField;

        private StateType propertyStateField;

        private string propertyCountyField;

        private string propertyCityField;

        private string propertyZipField;

        private UnitsType unitsField;

        private bool unitsFieldSpecified;

        public PurchaseSubjectPropertyType()
        {
            this.propertyTypeField = PropertyType.SINGLEFAMDET;
            this.propertyUseField = PropertyUseType.OWNEROCCUPIED;
        }


        public PropertyType PropertyType
        {
            get
            {
                return this.propertyTypeField;
            }
            set
            {
                this.propertyTypeField = value;
                this.RaisePropertyChanged("PropertyType");
            }
        }


        public PropertyUseType PropertyUse
        {
            get
            {
                return this.propertyUseField;
            }
            set
            {
                this.propertyUseField = value;
                this.RaisePropertyChanged("PropertyUse");
            }
        }


        public string PropertyAddress
        {
            get
            {
                return this.propertyAddressField;
            }
            set
            {
                this.propertyAddressField = value;
                this.RaisePropertyChanged("PropertyAddress");
            }
        }


        public StateType PropertyState
        {
            get
            {
                return this.propertyStateField;
            }
            set
            {
                this.propertyStateField = value;
                this.RaisePropertyChanged("PropertyState");
            }
        }


        public string PropertyCounty
        {
            get
            {
                return this.propertyCountyField;
            }
            set
            {
                this.propertyCountyField = value;
                this.RaisePropertyChanged("PropertyCounty");
            }
        }


        public string PropertyCity
        {
            get
            {
                return this.propertyCityField;
            }
            set
            {
                this.propertyCityField = value;
                this.RaisePropertyChanged("PropertyCity");
            }
        }


        public string PropertyZip
        {
            get
            {
                return this.propertyZipField;
            }
            set
            {
                this.propertyZipField = value;
                this.RaisePropertyChanged("PropertyZip");
            }
        }


        public UnitsType Units
        {
            get
            {
                return this.unitsField;
            }
            set
            {
                this.unitsField = value;
                this.RaisePropertyChanged("Units");
            }
        }


        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool UnitsSpecified
        {
            get
            {
                return this.unitsFieldSpecified;
            }
            set
            {
                this.unitsFieldSpecified = value;
                this.RaisePropertyChanged("UnitsSpecified");
            }
        }

        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

        protected void RaisePropertyChanged(string propertyName)
        {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null))
            {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }


    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class PurchaseType : object, System.ComponentModel.INotifyPropertyChanged
    {

        private decimal requestedLoanAmountField;

        private bool requestedLoanAmountFieldSpecified;

        private decimal propertyPurchasePriceField;

        private YesNoType firstTimeHomeBuyerField;

        private bool firstTimeHomeBuyerFieldSpecified;

        private YesNoType foundAHomeField;

        private YesNoType signedSalesContractField;

        private bool signedSalesContractFieldSpecified;

        private YesNoType needRealtorField;

        private decimal downPaymentField;

        private ProductType[] requestedProductsField;

        private PurchaseSubjectPropertyType subjectPropertyField;

        public PurchaseType()
        {
            this.needRealtorField = YesNoType.Y;
        }


        public decimal RequestedLoanAmount
        {
            get
            {
                return this.requestedLoanAmountField;
            }
            set
            {
                this.requestedLoanAmountField = value;
                this.RaisePropertyChanged("RequestedLoanAmount");
            }
        }


        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool RequestedLoanAmountSpecified
        {
            get
            {
                return this.requestedLoanAmountFieldSpecified;
            }
            set
            {
                this.requestedLoanAmountFieldSpecified = value;
                this.RaisePropertyChanged("RequestedLoanAmountSpecified");
            }
        }


        public decimal PropertyPurchasePrice
        {
            get
            {
                return this.propertyPurchasePriceField;
            }
            set
            {
                this.propertyPurchasePriceField = value;
                this.RaisePropertyChanged("PropertyPurchasePrice");
            }
        }


        public YesNoType FirstTimeHomeBuyer
        {
            get
            {
                return this.firstTimeHomeBuyerField;
            }
            set
            {
                this.firstTimeHomeBuyerField = value;
                this.RaisePropertyChanged("FirstTimeHomeBuyer");
            }
        }


        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool FirstTimeHomeBuyerSpecified
        {
            get
            {
                return this.firstTimeHomeBuyerFieldSpecified;
            }
            set
            {
                this.firstTimeHomeBuyerFieldSpecified = value;
                this.RaisePropertyChanged("FirstTimeHomeBuyerSpecified");
            }
        }


        public YesNoType FoundAHome
        {
            get
            {
                return this.foundAHomeField;
            }
            set
            {
                this.foundAHomeField = value;
                this.RaisePropertyChanged("FoundAHome");
            }
        }


        public YesNoType SignedSalesContract
        {
            get
            {
                return this.signedSalesContractField;
            }
            set
            {
                this.signedSalesContractField = value;
                this.RaisePropertyChanged("SignedSalesContract");
            }
        }


        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool SignedSalesContractSpecified
        {
            get
            {
                return this.signedSalesContractFieldSpecified;
            }
            set
            {
                this.signedSalesContractFieldSpecified = value;
                this.RaisePropertyChanged("SignedSalesContractSpecified");
            }
        }


        public YesNoType NeedRealtor
        {
            get
            {
                return this.needRealtorField;
            }
            set
            {
                this.needRealtorField = value;
                this.RaisePropertyChanged("NeedRealtor");
            }
        }


        public decimal DownPayment
        {
            get
            {
                return this.downPaymentField;
            }
            set
            {
                this.downPaymentField = value;
                this.RaisePropertyChanged("DownPayment");
            }
        }


        [System.Xml.Serialization.XmlArrayItemAttribute("Product", IsNullable = false)]
        public ProductType[] RequestedProducts
        {
            get
            {
                return this.requestedProductsField;
            }
            set
            {
                this.requestedProductsField = value;
                this.RaisePropertyChanged("RequestedProducts");
            }
        }


        public PurchaseSubjectPropertyType SubjectProperty
        {
            get
            {
                if (this.subjectPropertyField == null) this.subjectPropertyField = new PurchaseSubjectPropertyType();
                return this.subjectPropertyField;
            }
            set
            {
                this.subjectPropertyField = value;
                this.RaisePropertyChanged("SubjectProperty");
            }
        }

        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

        protected void RaisePropertyChanged(string propertyName)
        {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null))
            {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }


    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class HomeLoanProductType : object, System.ComponentModel.INotifyPropertyChanged
    {

        private object itemField;


        [System.Xml.Serialization.XmlElementAttribute("HomeEquity", typeof(HomeEquityType))]
        [System.Xml.Serialization.XmlElementAttribute("Purchase", typeof(PurchaseType))]
        [System.Xml.Serialization.XmlElementAttribute("Refinance", typeof(RefinanceType))]
        public object Item
        {
            get
            {
                return this.itemField;
            }
            set
            {
                this.itemField = value;
                this.RaisePropertyChanged("Item");
            }
        }

        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

        protected void RaisePropertyChanged(string propertyName)
        {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null))
            {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
