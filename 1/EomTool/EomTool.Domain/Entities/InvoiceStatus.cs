
namespace EomTool.Domain.Entities
{
    public partial class InvoiceStatus
    {
        public static int Default = 0;
        public static int AccountManagerReview = 1;
        public static int AccountingReview = 2;
        public static int Generated = 3;
    }
}
