namespace EomApp1.Formss.Accounting.Controls
{
    partial class Currencies
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Currencies));
            this.accountingDataSet = new EomApp1.Formss.Accounting.Data.AccountingDataSet();
            this.currencyBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.currencyTableAdapter = new EomApp1.Formss.Accounting.Data.AccountingDataSetTableAdapters.CurrencyTableAdapter();
            this.tableAdapterManager = new EomApp1.Formss.Accounting.Data.AccountingDataSetTableAdapters.TableAdapterManager();
            this.currencyBindingNavigator = new System.Windows.Forms.BindingNavigator(this.components);
            this.currencyBindingNavigatorSaveItem = new System.Windows.Forms.ToolStripButton();
            this.currencyDataGridView = new System.Windows.Forms.DataGridView();
            this.dataGridViewTextBoxColumn2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.accountingDataSet)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.currencyBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.currencyBindingNavigator)).BeginInit();
            this.currencyBindingNavigator.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.currencyDataGridView)).BeginInit();
            this.SuspendLayout();
            // 
            // accountingDataSet
            // 
            this.accountingDataSet.DataSetName = "AccountingDataSet";
            this.accountingDataSet.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // currencyBindingSource
            // 
            this.currencyBindingSource.DataMember = "Currency";
            this.currencyBindingSource.DataSource = this.accountingDataSet;
            // 
            // currencyTableAdapter
            // 
            this.currencyTableAdapter.ClearBeforeFill = true;
            // 
            // tableAdapterManager
            // 
            this.tableAdapterManager.BackupDataSetBeforeUpdate = false;
            this.tableAdapterManager.CurrencyTableAdapter = this.currencyTableAdapter;
            this.tableAdapterManager.UpdateOrder = EomApp1.Formss.Accounting.Data.AccountingDataSetTableAdapters.TableAdapterManager.UpdateOrderOption.InsertUpdateDelete;
            // 
            // currencyBindingNavigator
            // 
            this.currencyBindingNavigator.AddNewItem = null;
            this.currencyBindingNavigator.BindingSource = this.currencyBindingSource;
            this.currencyBindingNavigator.CountItem = null;
            this.currencyBindingNavigator.DeleteItem = null;
            this.currencyBindingNavigator.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.currencyBindingNavigator.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.currencyBindingNavigatorSaveItem});
            this.currencyBindingNavigator.Location = new System.Drawing.Point(0, 0);
            this.currencyBindingNavigator.MoveFirstItem = null;
            this.currencyBindingNavigator.MoveLastItem = null;
            this.currencyBindingNavigator.MoveNextItem = null;
            this.currencyBindingNavigator.MovePreviousItem = null;
            this.currencyBindingNavigator.Name = "currencyBindingNavigator";
            this.currencyBindingNavigator.PositionItem = null;
            this.currencyBindingNavigator.Size = new System.Drawing.Size(245, 25);
            this.currencyBindingNavigator.TabIndex = 0;
            this.currencyBindingNavigator.Text = "bindingNavigator1";
            // 
            // currencyBindingNavigatorSaveItem
            // 
            this.currencyBindingNavigatorSaveItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.currencyBindingNavigatorSaveItem.Image = ((System.Drawing.Image)(resources.GetObject("currencyBindingNavigatorSaveItem.Image")));
            this.currencyBindingNavigatorSaveItem.Name = "currencyBindingNavigatorSaveItem";
            this.currencyBindingNavigatorSaveItem.Size = new System.Drawing.Size(23, 22);
            this.currencyBindingNavigatorSaveItem.Text = "Save Data";
            this.currencyBindingNavigatorSaveItem.Click += new System.EventHandler(this.currencyBindingNavigatorSaveItem_Click);
            // 
            // currencyDataGridView
            // 
            this.currencyDataGridView.AllowUserToAddRows = false;
            this.currencyDataGridView.AllowUserToDeleteRows = false;
            this.currencyDataGridView.AutoGenerateColumns = false;
            this.currencyDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.currencyDataGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dataGridViewTextBoxColumn2,
            this.dataGridViewTextBoxColumn3});
            this.currencyDataGridView.DataSource = this.currencyBindingSource;
            this.currencyDataGridView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.currencyDataGridView.Location = new System.Drawing.Point(0, 25);
            this.currencyDataGridView.Name = "currencyDataGridView";
            this.currencyDataGridView.RowHeadersVisible = false;
            this.currencyDataGridView.Size = new System.Drawing.Size(245, 204);
            this.currencyDataGridView.TabIndex = 1;
            // 
            // dataGridViewTextBoxColumn2
            // 
            this.dataGridViewTextBoxColumn2.DataPropertyName = "name";
            this.dataGridViewTextBoxColumn2.HeaderText = "Currency";
            this.dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
            this.dataGridViewTextBoxColumn2.ReadOnly = true;
            // 
            // dataGridViewTextBoxColumn3
            // 
            this.dataGridViewTextBoxColumn3.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.dataGridViewTextBoxColumn3.DataPropertyName = "to_usd_multiplier";
            this.dataGridViewTextBoxColumn3.HeaderText = "To USD Multiplier";
            this.dataGridViewTextBoxColumn3.Name = "dataGridViewTextBoxColumn3";
            // 
            // Currencies
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.currencyDataGridView);
            this.Controls.Add(this.currencyBindingNavigator);
            this.Name = "Currencies";
            this.Size = new System.Drawing.Size(245, 229);
            ((System.ComponentModel.ISupportInitialize)(this.accountingDataSet)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.currencyBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.currencyBindingNavigator)).EndInit();
            this.currencyBindingNavigator.ResumeLayout(false);
            this.currencyBindingNavigator.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.currencyDataGridView)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Data.AccountingDataSet accountingDataSet;
        private System.Windows.Forms.BindingSource currencyBindingSource;
        private Data.AccountingDataSetTableAdapters.CurrencyTableAdapter currencyTableAdapter;
        private Data.AccountingDataSetTableAdapters.TableAdapterManager tableAdapterManager;
        private System.Windows.Forms.BindingNavigator currencyBindingNavigator;
        private System.Windows.Forms.ToolStripButton currencyBindingNavigatorSaveItem;
        private System.Windows.Forms.DataGridView currencyDataGridView;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn3;
    }
}
