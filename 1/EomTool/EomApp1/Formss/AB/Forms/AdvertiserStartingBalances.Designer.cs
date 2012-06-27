namespace EomApp1.Formss.AB.Forms
{
    partial class AdvertiserStartingBalances
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AdvertiserStartingBalances));
            this.dAMain1DataSet = new EomApp1.Formss.AB.Data.DAMain1DataSet();
            this.advertiserStartingBalanceBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.advertiserStartingBalanceTableAdapter = new EomApp1.Formss.AB.Data.DAMain1DataSetTableAdapters.AdvertiserStartingBalanceTableAdapter();
            this.tableAdapterManager = new EomApp1.Formss.AB.Data.DAMain1DataSetTableAdapters.TableAdapterManager();
            this.advertiserTableAdapter1 = new EomApp1.Formss.AB.Data.DAMain1DataSetTableAdapters.AdvertiserTableAdapter();
            this.advertiserStartingBalanceBindingNavigator = new System.Windows.Forms.BindingNavigator(this.components);
            this.advertiserStartingBalanceBindingNavigatorSaveItem = new System.Windows.Forms.ToolStripButton();
            this.advertiserStartingBalanceDataGridView = new System.Windows.Forms.DataGridView();
            this.dataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Advertiser = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn5 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dAMain1DataSet)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.advertiserStartingBalanceBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.advertiserStartingBalanceBindingNavigator)).BeginInit();
            this.advertiserStartingBalanceBindingNavigator.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.advertiserStartingBalanceDataGridView)).BeginInit();
            this.SuspendLayout();
            // 
            // dAMain1DataSet
            // 
            this.dAMain1DataSet.DataSetName = "DAMain1DataSet";
            this.dAMain1DataSet.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // advertiserStartingBalanceBindingSource
            // 
            this.advertiserStartingBalanceBindingSource.DataMember = "AdvertiserStartingBalance";
            this.advertiserStartingBalanceBindingSource.DataSource = this.dAMain1DataSet;
            // 
            // advertiserStartingBalanceTableAdapter
            // 
            this.advertiserStartingBalanceTableAdapter.ClearBeforeFill = true;
            // 
            // tableAdapterManager
            // 
            this.tableAdapterManager.AdvertiserStartingBalanceTableAdapter = this.advertiserStartingBalanceTableAdapter;
            this.tableAdapterManager.AdvertiserTableAdapter = this.advertiserTableAdapter1;
            this.tableAdapterManager.BackupDataSetBeforeUpdate = false;
            this.tableAdapterManager.UpdateOrder = EomApp1.Formss.AB.Data.DAMain1DataSetTableAdapters.TableAdapterManager.UpdateOrderOption.InsertUpdateDelete;
            // 
            // advertiserTableAdapter1
            // 
            this.advertiserTableAdapter1.ClearBeforeFill = true;
            // 
            // advertiserStartingBalanceBindingNavigator
            // 
            this.advertiserStartingBalanceBindingNavigator.AddNewItem = null;
            this.advertiserStartingBalanceBindingNavigator.BindingSource = this.advertiserStartingBalanceBindingSource;
            this.advertiserStartingBalanceBindingNavigator.CountItem = null;
            this.advertiserStartingBalanceBindingNavigator.DeleteItem = null;
            this.advertiserStartingBalanceBindingNavigator.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.advertiserStartingBalanceBindingNavigatorSaveItem});
            this.advertiserStartingBalanceBindingNavigator.Location = new System.Drawing.Point(0, 0);
            this.advertiserStartingBalanceBindingNavigator.MoveFirstItem = null;
            this.advertiserStartingBalanceBindingNavigator.MoveLastItem = null;
            this.advertiserStartingBalanceBindingNavigator.MoveNextItem = null;
            this.advertiserStartingBalanceBindingNavigator.MovePreviousItem = null;
            this.advertiserStartingBalanceBindingNavigator.Name = "advertiserStartingBalanceBindingNavigator";
            this.advertiserStartingBalanceBindingNavigator.PositionItem = null;
            this.advertiserStartingBalanceBindingNavigator.Size = new System.Drawing.Size(525, 25);
            this.advertiserStartingBalanceBindingNavigator.TabIndex = 0;
            this.advertiserStartingBalanceBindingNavigator.Text = "bindingNavigator1";
            // 
            // advertiserStartingBalanceBindingNavigatorSaveItem
            // 
            this.advertiserStartingBalanceBindingNavigatorSaveItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.advertiserStartingBalanceBindingNavigatorSaveItem.Image = ((System.Drawing.Image)(resources.GetObject("advertiserStartingBalanceBindingNavigatorSaveItem.Image")));
            this.advertiserStartingBalanceBindingNavigatorSaveItem.Name = "advertiserStartingBalanceBindingNavigatorSaveItem";
            this.advertiserStartingBalanceBindingNavigatorSaveItem.Size = new System.Drawing.Size(23, 22);
            this.advertiserStartingBalanceBindingNavigatorSaveItem.Text = "Save Data";
            this.advertiserStartingBalanceBindingNavigatorSaveItem.Click += new System.EventHandler(this.advertiserStartingBalanceBindingNavigatorSaveItem_Click);
            // 
            // advertiserStartingBalanceDataGridView
            // 
            this.advertiserStartingBalanceDataGridView.AllowUserToAddRows = false;
            this.advertiserStartingBalanceDataGridView.AllowUserToDeleteRows = false;
            this.advertiserStartingBalanceDataGridView.AutoGenerateColumns = false;
            this.advertiserStartingBalanceDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.advertiserStartingBalanceDataGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dataGridViewTextBoxColumn1,
            this.Advertiser,
            this.dataGridViewTextBoxColumn2,
            this.dataGridViewTextBoxColumn3,
            this.dataGridViewTextBoxColumn4,
            this.dataGridViewTextBoxColumn5});
            this.advertiserStartingBalanceDataGridView.DataSource = this.advertiserStartingBalanceBindingSource;
            this.advertiserStartingBalanceDataGridView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.advertiserStartingBalanceDataGridView.Location = new System.Drawing.Point(0, 25);
            this.advertiserStartingBalanceDataGridView.Name = "advertiserStartingBalanceDataGridView";
            this.advertiserStartingBalanceDataGridView.Size = new System.Drawing.Size(525, 580);
            this.advertiserStartingBalanceDataGridView.TabIndex = 1;
            // 
            // dataGridViewTextBoxColumn1
            // 
            this.dataGridViewTextBoxColumn1.DataPropertyName = "id";
            this.dataGridViewTextBoxColumn1.HeaderText = "id";
            this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
            this.dataGridViewTextBoxColumn1.ReadOnly = true;
            this.dataGridViewTextBoxColumn1.Visible = false;
            // 
            // Advertiser
            // 
            this.Advertiser.DataPropertyName = "Advertiser";
            this.Advertiser.HeaderText = "Advertiser";
            this.Advertiser.Name = "Advertiser";
            this.Advertiser.ReadOnly = true;
            // 
            // dataGridViewTextBoxColumn2
            // 
            this.dataGridViewTextBoxColumn2.DataPropertyName = "advertiser_id";
            this.dataGridViewTextBoxColumn2.HeaderText = "advertiser_id";
            this.dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
            this.dataGridViewTextBoxColumn2.Visible = false;
            // 
            // dataGridViewTextBoxColumn3
            // 
            this.dataGridViewTextBoxColumn3.DataPropertyName = "currency";
            this.dataGridViewTextBoxColumn3.HeaderText = "Currency";
            this.dataGridViewTextBoxColumn3.Name = "dataGridViewTextBoxColumn3";
            // 
            // dataGridViewTextBoxColumn4
            // 
            this.dataGridViewTextBoxColumn4.DataPropertyName = "amount";
            this.dataGridViewTextBoxColumn4.HeaderText = "Amount";
            this.dataGridViewTextBoxColumn4.Name = "dataGridViewTextBoxColumn4";
            // 
            // dataGridViewTextBoxColumn5
            // 
            this.dataGridViewTextBoxColumn5.DataPropertyName = "effective_date";
            this.dataGridViewTextBoxColumn5.HeaderText = "effective_date";
            this.dataGridViewTextBoxColumn5.Name = "dataGridViewTextBoxColumn5";
            this.dataGridViewTextBoxColumn5.Visible = false;
            // 
            // AdvertiserStartingBalances
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(525, 605);
            this.Controls.Add(this.advertiserStartingBalanceDataGridView);
            this.Controls.Add(this.advertiserStartingBalanceBindingNavigator);
            this.Name = "AdvertiserStartingBalances";
            this.Text = "AdvertiserStartingBalances";
            this.Load += new System.EventHandler(this.AdvertiserStartingBalances_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dAMain1DataSet)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.advertiserStartingBalanceBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.advertiserStartingBalanceBindingNavigator)).EndInit();
            this.advertiserStartingBalanceBindingNavigator.ResumeLayout(false);
            this.advertiserStartingBalanceBindingNavigator.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.advertiserStartingBalanceDataGridView)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Data.DAMain1DataSet dAMain1DataSet;
        private System.Windows.Forms.BindingSource advertiserStartingBalanceBindingSource;
        private Data.DAMain1DataSetTableAdapters.AdvertiserStartingBalanceTableAdapter advertiserStartingBalanceTableAdapter;
        private Data.DAMain1DataSetTableAdapters.TableAdapterManager tableAdapterManager;
        private System.Windows.Forms.BindingNavigator advertiserStartingBalanceBindingNavigator;
        private System.Windows.Forms.ToolStripButton advertiserStartingBalanceBindingNavigatorSaveItem;
        private System.Windows.Forms.DataGridView advertiserStartingBalanceDataGridView;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn Advertiser;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn3;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn4;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn5;
        private Data.DAMain1DataSetTableAdapters.AdvertiserTableAdapter advertiserTableAdapter1;
    }
}