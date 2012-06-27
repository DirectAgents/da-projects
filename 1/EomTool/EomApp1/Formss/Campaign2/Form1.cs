using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace EomApp1.Formss.Campaign2
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // TODO: This line of code loads data into the 'dADatabaseApril2012_before_cakeDataSet.AccountManager' table. You can move, or remove it, as needed.
            this.accountManagerTableAdapter.Fill(this.dADatabaseApril2012_before_cakeDataSet.AccountManager);
            // TODO: This line of code loads data into the 'dADatabaseApril2012_before_cakeDataSet.Campaign' table. You can move, or remove it, as needed.
            this.campaignTableAdapter.Fill(this.dADatabaseApril2012_before_cakeDataSet.Campaign);

        }
    }
}
