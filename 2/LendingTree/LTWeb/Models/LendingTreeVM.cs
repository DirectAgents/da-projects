﻿namespace LTWeb.Models
{
    public class LendingTreeVM
    {
        public string AppID { get; set; }
        public string CreditRating { get; set; }
        public string LoanType { get; set; }
        public string PropertyState { get; set; }
        public string PropertyType { get; set; }
        public string PropertyUse { get; set; }
        public string PropertyZip { get; set; }
        public decimal PropertyApproximateValue { get; set; }
        public decimal EstimatedMortgageBalance { get; set; }
        public decimal CashOut { get; set; }
        public decimal MonthlyPayment { get; set; }
        public string BankruptcyDischarged { get; set; }
        public string ForeclosureDischarged { get; set; }
        public string DOB { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string HomePhone { get; set; }
        public string LastName { get; set; }
        public string SSN { get; set; }
        public string Address { get; set; }
        public string WorkPhone { get; set; }
        public string ApplicantZipCode { get; set; }
        public string AffiliateSiteID { get; set; }
        public bool IsVetran { get; set; }
        public decimal PurchasePrice { get; set; }
        public decimal DownPayment { get; set; }
        public string PropertyCity { get; set; }
        public string ESourceId { get; set; }
    }
}