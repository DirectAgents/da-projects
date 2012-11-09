namespace LTWeb.Service
{
    public interface ILendingTreeModel
    {
        void Initialize();
        bool RequiresInitialization();
        string DataPropertyName { get; }
        string GetXMLForPost();
        string AppID { get; set; }
        string CreditRating { get; set; }
        string LoanType { get; set; }
        string PropertyState { get; set; }
        string PropertyType { get; set; }
        string PropertyUse { get; set; }
        string PropertyZip { get; set; }
        decimal PropertyApproximateValue { get; set; }
        decimal EstimatedMortgageBalance { get; set; }
        decimal CashOut { get; set; }
        decimal MonthlyPayment { get; set; }
        string BankruptcyDischarged { get; set; }
        string ForeclosureDischarged { get; set; }
        string DOB { get; set; }
        string Email { get; set; }
        string FirstName { get; set; }
        string HomePhone { get; set; }
        string LastName { get; set; }
        string SSN { get; set; }
        string Address { get; set; }
        string WorkPhone { get; set; }
        string ApplicantZipCode { get; set; }
        string AffiliateSiteID { get; set; }
        bool IsVetran { get; set; }
        decimal PurchasePrice { get; set; }
        decimal DownPayment { get; set; }
        string PropertyCity { get; set; }
        object this[string propertyName] { get; }
        bool RequiresDisclosure { get; }
        string ESourceId { get; set; }
    }
}
