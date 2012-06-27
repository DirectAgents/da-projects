using EomApp1.UI;
using System.Windows.Forms;

namespace EomApp1.Formss.Payment.Forms
{
    public partial class PaymentsForm : AppFormBase
    {
        public PaymentsForm()
        {
            InitializeComponent();
        }

        private void PaymentsForm_Load(object sender, System.EventArgs e)
        {
            advertiserListUC1.Fill();
        }
    }
}
