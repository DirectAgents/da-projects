using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using EomApp1.Presenters;
using EomApp1.Services;
using System.Data.Odbc;
using EomApp1.Formss.QB.Forms.Views;

namespace EomApp1.Formss.QB.Forms
{
    public partial class QB : Form, IQBView
    {
        private QBPresenter _presenter;
        public QB()
        {
            InitializeComponent();
        }

        private void QB_Load(object sender, EventArgs e)
        {
            treeView1.BackColor = Color.Gray;
            statusStrip1.BackColor = Color.Red;
            treeView1.Enabled = false;
            treeView1.CollapseAll();
            _presenter = new QBPresenter(this, new QBService());
            _presenter.Initialize();
        }

        public string Status
        {
            set { toolStripStatusLabel1.Text = value; }
        }

        public bool Connected
        {
            set 
            {
                if (!value)
                {
                    toolStripStatusLabel1.Text = "Not Connected to QB";
                }
                else
                {
                    treeView1.BackColor = Color.White;
                    statusStrip1.BackColor = Color.LimeGreen;
                    toolStripButton1.Enabled = false;
                    treeView1.ExpandAll();
                    treeView1.Enabled = true;
                }
            }
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            _presenter.Connect();
        }

        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (toolStripStatusLabel1.Text == "Not Connected to QB") return; // hack!

            TreeNode n = treeView1.SelectedNode;

            if ((string)n.Tag == "CustomerNodeTag")
            {
                dataGridView1.Enabled = false;
                dataGridView1.AutoGenerateColumns = true;
                bindingSource1.DataMember = "Customer";
                CustomerTable.Clear();
                InvoiceTable.Clear();
                ReceivePaymentTable.Clear();
                _presenter.LoadQBCustomerTable();
                dataGridView1.Enabled = true;
            }

            if ((string)n.Tag == "InvoiceNodeTag")
            {
                dataGridView1.Enabled = false;
                dataGridView1.AutoGenerateColumns = true;
                bindingSource1.DataMember = "Invoice";
                CustomerTable.Clear();
                InvoiceTable.Clear();
                ReceivePaymentTable.Clear();
                _presenter.LoadQBInvoiceTable();
                dataGridView1.Enabled = true;
            }

            if ((string)n.Tag == "ReceivePaymentTag")
            {
                dataGridView1.Enabled = false;
                dataGridView1.AutoGenerateColumns = true;
                bindingSource1.DataMember = "ReceivePayment";
                CustomerTable.Clear();
                InvoiceTable.Clear();
                ReceivePaymentTable.Clear();
                _presenter.LoadQBReceivePaymentTable();
                dataGridView1.Enabled = true;
            }
        }

        public DataTable QBCustomerTable
        {
            get
            {
                return CustomerTable;
            }
        }

        public DataTable QBInvoiceTable
        {
            get
            {
                return InvoiceTable;
            }
        }

        public DataTable QBReceivePaymentTable
        {
            get
            {
                return ReceivePaymentTable;
            }
        }

        private void dataGridView1_EnabledChanged(object sender, EventArgs e)
        {
            pictureBox1.Visible = !(sender as DataGridView).Enabled;
        }
    }
}
