using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Globalization;

namespace EomApp1.Formss.Advertiser3
{
    public partial class Advertisers3Form : Form
    {
        public Advertisers3Form()
        {
            InitializeComponent();
        }

        private void advertiserBindingNavigatorSaveItem_Click(object sender, EventArgs e)
        {
        }

        private void Advertisers3Form_Load(object sender, EventArgs e)
        {
            this.currencyTableAdapter.Fill(this.advertisers3DataSet.Currency);
            this.customerTableAdapter.Fill(this.advertisers3DataSet.Customer);
            this.advertiserTableAdapter.Fill(this.advertisers3DataSet.Advertiser);

        }

        private void customerBindingNavigatorSaveItem_Click(object sender, EventArgs e)
        {
        }

        private void advertiserBindingNavigatorSaveItem_Click_1(object sender, EventArgs e)
        {
            this.Validate();
            this.advertiserBindingSource.EndEdit();
            this.tableAdapterManager.UpdateAll(this.advertisers3DataSet);

        }

        string FormatCurrency(int currency, decimal amount)
        {
            var map = new Dictionary<int, string>
            {
                {1, "en-us"},
                {2, "en-gb"},
                {3, "de-de"},
                {5, "en-AU"}
            };
            return string.Format(CultureInfo.CreateSpecificCulture(map[currency]), "{0:C}", amount);
        }

        private void advertiserDataGridView_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            //int ci = e.ColumnIndex;
            //int ri = e.RowIndex;
            //if (ci == advertiserDataGridView.Columns.IndexOf(dataGridViewTextBoxColumn4))
            //{
            //    decimal? dd = advertiserDataGridView[ci, ri].Value as decimal?;
            //    //if() return;
            //    if (dd == null || dd.Value == 0.0m)
            //    {
            //        e.Value = "-";
            //    }
            //    else
            //    {
            //        int curr = (int)advertiserDataGridView["dataGridViewTextBoxColumn3", ri].Value;
            //        e.Value = FormatCurrency(curr, (decimal)advertiserDataGridView[ci, ri].Value);
            //    }
            //    e.FormattingApplied = true;
            //}
        }

        private void advertiserDataGridView_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            Console.WriteLine("here");
        }
    }
}
