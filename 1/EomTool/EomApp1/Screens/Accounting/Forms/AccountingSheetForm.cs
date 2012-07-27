using System.Windows.Forms;

namespace EomApp1.Screens.Accounting.Forms
{
    public partial class AccountingSheetForm : Form
    {
        public AccountingSheetForm()
        {
            InitializeComponent();
            InitViews();
        }

        public void InitViews()
        {
            accountingSheet1.Fill();
        }
    }
}
