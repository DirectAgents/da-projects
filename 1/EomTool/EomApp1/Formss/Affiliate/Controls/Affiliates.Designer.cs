namespace EomApp1.Formss.Affiliate.Controls
{
    partial class Affiliates
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Affiliates));
            this.affiliateBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.affiliatesDataSet21 = new EomApp1.Formss.Affiliate.Data.AffiliatesDataSet2();
            this.affiliateTableAdapter = new EomApp1.Formss.Affiliate.Data.AffiliatesDataSetTableAdapters.AffiliateTableAdapter();
            this.tableAdapterManager = new EomApp1.Formss.Affiliate.Data.AffiliatesDataSetTableAdapters.TableAdapterManager();
            this.affiliateBindingNavigator = new System.Windows.Forms.BindingNavigator(this.components);
            this.bindingNavigatorAddNewItem = new System.Windows.Forms.ToolStripButton();
            this.bindingNavigatorDeleteItem = new System.Windows.Forms.ToolStripButton();
            this.toolStripLabel2 = new System.Windows.Forms.ToolStripLabel();
            this.toolStripLabel3 = new System.Windows.Forms.ToolStripLabel();
            this.affiliateBindingNavigatorSaveItem = new System.Windows.Forms.ToolStripButton();
            this.toolStripLabel1 = new System.Windows.Forms.ToolStripLabel();
            this.toolStripButton1 = new System.Windows.Forms.ToolStripButton();
            this.affiliateDataGridView = new System.Windows.Forms.DataGridView();
            this.dataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn3 = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.mediaBuyerBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.colAffId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn7 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn6 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colCurrency = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.currencyBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.net_term_type_id = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.netTermTypeBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.payment_method_id = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.affiliatePaymentMethodBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.currencyTableAdapter = new EomApp1.Formss.Affiliate.Data.AffiliatesDataSetTableAdapters.CurrencyTableAdapter();
            this.mediaBuyerTableAdapter = new EomApp1.Formss.Affiliate.Data.AffiliatesDataSetTableAdapters.MediaBuyerTableAdapter();
            this.netTermTypeTableAdapter = new EomApp1.Formss.Affiliate.Data.AffiliatesDataSetTableAdapters.NetTermTypeTableAdapter();
            this.affiliateTableAdapter1 = new EomApp1.Formss.Affiliate.Data.AffiliatesDataSet2TableAdapters.AffiliateTableAdapter();
            this.affiliatePaymentMethodTableAdapter = new EomApp1.Formss.Affiliate.Data.AffiliatesDataSet2TableAdapters.AffiliatePaymentMethodTableAdapter();
            ((System.ComponentModel.ISupportInitialize)(this.affiliateBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.affiliatesDataSet21)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.affiliateBindingNavigator)).BeginInit();
            this.affiliateBindingNavigator.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.affiliateDataGridView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.mediaBuyerBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.currencyBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.netTermTypeBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.affiliatePaymentMethodBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // affiliateBindingSource
            // 
            this.affiliateBindingSource.DataMember = "Affiliate";
            this.affiliateBindingSource.DataSource = this.affiliatesDataSet21;
            // 
            // affiliatesDataSet21
            // 
            this.affiliatesDataSet21.DataSetName = "AffiliatesDataSet2";
            this.affiliatesDataSet21.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // affiliateTableAdapter
            // 
            this.affiliateTableAdapter.ClearBeforeFill = true;
            // 
            // tableAdapterManager
            // 
            this.tableAdapterManager.AffiliateTableAdapter = this.affiliateTableAdapter;
            this.tableAdapterManager.BackupDataSetBeforeUpdate = false;
            this.tableAdapterManager.CurrencyTableAdapter = null;
            this.tableAdapterManager.MediaBuyerTableAdapter = null;
            this.tableAdapterManager.NetTermTypeTableAdapter = null;
            this.tableAdapterManager.UpdateOrder = EomApp1.Formss.Affiliate.Data.AffiliatesDataSetTableAdapters.TableAdapterManager.UpdateOrderOption.InsertUpdateDelete;
            // 
            // affiliateBindingNavigator
            // 
            this.affiliateBindingNavigator.AddNewItem = this.bindingNavigatorAddNewItem;
            this.affiliateBindingNavigator.BackColor = System.Drawing.Color.Black;
            this.affiliateBindingNavigator.BindingSource = this.affiliateBindingSource;
            this.affiliateBindingNavigator.CountItem = null;
            this.affiliateBindingNavigator.DeleteItem = this.bindingNavigatorDeleteItem;
            this.affiliateBindingNavigator.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.affiliateBindingNavigator.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.bindingNavigatorAddNewItem,
            this.toolStripLabel2,
            this.bindingNavigatorDeleteItem,
            this.toolStripLabel3,
            this.affiliateBindingNavigatorSaveItem,
            this.toolStripLabel1,
            this.toolStripButton1});
            this.affiliateBindingNavigator.Location = new System.Drawing.Point(0, 0);
            this.affiliateBindingNavigator.MoveFirstItem = null;
            this.affiliateBindingNavigator.MoveLastItem = null;
            this.affiliateBindingNavigator.MoveNextItem = null;
            this.affiliateBindingNavigator.MovePreviousItem = null;
            this.affiliateBindingNavigator.Name = "affiliateBindingNavigator";
            this.affiliateBindingNavigator.PositionItem = null;
            this.affiliateBindingNavigator.Size = new System.Drawing.Size(977, 25);
            this.affiliateBindingNavigator.TabIndex = 0;
            this.affiliateBindingNavigator.Text = "bindingNavigator1";
            // 
            // bindingNavigatorAddNewItem
            // 
            this.bindingNavigatorAddNewItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.bindingNavigatorAddNewItem.Image = ((System.Drawing.Image)(resources.GetObject("bindingNavigatorAddNewItem.Image")));
            this.bindingNavigatorAddNewItem.Name = "bindingNavigatorAddNewItem";
            this.bindingNavigatorAddNewItem.RightToLeftAutoMirrorImage = true;
            this.bindingNavigatorAddNewItem.Size = new System.Drawing.Size(23, 22);
            this.bindingNavigatorAddNewItem.Text = "Add new";
            // 
            // bindingNavigatorDeleteItem
            // 
            this.bindingNavigatorDeleteItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.bindingNavigatorDeleteItem.Image = ((System.Drawing.Image)(resources.GetObject("bindingNavigatorDeleteItem.Image")));
            this.bindingNavigatorDeleteItem.Name = "bindingNavigatorDeleteItem";
            this.bindingNavigatorDeleteItem.RightToLeftAutoMirrorImage = true;
            this.bindingNavigatorDeleteItem.Size = new System.Drawing.Size(23, 22);
            this.bindingNavigatorDeleteItem.Text = "Delete";
            // 
            // toolStripLabel2
            // 
            this.toolStripLabel2.Name = "toolStripLabel2";
            this.toolStripLabel2.Size = new System.Drawing.Size(22, 22);
            this.toolStripLabel2.Text = "     ";
            // 
            // toolStripLabel3
            // 
            this.toolStripLabel3.Name = "toolStripLabel3";
            this.toolStripLabel3.Size = new System.Drawing.Size(22, 22);
            this.toolStripLabel3.Text = "     ";
            // 
            // affiliateBindingNavigatorSaveItem
            // 
            this.affiliateBindingNavigatorSaveItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.affiliateBindingNavigatorSaveItem.Image = ((System.Drawing.Image)(resources.GetObject("affiliateBindingNavigatorSaveItem.Image")));
            this.affiliateBindingNavigatorSaveItem.Name = "affiliateBindingNavigatorSaveItem";
            this.affiliateBindingNavigatorSaveItem.Size = new System.Drawing.Size(23, 22);
            this.affiliateBindingNavigatorSaveItem.Text = "Save Data";
            this.affiliateBindingNavigatorSaveItem.Click += new System.EventHandler(this.affiliateBindingNavigatorSaveItem_Click);
            // 
            // toolStripLabel1
            // 
            this.toolStripLabel1.Name = "toolStripLabel1";
            this.toolStripLabel1.Size = new System.Drawing.Size(22, 22);
            this.toolStripLabel1.Text = "     ";
            // 
            // toolStripButton1
            // 
            this.toolStripButton1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripButton1.ForeColor = System.Drawing.Color.White;
            this.toolStripButton1.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton1.Image")));
            this.toolStripButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton1.Name = "toolStripButton1";
            this.toolStripButton1.Size = new System.Drawing.Size(69, 22);
            this.toolStripButton1.Text = "Synch MBs";
            this.toolStripButton1.Click += new System.EventHandler(this.toolStripButton1_Click);
            // 
            // affiliateDataGridView
            // 
            this.affiliateDataGridView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.affiliateDataGridView.AutoGenerateColumns = false;
            this.affiliateDataGridView.BackgroundColor = System.Drawing.Color.Black;
            this.affiliateDataGridView.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.affiliateDataGridView.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            this.affiliateDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.affiliateDataGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dataGridViewTextBoxColumn1,
            this.dataGridViewTextBoxColumn2,
            this.dataGridViewTextBoxColumn3,
            this.colAffId,
            this.dataGridViewTextBoxColumn7,
            this.dataGridViewTextBoxColumn6,
            this.colCurrency,
            this.net_term_type_id,
            this.payment_method_id});
            this.affiliateDataGridView.DataSource = this.affiliateBindingSource;
            this.affiliateDataGridView.GridColor = System.Drawing.Color.Silver;
            this.affiliateDataGridView.Location = new System.Drawing.Point(3, 28);
            this.affiliateDataGridView.Margin = new System.Windows.Forms.Padding(0);
            this.affiliateDataGridView.Name = "affiliateDataGridView";
            this.affiliateDataGridView.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            this.affiliateDataGridView.RowHeadersVisible = false;
            this.affiliateDataGridView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.affiliateDataGridView.Size = new System.Drawing.Size(974, 530);
            this.affiliateDataGridView.TabIndex = 1;
            // 
            // dataGridViewTextBoxColumn1
            // 
            this.dataGridViewTextBoxColumn1.DataPropertyName = "id";
            this.dataGridViewTextBoxColumn1.HeaderText = "id";
            this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
            this.dataGridViewTextBoxColumn1.ReadOnly = true;
            this.dataGridViewTextBoxColumn1.Visible = false;
            // 
            // dataGridViewTextBoxColumn2
            // 
            this.dataGridViewTextBoxColumn2.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.dataGridViewTextBoxColumn2.DataPropertyName = "name";
            this.dataGridViewTextBoxColumn2.HeaderText = "Affiliate";
            this.dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
            // 
            // dataGridViewTextBoxColumn3
            // 
            this.dataGridViewTextBoxColumn3.DataPropertyName = "media_buyer_id";
            this.dataGridViewTextBoxColumn3.DataSource = this.mediaBuyerBindingSource;
            this.dataGridViewTextBoxColumn3.DisplayMember = "name";
            this.dataGridViewTextBoxColumn3.DisplayStyle = System.Windows.Forms.DataGridViewComboBoxDisplayStyle.ComboBox;
            this.dataGridViewTextBoxColumn3.DisplayStyleForCurrentCellOnly = true;
            this.dataGridViewTextBoxColumn3.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.dataGridViewTextBoxColumn3.HeaderText = "Media Buyer";
            this.dataGridViewTextBoxColumn3.Name = "dataGridViewTextBoxColumn3";
            this.dataGridViewTextBoxColumn3.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridViewTextBoxColumn3.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.dataGridViewTextBoxColumn3.ValueMember = "id";
            // 
            // colAffId
            // 
            this.colAffId.DataPropertyName = "affid";
            this.colAffId.HeaderText = "AffId";
            this.colAffId.Name = "colAffId";
            // 
            // dataGridViewTextBoxColumn7
            // 
            this.dataGridViewTextBoxColumn7.DataPropertyName = "add_code";
            this.dataGridViewTextBoxColumn7.HeaderText = "CD Number";
            this.dataGridViewTextBoxColumn7.Name = "dataGridViewTextBoxColumn7";
            // 
            // dataGridViewTextBoxColumn6
            // 
            this.dataGridViewTextBoxColumn6.DataPropertyName = "email";
            this.dataGridViewTextBoxColumn6.HeaderText = "Email";
            this.dataGridViewTextBoxColumn6.Name = "dataGridViewTextBoxColumn6";
            // 
            // colCurrency
            // 
            this.colCurrency.DataPropertyName = "currency_id";
            this.colCurrency.DataSource = this.currencyBindingSource;
            this.colCurrency.DisplayMember = "name";
            this.colCurrency.DisplayStyle = System.Windows.Forms.DataGridViewComboBoxDisplayStyle.ComboBox;
            this.colCurrency.DisplayStyleForCurrentCellOnly = true;
            this.colCurrency.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.colCurrency.HeaderText = "Curr";
            this.colCurrency.Name = "colCurrency";
            this.colCurrency.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.colCurrency.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.colCurrency.ValueMember = "id";
            this.colCurrency.Width = 75;
            // 
            // net_term_type_id
            // 
            this.net_term_type_id.DataPropertyName = "net_term_type_id";
            this.net_term_type_id.DataSource = this.netTermTypeBindingSource;
            this.net_term_type_id.DisplayMember = "name";
            this.net_term_type_id.DisplayStyle = System.Windows.Forms.DataGridViewComboBoxDisplayStyle.ComboBox;
            this.net_term_type_id.DisplayStyleForCurrentCellOnly = true;
            this.net_term_type_id.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.net_term_type_id.HeaderText = "Net Terms";
            this.net_term_type_id.Name = "net_term_type_id";
            this.net_term_type_id.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.net_term_type_id.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.net_term_type_id.ValueMember = "id";
            // 
            // payment_method_id
            // 
            this.payment_method_id.DataPropertyName = "payment_method_id";
            this.payment_method_id.DataSource = this.affiliatePaymentMethodBindingSource;
            this.payment_method_id.DisplayMember = "name";
            this.payment_method_id.DisplayStyle = System.Windows.Forms.DataGridViewComboBoxDisplayStyle.ComboBox;
            this.payment_method_id.DisplayStyleForCurrentCellOnly = true;
            this.payment_method_id.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.payment_method_id.HeaderText = "Pay Method";
            this.payment_method_id.Name = "payment_method_id";
            this.payment_method_id.ValueMember = "id";
            // 
            // affiliatePaymentMethodBindingSource
            // 
            this.affiliatePaymentMethodBindingSource.DataMember = "AffiliatePaymentMethod";
            this.affiliatePaymentMethodBindingSource.DataSource = this.affiliatesDataSet21;
            // 
            // currencyTableAdapter
            // 
            this.currencyTableAdapter.ClearBeforeFill = true;
            // 
            // mediaBuyerTableAdapter
            // 
            this.mediaBuyerTableAdapter.ClearBeforeFill = true;
            // 
            // netTermTypeTableAdapter
            // 
            this.netTermTypeTableAdapter.ClearBeforeFill = true;
            // 
            // affiliateTableAdapter1
            // 
            this.affiliateTableAdapter1.ClearBeforeFill = true;
            // 
            // affiliatePaymentMethodTableAdapter
            // 
            this.affiliatePaymentMethodTableAdapter.ClearBeforeFill = true;
            // 
            // Affiliates
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.affiliateDataGridView);
            this.Controls.Add(this.affiliateBindingNavigator);
            this.Name = "Affiliates";
            this.Size = new System.Drawing.Size(977, 558);
            this.Load += new System.EventHandler(this.Affiliates_Load);
            ((System.ComponentModel.ISupportInitialize)(this.affiliateBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.affiliatesDataSet21)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.affiliateBindingNavigator)).EndInit();
            this.affiliateBindingNavigator.ResumeLayout(false);
            this.affiliateBindingNavigator.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.affiliateDataGridView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.mediaBuyerBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.currencyBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.netTermTypeBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.affiliatePaymentMethodBindingSource)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.BindingSource affiliateBindingSource;
        private Data.AffiliatesDataSetTableAdapters.AffiliateTableAdapter affiliateTableAdapter;
        private Data.AffiliatesDataSetTableAdapters.TableAdapterManager tableAdapterManager;
        private System.Windows.Forms.BindingNavigator affiliateBindingNavigator;
        private System.Windows.Forms.ToolStripButton bindingNavigatorAddNewItem;
        private System.Windows.Forms.ToolStripButton bindingNavigatorDeleteItem;
        private System.Windows.Forms.ToolStripButton affiliateBindingNavigatorSaveItem;
        private System.Windows.Forms.DataGridView affiliateDataGridView;
        private System.Windows.Forms.ToolStripLabel toolStripLabel2;
        private System.Windows.Forms.ToolStripLabel toolStripLabel3;
        private System.Windows.Forms.ToolStripLabel toolStripLabel1;
        private System.Windows.Forms.BindingSource mediaBuyerBindingSource;
        private System.Windows.Forms.BindingSource currencyBindingSource;
        private Data.AffiliatesDataSetTableAdapters.CurrencyTableAdapter currencyTableAdapter;
        private Data.AffiliatesDataSetTableAdapters.MediaBuyerTableAdapter mediaBuyerTableAdapter;
        private System.Windows.Forms.ToolStripButton toolStripButton1;
        private System.Windows.Forms.BindingSource netTermTypeBindingSource;
        private Data.AffiliatesDataSetTableAdapters.NetTermTypeTableAdapter netTermTypeTableAdapter;
        private Data.AffiliatesDataSet2 affiliatesDataSet21;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
        private System.Windows.Forms.DataGridViewComboBoxColumn dataGridViewTextBoxColumn3;
        private System.Windows.Forms.DataGridViewTextBoxColumn colAffId;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn7;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn6;
        private System.Windows.Forms.DataGridViewComboBoxColumn colCurrency;
        private System.Windows.Forms.DataGridViewComboBoxColumn net_term_type_id;
        private System.Windows.Forms.DataGridViewComboBoxColumn payment_method_id;
        private System.Windows.Forms.BindingSource affiliatePaymentMethodBindingSource;
        private Data.AffiliatesDataSet2TableAdapters.AffiliateTableAdapter affiliateTableAdapter1;
        private Data.AffiliatesDataSet2TableAdapters.AffiliatePaymentMethodTableAdapter affiliatePaymentMethodTableAdapter;
    }
}
