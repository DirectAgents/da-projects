
namespace EomTool.Domain.Entities
{
    public partial class ItemAccountingStatus
    {
        //TODO: change to const ?
        public static int Default = 1;
        public static int PaymentDue = 2;
        public static int DoNotPay = 3;
        public static int CheckCut = 4;
        public static int CheckSignedAndPaid = 5;
        public static int Approved = 6;
        public static int Hold = 7;
        public static int Verified = 8;
    }
}
