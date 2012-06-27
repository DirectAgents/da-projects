using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using EomApp1.Formss.AB.Views;
using EomApp1.Formss.AB.Presenters;

namespace EomApp1.Formss.AB.Forms
{
    public partial class AB10 : Form, IABView
    {
        private ABPresenter _presenter;
        private AdvancedDataGridView.TreeGridView treeGridView1;
        private AdvancedDataGridView.TreeGridColumn treeColumn;
        private DataGridViewTextBoxColumn periodDataGridViewTextBoxColumn;
        private DataGridViewTextBoxColumn descriptionDataGridViewTextBoxColumn;
        private DataGridViewTextBoxColumn quantityDataGridViewTextBoxColumn;
        private DataGridViewTextBoxColumn amountDataGridViewTextBoxColumn;
        private DataGridViewTextBoxColumn totalDataGridViewTextBoxColumn;
        private DataGridViewTextBoxColumn currencyDataGridViewTextBoxColumn;
        public AB10()
        {
            InitializeComponent();
        }

        private void AB_Load(object sender, EventArgs e)
        {
            _presenter = new ABPresenter();
            _presenter.Init(this);
            _advertisersTree.ExpandAll();
            InitializeTreeGrid();
        }

        private void InitializeTreeGrid()
        {
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();

            // The TreeGridView control
            this.treeGridView1 = new AdvancedDataGridView.TreeGridView();
            // The Tree part
            this.treeColumn = new AdvancedDataGridView.TreeGridColumn();
            // The TextBox columns
            this.periodDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.descriptionDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.quantityDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.amountDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.totalDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.currencyDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            // 
            // periodDataGridViewTextBoxColumn
            // 
            this.periodDataGridViewTextBoxColumn.DataPropertyName = "Period";
            this.periodDataGridViewTextBoxColumn.HeaderText = "Period";
            this.periodDataGridViewTextBoxColumn.Name = "periodDataGridViewTextBoxColumn";
            this.periodDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // descriptionDataGridViewTextBoxColumn
            // 
            this.descriptionDataGridViewTextBoxColumn.DataPropertyName = "Description";
            this.descriptionDataGridViewTextBoxColumn.HeaderText = "Description";
            this.descriptionDataGridViewTextBoxColumn.Name = "descriptionDataGridViewTextBoxColumn";
            this.descriptionDataGridViewTextBoxColumn.ReadOnly = true;
            this.descriptionDataGridViewTextBoxColumn.Width = 300;
            // 
            // quantityDataGridViewTextBoxColumn
            // 
            this.quantityDataGridViewTextBoxColumn.DataPropertyName = "Quantity";
            this.quantityDataGridViewTextBoxColumn.HeaderText = "Quantity";
            this.quantityDataGridViewTextBoxColumn.Name = "quantityDataGridViewTextBoxColumn";
            this.quantityDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // amountDataGridViewTextBoxColumn
            // 
            this.amountDataGridViewTextBoxColumn.DataPropertyName = "Amount";
            this.amountDataGridViewTextBoxColumn.HeaderText = "Amount";
            this.amountDataGridViewTextBoxColumn.Name = "amountDataGridViewTextBoxColumn";
            this.amountDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // totalDataGridViewTextBoxColumn
            // 
            this.totalDataGridViewTextBoxColumn.DataPropertyName = "Total";
            this.totalDataGridViewTextBoxColumn.HeaderText = "Total";
            this.totalDataGridViewTextBoxColumn.Name = "totalDataGridViewTextBoxColumn";
            this.totalDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // currencyDataGridViewTextBoxColumn
            // 
            this.currencyDataGridViewTextBoxColumn.DataPropertyName = "Currency";
            this.currencyDataGridViewTextBoxColumn.HeaderText = "Currency";
            this.currencyDataGridViewTextBoxColumn.Name = "currencyDataGridViewTextBoxColumn";
            this.currencyDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // splitContainer2.Panel1
            // 
            this.treeGridView1.AllowUserToAddRows = false;
            this.treeGridView1.AllowUserToDeleteRows = false;
            this.treeGridView1.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.SingleVertical;
            this.treeGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] 
            {
                this.treeColumn,
                this.periodDataGridViewTextBoxColumn,
                this.descriptionDataGridViewTextBoxColumn,
                this.quantityDataGridViewTextBoxColumn,
                this.amountDataGridViewTextBoxColumn,
                this.totalDataGridViewTextBoxColumn,
                this.currencyDataGridViewTextBoxColumn
            });
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.treeGridView1.DefaultCellStyle = dataGridViewCellStyle3;
            this.treeGridView1.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.treeGridView1.ImageList = null;
            this.treeGridView1.Location = new System.Drawing.Point(12, 12);
            this.treeGridView1.Name = "treeGridView1";
            this.treeGridView1.RowHeadersVisible = false;
            this.treeGridView1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.treeGridView1.ShowLines = false;
            this.treeGridView1.Size = new System.Drawing.Size(530, 442);
            this.treeGridView1.TabIndex = 3;
            
            //treeGridView1.Dock = DockStyle.Fill;
            splitContainer1.Panel1.Controls.Add(treeGridView1);
        }

        #region IABView Members

        List<Data.Advertiser> IABView.Advertisers
        {
            set
            {
                TreeNode root = _advertisersTree.TopNode;
                foreach (var item in value)
                {
                    root.Nodes.Add(item.Name);
                }
            }
        }

        List<Data.ABItem> IABView.ABItems
        {
            set
            {
                //aBItemBindingSource.Clear();
                //value.ForEach(c => aBItemBindingSource.Add(c));
                var periodMap = new Dictionary<string, bool>();
                foreach (var abItem in value)
                {
                    treeGridView1.Nodes.Add(null, abItem.Period, abItem.Description, abItem.Quantity, abItem.Amount, abItem.Total, abItem.Currency);
                    
                    //string period = abItem.Period;
                    //if (period == "-")
                    //{

                    //}
                    //else if (!periodMap.ContainsKey(period))
                    //{

                    //}
                    //else // child node
                    //{

                    //}
                }
            }
        }

        bool IABView.ConvertToTargetCurrency
        {
            get
            {
                return _convertCurrencyMenuItem.Checked;
            }
        }

        decimal IABView.Total
        {
            set
            {
                label1.Text = value.ToString("N2");
            }
        }

        #endregion

        private void _advertisersTree_AfterSelect(object sender, TreeViewEventArgs e)
        {
            HandleSelection();
        }

        private void HandleSelection()
        {
            label1.Text = "";
            if ((string)_advertisersTree.SelectedNode.Tag == "Root") return;
            string selectedAdvertiser = _advertisersTree.SelectedNode.Text;
            _presenter.SelectedAdvertiser = selectedAdvertiser;
        }

        private void _convertCurrencyMenuItem_CheckedChanged(object sender, EventArgs e)
        {
            HandleSelection();
        }

        private void editStartingBalancesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            (new AdvertiserStartingBalances()).ShowDialog();
        }
    }
}
