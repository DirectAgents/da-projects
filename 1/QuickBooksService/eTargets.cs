using System;

namespace QuickBooksService
{
    [Flags]
    public enum eTargets
    {
        None = 0,
        Customer = 1,
        Invoice = 2,
        Payment = 4,
        CreditMemo = 8,
        All = 16
    }
}
