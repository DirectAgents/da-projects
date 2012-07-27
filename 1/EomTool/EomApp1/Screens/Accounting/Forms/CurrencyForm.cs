using System;
using System.Windows.Forms;

namespace EomApp1.Screens.Accounting.Forms
{
    public partial class CurrencyForm : Form
    {
        public CurrencyForm()
        {
            InitializeComponent();
        }

        private void CurrencyForm_Load(object sender, EventArgs e)
        {
            currencies1.Init();
        }
    }
}
