namespace EomTool.Domain.Entities
{
    public partial class PaymentBatchState
    {
        public static int Default = 1;
        public static int Queued = 2;
        public static int Sent = 3;
    }
}
