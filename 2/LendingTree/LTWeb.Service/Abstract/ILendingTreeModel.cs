﻿namespace LTWeb.Service
{
    public interface ILendingTreeModel
    {
        string DataPropertyName { get; } // TODO: remove?
        string GetXMLForPost();
        string GetUrlForPost();
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
        bool IsCashOutSet { get; set; }
        decimal MonthlyPayment { get; set; }
        string BankruptcyDischarged { get; set; }
        string ForeclosureDischarged { get; set; }
        string DOB { get; set; }
        string Email { get; set; }
        string FirstName { get; set; }
        string HomePhone { get; set; }
        string LastName { get; set; }
        string SSN { get; set; }
        bool SsnRequired { get; set; }
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
        bool IsLoanTypeSet();
        string VisitorIPAddress { set; get; }
        string VisitorURL { set; }
        bool ResponseValidForPixelFire { get; }
    }
}
