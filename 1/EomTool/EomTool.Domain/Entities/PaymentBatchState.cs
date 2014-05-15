namespace EomTool.Domain.Entities
{
    public partial class PaymentBatchState
    {
        //TODO: change to const ?
        public static int Default = 1;
        public static int Sent = 3;
        public static int Complete = 6;
    }
}
