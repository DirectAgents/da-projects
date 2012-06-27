using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace EomApp1.Formss.Accounting.Forms
{
    public partial class AccountingSheetForm : Form, EomApp1.Formss.Accounting.Forms.IFormContainer
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
