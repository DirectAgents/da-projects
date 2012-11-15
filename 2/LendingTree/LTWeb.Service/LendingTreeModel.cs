using System;
using System.ComponentModel;
using LTWeb.DataAccess;

namespace LTWeb.Service
{
    public class LendingTreeModel : INotifyPropertyChanged, ILendingTreeModel
    {
        LTRequest _request;
        LTRequest _response;

        public LendingTreeModel(string serviceConfigName)
        {
            using (var repo = new Repository(new LTWebDataContext(), false))
            {
                LendingTreeConfig = repo.Single<ServiceConfig>(c => c.Name == serviceConfigName);
            }
            this.AppID = Guid.NewGuid().ToString();
        }

        /// <summary>
        /// This is used by the client to determine if Initialize needs to be called.
        /// </summary>
        /// <returns></returns>
        bool ILendingTreeModel.RequiresInitialization()
        {
            return this.AppID == Guid.Empty.ToString();
        }

        public string DataPropertyName { get { return "Data"; } }

        public string GetXMLForPost()
        {
            string xml = Request.ToString();
            return xml;
        }

        public string GetUrlForPost()
        {
            return LendingTreeConfig.PostUrl;
        }

        public object this[string propertyName]
        {
            get
            {
                return GetType().GetProperty(propertyName).GetValue(this, null);
            }
        }

        public ServiceConfig LendingTreeConfig { get; set; }

        public string StatesExcludedFromDisclosure { get; set; }

        public LTRequest Request
        {
            get
            {
                if (_request == null)
                {
                    _request = new LTRequest();
                    _request.Request.SourceOfRequest = LendingTreeConfig.SourceOfRequest;
                    OnPropertyChanged(this, DataPropertyName);
                }
                return _request;
            }
            set
            {
                // Ensure property is on set one time.
                if (_request != null)
                {
                    throw new Exception("model already exists");
                }
                _request = value;
            }
        }

        public System.Xml.Linq.XElement ResponseXml
        {
            set
            {
                LTRequest.Create(value, out _response);
            }
        }

        public bool ResponseValidForPixelFire
        {
            get
            {
                return _request.Request.LoanType == ELoanType.REFINANCE;
            }
        }

        public string VisitorIPAddress
        {
            set
            {
                _request.Request.SourceOfRequest.VisitorIPAddress = value;
            }
        }

        public string VisitorURL
        {
            set
            {
                _request.Request.SourceOfRequest.VisitorURL = value;
            }
        }

        public bool RequiresDisclosure
        {
            get
            {
                return !StatesExcludedFromDisclosure.Contains(this.PropertyState);
            }
        }

        #region Events/Notification

        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// When a property changes, if that property is copied to the underlying LendingTreeAffiliateRequest
        /// (a.k.a. the "Data" property), then we send one notification for the Data property followed by the 
        /// another for the actual property.
        /// </summary>
        /// <param name="propertyName"></param>
        void OnDataChanged(string propertyName)
        {
            OnPropertyChanged(this, DataPropertyName);
            OnPropertyChanged(this, propertyName);
        }

        void OnPropertyChanged(object target, string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        #endregion

        #region Model Properties

        public string AppID
        {
            get
            {
                return Request.Request.AppID;
            }
            set
            {
                Request.Request.AppID = value;
                OnDataChanged("AppID");
            }
        }

        public string LoanType
        {
            get
            {
                return GetEnumName<ELoanType>(Request.Request.LoanType);
            }
            set
            {
                Request.Request.LoanType = ParseEnum<ELoanType>(value);
                OnDataChanged("LoanType");
            }
        }

        /// <summary>
        /// refi --> //Applicant/State
        /// purchase --> //Applicant/State AND //Purchase/SubjectProperty/PropertyState
        /// </summary>
        public string PropertyState
        {
            get
            {
                StateType stateType = this.IsLoanTypeRefinance ? Request.Request.TheApplicant.State : (GetHomeLoanProductItem() as PurchaseType).SubjectProperty.PropertyState;
                return GetEnumName<StateType>(stateType);
            }
            set
            {
                Request.Request.TheApplicant.State = ParseEnum<StateType>(value);
                if (Request.Request.LoanType == ELoanType.PURCHASE) (GetHomeLoanProductItem() as PurchaseType).SubjectProperty.PropertyState = ParseEnum<StateType>(value);
                OnDataChanged("PropertyState");
            }
        }

        public string CreditRating
        {
            get
            {
                return GetEnumName<CreditHistoryTypeCreditSelfRating>(Request.Request.TheApplicant.CreditHistory.CreditSelfRating);
            }
            set
            {
                Request.Request.TheApplicant.CreditHistory.CreditSelfRating = ParseEnum<CreditHistoryTypeCreditSelfRating>(value);
                OnDataChanged("CreditRating");
            }
        }

        public string BankruptcyDischarged
        {
            get
            {
                return GetEnumName<CreditHistoryTypeBankruptcyDischarged>(Request.Request.TheApplicant.CreditHistory.BankruptcyDischarged);
            }
            set
            {
                Request.Request.TheApplicant.CreditHistory.BankruptcyDischarged = ParseEnum<CreditHistoryTypeBankruptcyDischarged>(value);
                OnDataChanged("BankruptcyDischarged");
            }
        }

        public string ForeclosureDischarged
        {
            get
            {
                return GetEnumName<CreditHistoryTypeForeclosureDischarged>(Request.Request.TheApplicant.CreditHistory.ForeclosureDischarged);
            }
            set
            {
                Request.Request.TheApplicant.CreditHistory.ForeclosureDischarged = ParseEnum<CreditHistoryTypeForeclosureDischarged>(value);
                OnDataChanged("ForeclosureDischarged");
            }
        }

        public string PropertyType
        {
            get
            {
                return GetEnumName<PropertyType>(GetSubjectPropertyProperty<PropertyType>("PropertyType"));
            }
            set
            {
                SetSubjectPropertyProperty("PropertyType", ParseEnum<PropertyType>(value));
                OnDataChanged("PropertyType");
            }
        }

        public string PropertyUse
        {
            get
            {
                return GetEnumName<PropertyUseType>(GetSubjectPropertyProperty<PropertyUseType>("PropertyUse"));
            }
            set
            {
                SetSubjectPropertyProperty("PropertyUse", ParseEnum<PropertyUseType>(value));
                OnDataChanged("PropertyUse");
            }
        }

        public string PropertyZip
        {
            get
            {
                return GetSubjectPropertyProperty<string>("PropertyZip");
            }
            set
            {
                SetSubjectPropertyProperty("PropertyZip", value);
                OnDataChanged("PropertyZip");

                if (string.IsNullOrEmpty(ApplicantZipCode))
                {
                    ApplicantZipCode = value;
                }
            }
        }

        public decimal PropertyApproximateValue
        {
            get
            {
                AssertRefi();
                return (Request.Request.HomeLoanProduct.Item as RefinanceType).PropertyEstimatedValue;
            }
            set
            {
                AssertRefi();
                (Request.Request.HomeLoanProduct.Item as RefinanceType).PropertyEstimatedValue = value;
                OnDataChanged("PropertyApproximateValue");
            }
        }

        public decimal EstimatedMortgageBalance
        {
            get
            {
                AssertRefi();
                return (Request.Request.HomeLoanProduct.Item as RefinanceType).EstimatedMortgageBalance;
            }
            set
            {
                AssertRefi();
                (Request.Request.HomeLoanProduct.Item as RefinanceType).EstimatedMortgageBalance = value;
                OnDataChanged("EstimatedMortgageBalance");
            }
        }

        public decimal CashOut
        {
            get
            {
                AssertRefi();
                return (Request.Request.HomeLoanProduct.Item as RefinanceType).CashOut;
            }
            set
            {
                AssertRefi();
                (Request.Request.HomeLoanProduct.Item as RefinanceType).CashOut = value;
                OnDataChanged("CashOut");
            }
        }

        public decimal MonthlyPayment
        {
            get
            {
                AssertRefi();
                return (Request.Request.HomeLoanProduct.Item as RefinanceType).MonthlyPayment;
            }
            set
            {
                AssertRefi();
                (Request.Request.HomeLoanProduct.Item as RefinanceType).MonthlyPayment = value;
                OnDataChanged("MonthlyPayment");
            }
        }

        public decimal PurchasePrice
        {
            get
            {
                return GetPurchase().PropertyPurchasePrice;
            }
            set
            {
                GetPurchase().PropertyPurchasePrice = value;
                OnDataChanged("PurchasePrice");
            }
        }

        public decimal DownPayment
        {
            get
            {
                decimal propertyPurchasePrice = GetPurchase().PropertyPurchasePrice;
                return propertyPurchasePrice;
            }
            set
            {
                decimal propertyPurchasePrice = GetPurchase().PropertyPurchasePrice;
                decimal downPayment;
                switch (Convert.ToInt32(value))
                {
                    case 1:
                        downPayment = propertyPurchasePrice * (decimal)0.19;
                        break;
                    case 2:
                        downPayment = propertyPurchasePrice * (decimal)0.20;
                        break;
                    case 3:
                        downPayment = propertyPurchasePrice * (decimal)0.21;
                        break;
                    default:
                        throw new Exception("invalid down payment value");
                }
                GetPurchase().DownPayment = downPayment;
                OnDataChanged("DownPayment");
            }
        }

        public string PropertyCity
        {
            get
            {
                return GetPurchase().SubjectProperty.PropertyCity;
            }
            set
            {
                GetPurchase().SubjectProperty.PropertyCity = value;
                OnDataChanged("PropertyCity");
            }
        }

        public string DOB
        {
            get
            {
                return Request.Request.TheApplicant.DateOfBirth;
            }
            set
            {
                Request.Request.TheApplicant.DateOfBirth = value;
                OnDataChanged("DOB");
            }
        }

        public string Email
        {
            get
            {
                return Request.Request.TheApplicant.EmailAddress;
            }
            set
            {
                Request.Request.TheApplicant.EmailAddress = value;
                OnDataChanged("Email");
            }
        }

        public string FirstName
        {
            get
            {
                return Request.Request.TheApplicant.FirstName;
            }
            set
            {
                Request.Request.TheApplicant.FirstName = value;
                OnDataChanged("FirstName");
            }
        }

        public string HomePhone
        {
            get
            {
                return Request.Request.TheApplicant.HomePhone;
            }
            set
            {
                Request.Request.TheApplicant.HomePhone = FixPhoneNum(value);
                OnDataChanged("Email");
            }
        }

        public string LastName
        {
            get
            {
                return Request.Request.TheApplicant.LastName;
            }
            set
            {
                Request.Request.TheApplicant.LastName = value;
                OnDataChanged("LastName");
            }
        }

        public string SSN
        {
            get
            {
                return Request.Request.TheApplicant.SSN;
            }
            set
            {
                Request.Request.TheApplicant.SSN = value;
                OnDataChanged("SSN");
            }
        }

        public string Address
        {
            get
            {
                return Request.Request.TheApplicant.Street;
            }
            set
            {
                Request.Request.TheApplicant.Street = value;
                OnDataChanged("Address");
            }
        }

        public string WorkPhone
        {
            get
            {
                return Request.Request.TheApplicant.WorkPhone;
            }
            set
            {
                Request.Request.TheApplicant.WorkPhone = FixPhoneNum(value);
                OnDataChanged("WorkPhone");
            }
        }

        public string ApplicantZipCode
        {
            get
            {
                return Request.Request.TheApplicant.Zip;
            }
            set
            {
                Request.Request.TheApplicant.Zip = value;
                OnDataChanged("ApplicantZipCode");
            }
        }

        //public bool LendingTreeLoansOptIn
        //{
        //    get
        //    {
        //        return Data.Request.SourceOfRequest.LTLOptin == YesNoType.Y;
        //    }
        //    set
        //    {
        //        Data.Request.SourceOfRequest.LTLOptin = value ? YesNoType.Y : YesNoType.N;
        //        OnDataChanged("IsVetran");
        //    }
        //}

        public string AffiliateSiteID
        {
            get
            {
                return Request.Request.SourceOfRequest.AffiliateSiteID;
            }
            set
            {
                Request.Request.SourceOfRequest.AffiliateSiteID = value;
                OnDataChanged("AffiliateSiteID");
            }
        }

        public bool IsVetran
        {
            get
            {
                return Request.Request.TheApplicant.IsVeteran == YesNoType.Y;
            }
            set
            {
                Request.Request.TheApplicant.IsVeteran = value ? YesNoType.Y : YesNoType.N;
                OnDataChanged("IsVetran");
            }
        }

        public string ESourceId
        {
            get
            {
                return Request.Request.SourceOfRequest.LendingTreeAffiliateEsourceID;
            }
            set
            {
                Request.Request.SourceOfRequest.LendingTreeAffiliateEsourceID = value;
                OnDataChanged("ESourceId");
            }
        }

        #endregion

        #region Private Helpers

        bool IsLoanTypeRefinance
        {
            get
            {
                return Request.Request.LoanType == ELoanType.REFINANCE;
            }
        }

        string FixPhoneNum(string phoneNumber)
        {
            string result = phoneNumber
                .Replace("-", String.Empty)
                .Replace(" ", String.Empty)
                .Replace("(", String.Empty)
                .Replace(")", String.Empty);
            if (result.Length == 11 && result.StartsWith("1"))
            {
                result = result.Substring(1);
            }
            return result;
        }

        PurchaseType GetPurchase()
        {
            AssertPurchase();
            return (Request.Request.HomeLoanProduct.Item as PurchaseType);
        }

        void Assert(bool b, string errorMessage)
        {
            if (!b) throw new Exception(errorMessage);
        }

        void AssertRefi()
        {
            Assert(Request.Request.LoanType == ELoanType.REFINANCE, "invalid loan type");
        }

        void AssertPurchase()
        {
            Assert(Request.Request.LoanType == ELoanType.PURCHASE, "invalid loan type");
        }

        static string GetEnumName<T>(T v)
        {
            return Enum.GetName(typeof(T), v);
        }

        static T ParseEnum<T>(string v) where T : struct
        {
            T o;
            return Enum.TryParse<T>(v, out o) ? (T)o : (T)Enum.Parse(typeof(T), "Item" + v.Replace("-", String.Empty));
        }

        object GetHomeLoanProductItem()
        {
            object result = null;
            switch (Request.Request.LoanType)
            {
                case ELoanType.REFINANCE:
                    result = (RefinanceType)Request.Request.HomeLoanProduct.Item;
                    break;
                case ELoanType.PURCHASE:
                    result = (PurchaseType)Request.Request.HomeLoanProduct.Item;
                    break;
                default:
                    throw new Exception("invalid loan type");
            }
            return result;
        }

        void SetSubjectPropertyProperty(string s, object v)
        {
            object hlpi = GetHomeLoanProductItem();
            object sp = hlpi.GetType().GetProperty("SubjectProperty").GetValue(hlpi, null);
            sp.GetType().GetProperty(s).SetValue(sp, v, null);
        }

        T GetSubjectPropertyProperty<T>(string s)
        {
            object hlpi = GetHomeLoanProductItem();
            object sp = hlpi.GetType().GetProperty("SubjectProperty").GetValue(hlpi, null);
            return (T)sp.GetType().GetProperty(s).GetValue(sp, null);
        }

        #endregion

        public bool SsnRequired
        {
            get;
            set;
        }
    }
}
