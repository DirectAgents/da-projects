using System;

namespace QuickBooks.UI.Models
{ 
    public class CompanyFile
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public class QuickBooksSynchSettings
    {
        public int CompanyFileId { get; set; }
    }

    public enum QuickBooksSynchTargets
    {
        Customer,
        Invoice,
        Payment,
        CreditMemo,
    }
}
