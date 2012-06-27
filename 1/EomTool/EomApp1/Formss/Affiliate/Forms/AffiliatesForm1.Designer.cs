namespace EomApp1.Formss.Affiliate.Forms
{
    partial class AffiliatesForm1
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AffiliatesForm1));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            this.affiliatesDataSet2 = new EomApp1.Formss.Affiliate.Data.AffiliatesDataSet2();
            this.affiliateBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.affiliateTableAdapter = new EomApp1.Formss.Affiliate.Data.AffiliatesDataSet2TableAdapters.AffiliateTableAdapter();
            this.tableAdapterManager = new EomApp1.Formss.Affiliate.Data.AffiliatesDataSet2TableAdapters.TableAdapterManager();
            this.affiliateBindingNavigator = new System.Windows.Forms.BindingNavigator(this.components);
            this.bindingNavigatorAddNewItem = new System.Windows.Forms.ToolStripButton();
            this.bindingNavigatorDeleteItem = new System.Windows.Forms.ToolStripButton();
            this.bindingNavigatorSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.bindingNavigatorSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.affiliateBindingNavigatorSaveItem = new System.Windows.Forms.ToolStripButton();
            this.affiliateDataGridView = new System.Windows.Forms.DataGridView();
            this.netTermTypeBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.affiliatesBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.affiliatePaymentMethodBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.dataItemBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.dataItemBindingSource1 = new System.Windows.Forms.BindingSource(this.components);
            this.dataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn3 = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.dataGridViewTextBoxColumn4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn5 = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.dataGridViewTextBoxColumn6 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn7 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn9 = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.dataGridViewTextBoxColumn10 = new System.Windows.Forms.DataGridViewComboBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.affiliatesDataSet2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.affiliateBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.affiliateBindingNavigator)).BeginInit();
            this.affiliateBindingNavigator.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.affiliateDataGridView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.netTermTypeBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.affiliatesBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.affiliatePaymentMethodBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataItemBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataItemBindingSource1)).BeginInit();
            this.SuspendLayout();
            // 
            // affiliatesDataSet2
            // 
            this.affiliatesDataSet2.DataSetName = "AffiliatesDataSet2";
            this.affiliatesDataSet2.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // affiliateBindingSource
            // 
            this.affiliateBindingSource.DataMember = "Affiliate";
            this.affiliateBindingSource.DataSource = this.affiliatesDataSet2;
            // 
            // affiliateTableAdapter
            // 
            this.affiliateTableAdapter.ClearBeforeFill = true;
            // 
            // tableAdapterManager
            // 
            this.tableAdapterManager.AffiliatePaymentMethodTableAdapter = null;
            this.tableAdapterManager.AffiliateTableAdapter = this.affiliateTableAdapter;
            this.tableAdapterManager.BackupDataSetBeforeUpdate = false;
            this.tableAdapterManager.CurrencyTableAdapter = null;
            this.tableAdapterManager.MediaBuyerTableAdapter = null;
            this.tableAdapterManager.NetTermTypeTableAdapter = null;
            this.tableAdapterManager.UpdateOrder = EomApp1.Formss.Affiliate.Data.AffiliatesDataSet2TableAdapters.TableAdapterManager.UpdateOrderOption.InsertUpdateDelete;
            // 
            // affiliateBindingNavigator
            // 
            this.affiliateBindingNavigator.AddNewItem = this.bindingNavigatorAddNewItem;
            this.affiliateBindingNavigator.BindingSource = this.affiliateBindingSource;
            this.affiliateBindingNavigator.CountItem = null;
            this.affiliateBindingNavigator.DeleteItem = this.bindingNavigatorDeleteItem;
            this.affiliateBindingNavigator.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.affiliateBindingNavigator.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.bindingNavigatorAddNewItem,
            this.bindingNavigatorSeparator2,
            this.bindingNavigatorDeleteItem,
            this.bindingNavigatorSeparator1,
            this.affiliateBindingNavigatorSaveItem});
            this.affiliateBindingNavigator.Location = new System.Drawing.Point(0, 0);
            this.affiliateBindingNavigator.MoveFirstItem = null;
            this.affiliateBindingNavigator.MoveLastItem = null;
            this.affiliateBindingNavigator.MoveNextItem = null;
            this.affiliateBindingNavigator.MovePreviousItem = null;
            this.affiliateBindingNavigator.Name = "affiliateBindingNavigator";
            this.affiliateBindingNavigator.PositionItem = null;
            this.affiliateBindingNavigator.Size = new System.Drawing.Size(1129, 25);
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
            // bindingNavigatorSeparator2
            // 
            this.bindingNavigatorSeparator2.Name = "bindingNavigatorSeparator2";
            this.bindingNavigatorSeparator2.Size = new System.Drawing.Size(6, 25);
            // 
            // bindingNavigatorSeparator1
            // 
            this.bindingNavigatorSeparator1.Name = "bindingNavigatorSeparator1";
            this.bindingNavigatorSeparator1.Size = new System.Drawing.Size(6, 25);
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
            // affiliateDataGridView
            // 
            this.affiliateDataGridView.AutoGenerateColumns = false;
            this.affiliateDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.affiliateDataGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dataGridViewTextBoxColumn1,
            this.dataGridViewTextBoxColumn2,
            this.dataGridViewTextBoxColumn3,
            this.dataGridViewTextBoxColumn4,
            this.dataGridViewTextBoxColumn5,
            this.dataGridViewTextBoxColumn6,
            this.dataGridViewTextBoxColumn7,
            this.dataGridViewTextBoxColumn9,
            this.dataGridViewTextBoxColumn10});
            this.affiliateDataGridView.DataSource = this.affiliateBindingSource;
            this.affiliateDataGridView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.affiliateDataGridView.Location = new System.Drawing.Point(0, 25);
            this.affiliateDataGridView.Name = "affiliateDataGridView";
            this.affiliateDataGridView.Size = new System.Drawing.Size(1129, 443);
            this.affiliateDataGridView.TabIndex = 1;
            // 
            // netTermTypeBindingSource
            // 
            this.netTermTypeBindingSource.DataSource = typeof(EomApp1.Formss.Affiliate.Data.NetTermType);
            // 
            // affiliatesBindingSource
            // 
            this.affiliatesBindingSource.DataMember = "Affiliates";
            this.affiliatesBindingSource.DataSource = this.netTermTypeBindingSource;
            // 
            // affiliatePaymentMethodBindingSource
            // 
            this.affiliatePaymentMethodBindingSource.DataSource = typeof(EomApp1.Formss.Affiliate.Data.AffiliatePaymentMethod);
            // 
            // dataItemBindingSource
            // 
            this.dataItemBindingSource.DataSource = typeof(EomApp1.Formss.Affiliate.Data.DataItem);
            // 
            // dataItemBindingSource1
            // 
            this.dataItemBindingSource1.DataSource = typeof(EomApp1.Formss.Affiliate.Data.DataItem);
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
            this.dataGridViewTextBoxColumn2.HeaderText = "Affilaite";
            this.dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
            // 
            // dataGridViewTextBoxColumn3
            // 
            this.dataGridViewTextBoxColumn3.DataPropertyName = "media_buyer_id";
            this.dataGridViewTextBoxColumn3.DataSource = this.dataItemBindingSource;
            this.dataGridViewTextBoxColumn3.DisplayMember = "Name";
            this.dataGridViewTextBoxColumn3.DisplayStyle = System.Windows.Forms.DataGridViewComboBoxDisplayStyle.ComboBox;
            this.dataGridViewTextBoxColumn3.DisplayStyleForCurrentCellOnly = true;
            this.dataGridViewTextBoxColumn3.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.dataGridViewTextBoxColumn3.HeaderText = "Media Buyer";
            this.dataGridViewTextBoxColumn3.Name = "dataGridViewTextBoxColumn3";
            this.dataGridViewTextBoxColumn3.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridViewTextBoxColumn3.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.dataGridViewTextBoxColumn3.ValueMember = "Id";
            // 
            // dataGridViewTextBoxColumn4
            // 
            this.dataGridViewTextBoxColumn4.DataPropertyName = "affid";
            this.dataGridViewTextBoxColumn4.HeaderText = "AffId";
            this.dataGridViewTextBoxColumn4.Name = "dataGridViewTextBoxColumn4";
            // 
            // dataGridViewTextBoxColumn5
            // 
            this.dataGridViewTextBoxColumn5.DataPropertyName = "currency_id";
            this.dataGridViewTextBoxColumn5.DataSource = this.dataItemBindingSource1;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.dataGridViewTextBoxColumn5.DefaultCellStyle = dataGridViewCellStyle1;
            this.dataGridViewTextBoxColumn5.DisplayMember = "Name";
            this.dataGridViewTextBoxColumn5.DisplayStyle = System.Windows.Forms.DataGridViewComboBoxDisplayStyle.ComboBox;
            this.dataGridViewTextBoxColumn5.DisplayStyleForCurrentCellOnly = true;
            this.dataGridViewTextBoxColumn5.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.dataGridViewTextBoxColumn5.HeaderText = "Curr";
            this.dataGridViewTextBoxColumn5.Name = "dataGridViewTextBoxColumn5";
            this.dataGridViewTextBoxColumn5.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridViewTextBoxColumn5.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.dataGridViewTextBoxColumn5.ValueMember = "Id";
            // 
            // dataGridViewTextBoxColumn6
            // 
            this.dataGridViewTextBoxColumn6.DataPropertyName = "email";
            this.dataGridViewTextBoxColumn6.HeaderText = "Email";
            this.dataGridViewTextBoxColumn6.Name = "dataGridViewTextBoxColumn6";
            // 
            // dataGridViewTextBoxColumn7
            // 
            this.dataGridViewTextBoxColumn7.DataPropertyName = "add_code";
            this.dataGridViewTextBoxColumn7.HeaderText = "CD#";
            this.dataGridViewTextBoxColumn7.Name = "dataGridViewTextBoxColumn7";
            // 
            // dataGridViewTextBoxColumn9
            // 
            this.dataGridViewTextBoxColumn9.DataPropertyName = "net_term_type_id";
            this.dataGridViewTextBoxColumn9.DataSource = this.netTermTypeBindingSource;
            this.dataGridViewTextBoxColumn9.DisplayMember = "name";
            this.dataGridViewTextBoxColumn9.DisplayStyle = System.Windows.Forms.DataGridViewComboBoxDisplayStyle.ComboBox;
            this.dataGridViewTextBoxColumn9.DisplayStyleForCurrentCellOnly = true;
            this.dataGridViewTextBoxColumn9.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.dataGridViewTextBoxColumn9.HeaderText = "Net Term";
            this.dataGridViewTextBoxColumn9.Name = "dataGridViewTextBoxColumn9";
            this.dataGridViewTextBoxColumn9.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridViewTextBoxColumn9.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.dataGridViewTextBoxColumn9.ValueMember = "id";
            // 
            // dataGridViewTextBoxColumn10
            // 
            this.dataGridViewTextBoxColumn10.DataPropertyName = "payment_method_id";
            this.dataGridViewTextBoxColumn10.DataSource = this.affiliatePaymentMethodBindingSource;
            this.dataGridViewTextBoxColumn10.DisplayMember = "name";
            this.dataGridViewTextBoxColumn10.DisplayStyle = System.Windows.Forms.DataGridViewComboBoxDisplayStyle.ComboBox;
            this.dataGridViewTextBoxColumn10.DisplayStyleForCurrentCellOnly = true;
            this.dataGridViewTextBoxColumn10.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.dataGridViewTextBoxColumn10.HeaderText = "Pay Meth";
            this.dataGridViewTextBoxColumn10.Name = "dataGridViewTextBoxColumn10";
            this.dataGridViewTextBoxColumn10.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridViewTextBoxColumn10.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.dataGridViewTextBoxColumn10.ValueMember = "id";
            // 
            // AffiliatesForm1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1129, 468);
            this.Controls.Add(this.affiliateDataGridView);
            this.Controls.Add(this.affiliateBindingNavigator);
            this.Name = "AffiliatesForm1";
            this.Text = "AffiliatesForm1";
            this.Load += new System.EventHandler(this.AffiliatesForm1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.affiliatesDataSet2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.affiliateBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.affiliateBindingNavigator)).EndInit();
            this.affiliateBindingNavigator.ResumeLayout(false);
            this.affiliateBindingNavigator.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.affiliateDataGridView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.netTermTypeBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.affiliatesBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.affiliatePaymentMethodBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataItemBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataItemBindingSource1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Data.AffiliatesDataSet2 affiliatesDataSet2;
        private System.Windows.Forms.BindingSource affiliateBindingSource;
        private Data.AffiliatesDataSet2TableAdapters.AffiliateTableAdapter affiliateTableAdapter;
        private Data.AffiliatesDataSet2TableAdapters.TableAdapterManager tableAdapterManager;
        private System.Windows.Forms.BindingNavigator affiliateBindingNavigator;
        private System.Windows.Forms.ToolStripButton bindingNavigatorAddNewItem;
        private System.Windows.Forms.ToolStripButton bindingNavigatorDeleteItem;
        private System.Windows.Forms.ToolStripSeparator bindingNavigatorSeparator1;
        private System.Windows.Forms.ToolStripSeparator bindingNavigatorSeparator2;
        private System.Windows.Forms.ToolStripButton affiliateBindingNavigatorSaveItem;
        private System.Windows.Forms.DataGridView affiliateDataGridView;
        private System.Windows.Forms.BindingSource dataItemBindingSource;
        private System.Windows.Forms.BindingSource dataItemBindingSource1;
        private System.Windows.Forms.BindingSource netTermTypeBindingSource;
        private System.Windows.Forms.BindingSource affiliatesBindingSource;
        private System.Windows.Forms.BindingSource affiliatePaymentMethodBindingSource;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
        private System.Windows.Forms.DataGridViewComboBoxColumn dataGridViewTextBoxColumn3;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn4;
        private System.Windows.Forms.DataGridViewComboBoxColumn dataGridViewTextBoxColumn5;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn6;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn7;
        private System.Windows.Forms.DataGridViewComboBoxColumn dataGridViewTextBoxColumn9;
        private System.Windows.Forms.DataGridViewComboBoxColumn dataGridViewTextBoxColumn10;
    }
}