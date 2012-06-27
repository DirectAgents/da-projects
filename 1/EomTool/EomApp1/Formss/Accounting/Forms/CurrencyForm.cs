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
