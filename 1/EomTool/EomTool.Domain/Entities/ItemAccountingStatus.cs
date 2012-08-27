using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EomTool.Domain.Entities
{
    public partial class ItemAccountingStatus
    {
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
