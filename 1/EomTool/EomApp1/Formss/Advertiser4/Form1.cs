using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace EomApp1.Formss.Advertiser4
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void advertiserBindingNavigatorSaveItem_Click(object sender, EventArgs e)
        {
            this.Validate();
            this.advertiserBindingSource.EndEdit();
            this.tableAdapterManager.UpdateAll(this.dataSet1);

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.receivePaymentTableAdapter.Fill(this.dataSet1.ReceivePayment);
            this.customerTableAdapter.Fill(this.dataSet1.Customer);
            this.currencyTableAdapter.Fill(this.dataSet1.Currency);
            this.advertiserTableAdapter.Fill(this.dataSet1.Advertiser);

        }

        private void advertiserBindingSource_PositionChanged(object sender, EventArgs e)
        {
            var drv = ((DataRowView)advertiserBindingSource.Current).Row;
            var r = (DataSet1.AdvertiserRow)drv;
            var customerId = r.customer_id;
            MessageBox.Show(customerId.ToString());
            invoiceTableAdapter.FillByCustomerId(dataSet1.Invoice, customerId);
            receivePaymentTableAdapter.FillByCustomerId(dataSet1.ReceivePayment, customerId);
        }
    }
}
