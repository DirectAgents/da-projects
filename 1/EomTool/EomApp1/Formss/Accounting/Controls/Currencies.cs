using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace EomApp1.Formss.Accounting.Controls
{
    public partial class Currencies : UserControl
    {
        public Currencies()
        {
            InitializeComponent();
        }

        private void currencyBindingNavigatorSaveItem_Click(object sender, EventArgs e)
        {
            this.Validate();
            this.currencyBindingSource.EndEdit();
            this.tableAdapterManager.UpdateAll(this.accountingDataSet);
        }

        public void Init()
        {
            tableAdapterManager.CurrencyTableAdapter.Fill(accountingDataSet.Currency);
        }
    }
}
