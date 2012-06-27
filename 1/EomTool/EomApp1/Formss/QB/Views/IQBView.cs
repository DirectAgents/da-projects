using System.Data;
namespace EomApp1.Formss.QB.Forms.Views
{
    public interface IQBView
    {
        bool Connected { set; }
        string Status { set; }
        DataTable QBCustomerTable { get; }
        DataTable QBInvoiceTable { get; }
        DataTable QBReceivePaymentTable { get; }
    }
}
