namespace EomApp1.Formss.Campaign
{
    partial class CampaignsUC
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CampaignsUC));
            this.campaignDataSet = new EomApp1.Formss.Campaign.CampaignDataSet();
            this.campaignBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.campaignTableAdapter = new EomApp1.Formss.Campaign.CampaignDataSetTableAdapters.CampaignTableAdapter();
            this.tableAdapterManager = new EomApp1.Formss.Campaign.CampaignDataSetTableAdapters.TableAdapterManager();
            this.campaignBindingNavigator = new System.Windows.Forms.BindingNavigator(this.components);
            this.bindingNavigatorAddNewItem = new System.Windows.Forms.ToolStripButton();
            this.bindingNavigatorDeleteItem = new System.Windows.Forms.ToolStripButton();
            this.toolStripLabel1 = new System.Windows.Forms.ToolStripLabel();
            this.toolStripLabel2 = new System.Windows.Forms.ToolStripLabel();
            this.campaignBindingNavigatorSaveItem = new System.Windows.Forms.ToolStripButton();
            this.campaignDataGridView = new System.Windows.Forms.DataGridView();
            this.dataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn2 = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.accountManagerBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.campaignDataSetBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.dataGridViewTextBoxColumn4 = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.adManagerBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.dataGridViewTextBoxColumn5 = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.advertiserBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.dataGridViewTextBoxColumn6 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn7 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn3 = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.campaignStatusBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.dataGridViewTextBoxColumn11 = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.dTCampaignStatusBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.dataGridViewTextBoxColumn12 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn14 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewCheckBoxColumn1 = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.dataGridViewCheckBoxColumn2 = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.dataGridViewCheckBoxColumn3 = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.dataGridViewCheckBoxColumn4 = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.dataGridViewTextBoxColumn15 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn9 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn10 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn16 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dTCampaignTypeBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.accountManagerTableAdapter = new EomApp1.Formss.Campaign.CampaignDataSetTableAdapters.AccountManagerTableAdapter();
            this.campaignStatusTableAdapter = new EomApp1.Formss.Campaign.CampaignDataSetTableAdapters.CampaignStatusTableAdapter();
            this.adManagerTableAdapter = new EomApp1.Formss.Campaign.CampaignDataSetTableAdapters.AdManagerTableAdapter();
            this.advertiserTableAdapter = new EomApp1.Formss.Campaign.CampaignDataSetTableAdapters.AdvertiserTableAdapter();
            this.dTCampaignStatusTableAdapter = new EomApp1.Formss.Campaign.CampaignDataSetTableAdapters.DTCampaignStatusTableAdapter();
            this.dTCampaignTypeTableAdapter = new EomApp1.Formss.Campaign.CampaignDataSetTableAdapters.DTCampaignTypeTableAdapter();
            ((System.ComponentModel.ISupportInitialize)(this.campaignDataSet)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.campaignBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.campaignBindingNavigator)).BeginInit();
            this.campaignBindingNavigator.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.campaignDataGridView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.accountManagerBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.campaignDataSetBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.adManagerBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.advertiserBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.campaignStatusBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dTCampaignStatusBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dTCampaignTypeBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // campaignDataSet
            // 
            this.campaignDataSet.DataSetName = "CampaignDataSet";
            this.campaignDataSet.EnforceConstraints = false;
            this.campaignDataSet.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // campaignBindingSource
            // 
            this.campaignBindingSource.DataMember = "Campaign";
            this.campaignBindingSource.DataSource = this.campaignDataSet;
            // 
            // campaignTableAdapter
            // 
            this.campaignTableAdapter.ClearBeforeFill = true;
            // 
            // tableAdapterManager
            // 
            this.tableAdapterManager.AccountManagerTableAdapter = null;
            this.tableAdapterManager.AdManagerTableAdapter = null;
            this.tableAdapterManager.AdvertiserTableAdapter = null;
            this.tableAdapterManager.BackupDataSetBeforeUpdate = false;
            this.tableAdapterManager.CampaignStatusTableAdapter = null;
            this.tableAdapterManager.CampaignTableAdapter = this.campaignTableAdapter;
            this.tableAdapterManager.DTCampaignAllowedCountriesTableAdapter = null;
            this.tableAdapterManager.DTCampaignStatusTableAdapter = null;
            this.tableAdapterManager.DTCampaignTypeTableAdapter = null;
            this.tableAdapterManager.UpdateOrder = EomApp1.Formss.Campaign.CampaignDataSetTableAdapters.TableAdapterManager.UpdateOrderOption.InsertUpdateDelete;
            // 
            // campaignBindingNavigator
            // 
            this.campaignBindingNavigator.AddNewItem = this.bindingNavigatorAddNewItem;
            this.campaignBindingNavigator.BindingSource = this.campaignBindingSource;
            this.campaignBindingNavigator.CountItem = null;
            this.campaignBindingNavigator.DeleteItem = this.bindingNavigatorDeleteItem;
            this.campaignBindingNavigator.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.campaignBindingNavigator.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.bindingNavigatorAddNewItem,
            this.toolStripLabel1,
            this.bindingNavigatorDeleteItem,
            this.toolStripLabel2,
            this.campaignBindingNavigatorSaveItem});
            this.campaignBindingNavigator.Location = new System.Drawing.Point(0, 0);
            this.campaignBindingNavigator.MoveFirstItem = null;
            this.campaignBindingNavigator.MoveLastItem = null;
            this.campaignBindingNavigator.MoveNextItem = null;
            this.campaignBindingNavigator.MovePreviousItem = null;
            this.campaignBindingNavigator.Name = "campaignBindingNavigator";
            this.campaignBindingNavigator.PositionItem = null;
            this.campaignBindingNavigator.Size = new System.Drawing.Size(1143, 25);
            this.campaignBindingNavigator.TabIndex = 0;
            this.campaignBindingNavigator.Text = "bindingNavigator1";
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
            // toolStripLabel1
            // 
            this.toolStripLabel1.Name = "toolStripLabel1";
            this.toolStripLabel1.Size = new System.Drawing.Size(37, 22);
            this.toolStripLabel1.Text = "          ";
            // 
            // toolStripLabel2
            // 
            this.toolStripLabel2.Name = "toolStripLabel2";
            this.toolStripLabel2.Size = new System.Drawing.Size(37, 22);
            this.toolStripLabel2.Text = "          ";
            // 
            // campaignBindingNavigatorSaveItem
            // 
            this.campaignBindingNavigatorSaveItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.campaignBindingNavigatorSaveItem.Image = ((System.Drawing.Image)(resources.GetObject("campaignBindingNavigatorSaveItem.Image")));
            this.campaignBindingNavigatorSaveItem.Name = "campaignBindingNavigatorSaveItem";
            this.campaignBindingNavigatorSaveItem.Size = new System.Drawing.Size(23, 22);
            this.campaignBindingNavigatorSaveItem.Text = "Save Data";
            this.campaignBindingNavigatorSaveItem.Click += new System.EventHandler(this.campaignBindingNavigatorSaveItem_Click);
            // 
            // campaignDataGridView
            // 
            this.campaignDataGridView.AutoGenerateColumns = false;
            this.campaignDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.campaignDataGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dataGridViewTextBoxColumn1,
            this.dataGridViewTextBoxColumn2,
            this.dataGridViewTextBoxColumn4,
            this.dataGridViewTextBoxColumn5,
            this.dataGridViewTextBoxColumn6,
            this.dataGridViewTextBoxColumn7,
            this.dataGridViewTextBoxColumn3,
            this.dataGridViewTextBoxColumn11,
            this.dataGridViewTextBoxColumn12,
            this.dataGridViewTextBoxColumn14,
            this.dataGridViewCheckBoxColumn1,
            this.dataGridViewCheckBoxColumn2,
            this.dataGridViewCheckBoxColumn3,
            this.dataGridViewCheckBoxColumn4,
            this.dataGridViewTextBoxColumn15,
            this.dataGridViewTextBoxColumn9,
            this.dataGridViewTextBoxColumn10,
            this.dataGridViewTextBoxColumn16});
            this.campaignDataGridView.DataSource = this.campaignBindingSource;
            this.campaignDataGridView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.campaignDataGridView.Location = new System.Drawing.Point(0, 25);
            this.campaignDataGridView.Name = "campaignDataGridView";
            this.campaignDataGridView.RowHeadersVisible = false;
            this.campaignDataGridView.Size = new System.Drawing.Size(1143, 580);
            this.campaignDataGridView.TabIndex = 1;
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
            this.dataGridViewTextBoxColumn2.DataPropertyName = "account_manager_id";
            this.dataGridViewTextBoxColumn2.DataSource = this.accountManagerBindingSource;
            this.dataGridViewTextBoxColumn2.DisplayMember = "name";
            this.dataGridViewTextBoxColumn2.DisplayStyle = System.Windows.Forms.DataGridViewComboBoxDisplayStyle.ComboBox;
            this.dataGridViewTextBoxColumn2.DisplayStyleForCurrentCellOnly = true;
            this.dataGridViewTextBoxColumn2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.dataGridViewTextBoxColumn2.HeaderText = "Account Manager";
            this.dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
            this.dataGridViewTextBoxColumn2.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridViewTextBoxColumn2.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.dataGridViewTextBoxColumn2.ValueMember = "id";
            // 
            // accountManagerBindingSource
            // 
            this.accountManagerBindingSource.DataMember = "AccountManager";
            this.accountManagerBindingSource.DataSource = this.campaignDataSetBindingSource;
            // 
            // campaignDataSetBindingSource
            // 
            this.campaignDataSetBindingSource.DataSource = this.campaignDataSet;
            this.campaignDataSetBindingSource.Position = 0;
            // 
            // dataGridViewTextBoxColumn4
            // 
            this.dataGridViewTextBoxColumn4.DataPropertyName = "ad_manager_id";
            this.dataGridViewTextBoxColumn4.DataSource = this.adManagerBindingSource;
            this.dataGridViewTextBoxColumn4.DisplayMember = "name";
            this.dataGridViewTextBoxColumn4.DisplayStyle = System.Windows.Forms.DataGridViewComboBoxDisplayStyle.ComboBox;
            this.dataGridViewTextBoxColumn4.DisplayStyleForCurrentCellOnly = true;
            this.dataGridViewTextBoxColumn4.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.dataGridViewTextBoxColumn4.HeaderText = "Ad Manager";
            this.dataGridViewTextBoxColumn4.Name = "dataGridViewTextBoxColumn4";
            this.dataGridViewTextBoxColumn4.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridViewTextBoxColumn4.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.dataGridViewTextBoxColumn4.ValueMember = "id";
            // 
            // adManagerBindingSource
            // 
            this.adManagerBindingSource.DataMember = "AdManager";
            this.adManagerBindingSource.DataSource = this.campaignDataSetBindingSource;
            // 
            // dataGridViewTextBoxColumn5
            // 
            this.dataGridViewTextBoxColumn5.DataPropertyName = "advertiser_id";
            this.dataGridViewTextBoxColumn5.DataSource = this.advertiserBindingSource;
            this.dataGridViewTextBoxColumn5.DisplayMember = "name";
            this.dataGridViewTextBoxColumn5.DisplayStyle = System.Windows.Forms.DataGridViewComboBoxDisplayStyle.ComboBox;
            this.dataGridViewTextBoxColumn5.DisplayStyleForCurrentCellOnly = true;
            this.dataGridViewTextBoxColumn5.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.dataGridViewTextBoxColumn5.HeaderText = "Advertiser";
            this.dataGridViewTextBoxColumn5.Name = "dataGridViewTextBoxColumn5";
            this.dataGridViewTextBoxColumn5.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridViewTextBoxColumn5.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.dataGridViewTextBoxColumn5.ValueMember = "id";
            // 
            // advertiserBindingSource
            // 
            this.advertiserBindingSource.DataMember = "Advertiser";
            this.advertiserBindingSource.DataSource = this.campaignDataSetBindingSource;
            // 
            // dataGridViewTextBoxColumn6
            // 
            this.dataGridViewTextBoxColumn6.DataPropertyName = "pid";
            this.dataGridViewTextBoxColumn6.HeaderText = "PID";
            this.dataGridViewTextBoxColumn6.Name = "dataGridViewTextBoxColumn6";
            // 
            // dataGridViewTextBoxColumn7
            // 
            this.dataGridViewTextBoxColumn7.DataPropertyName = "campaign_name";
            this.dataGridViewTextBoxColumn7.HeaderText = "Name";
            this.dataGridViewTextBoxColumn7.Name = "dataGridViewTextBoxColumn7";
            // 
            // dataGridViewTextBoxColumn3
            // 
            this.dataGridViewTextBoxColumn3.DataPropertyName = "campaign_status_id";
            this.dataGridViewTextBoxColumn3.DataSource = this.campaignStatusBindingSource;
            this.dataGridViewTextBoxColumn3.DisplayMember = "name";
            this.dataGridViewTextBoxColumn3.DisplayStyle = System.Windows.Forms.DataGridViewComboBoxDisplayStyle.ComboBox;
            this.dataGridViewTextBoxColumn3.DisplayStyleForCurrentCellOnly = true;
            this.dataGridViewTextBoxColumn3.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.dataGridViewTextBoxColumn3.HeaderText = "Status";
            this.dataGridViewTextBoxColumn3.Name = "dataGridViewTextBoxColumn3";
            this.dataGridViewTextBoxColumn3.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridViewTextBoxColumn3.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.dataGridViewTextBoxColumn3.ValueMember = "id";
            // 
            // campaignStatusBindingSource
            // 
            this.campaignStatusBindingSource.DataMember = "CampaignStatus";
            this.campaignStatusBindingSource.DataSource = this.campaignDataSetBindingSource;
            // 
            // dataGridViewTextBoxColumn11
            // 
            this.dataGridViewTextBoxColumn11.DataPropertyName = "dt_campaign_status";
            this.dataGridViewTextBoxColumn11.DataSource = this.dTCampaignStatusBindingSource;
            this.dataGridViewTextBoxColumn11.DisplayMember = "name";
            this.dataGridViewTextBoxColumn11.DisplayStyle = System.Windows.Forms.DataGridViewComboBoxDisplayStyle.ComboBox;
            this.dataGridViewTextBoxColumn11.DisplayStyleForCurrentCellOnly = true;
            this.dataGridViewTextBoxColumn11.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.dataGridViewTextBoxColumn11.HeaderText = "DT Status";
            this.dataGridViewTextBoxColumn11.Name = "dataGridViewTextBoxColumn11";
            this.dataGridViewTextBoxColumn11.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridViewTextBoxColumn11.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.dataGridViewTextBoxColumn11.ValueMember = "name";
            // 
            // dTCampaignStatusBindingSource
            // 
            this.dTCampaignStatusBindingSource.DataMember = "DTCampaignStatus";
            this.dTCampaignStatusBindingSource.DataSource = this.campaignDataSetBindingSource;
            // 
            // dataGridViewTextBoxColumn12
            // 
            this.dataGridViewTextBoxColumn12.DataPropertyName = "dt_campaign_url";
            this.dataGridViewTextBoxColumn12.HeaderText = "URL";
            this.dataGridViewTextBoxColumn12.Name = "dataGridViewTextBoxColumn12";
            // 
            // dataGridViewTextBoxColumn14
            // 
            this.dataGridViewTextBoxColumn14.DataPropertyName = "dt_allowed_country_names";
            this.dataGridViewTextBoxColumn14.HeaderText = "Allowed Countries";
            this.dataGridViewTextBoxColumn14.Name = "dataGridViewTextBoxColumn14";
            // 
            // dataGridViewCheckBoxColumn1
            // 
            this.dataGridViewCheckBoxColumn1.DataPropertyName = "is_email";
            this.dataGridViewCheckBoxColumn1.HeaderText = "is Email";
            this.dataGridViewCheckBoxColumn1.Name = "dataGridViewCheckBoxColumn1";
            // 
            // dataGridViewCheckBoxColumn2
            // 
            this.dataGridViewCheckBoxColumn2.DataPropertyName = "is_search";
            this.dataGridViewCheckBoxColumn2.HeaderText = "is Search";
            this.dataGridViewCheckBoxColumn2.Name = "dataGridViewCheckBoxColumn2";
            // 
            // dataGridViewCheckBoxColumn3
            // 
            this.dataGridViewCheckBoxColumn3.DataPropertyName = "is_display";
            this.dataGridViewCheckBoxColumn3.HeaderText = "is Display";
            this.dataGridViewCheckBoxColumn3.Name = "dataGridViewCheckBoxColumn3";
            // 
            // dataGridViewCheckBoxColumn4
            // 
            this.dataGridViewCheckBoxColumn4.DataPropertyName = "is_coreg";
            this.dataGridViewCheckBoxColumn4.HeaderText = "is CoReg";
            this.dataGridViewCheckBoxColumn4.Name = "dataGridViewCheckBoxColumn4";
            // 
            // dataGridViewTextBoxColumn15
            // 
            this.dataGridViewTextBoxColumn15.DataPropertyName = "max_scrub";
            this.dataGridViewTextBoxColumn15.HeaderText = "Max Scrub";
            this.dataGridViewTextBoxColumn15.Name = "dataGridViewTextBoxColumn15";
            // 
            // dataGridViewTextBoxColumn9
            // 
            this.dataGridViewTextBoxColumn9.DataPropertyName = "modified";
            this.dataGridViewTextBoxColumn9.HeaderText = "Modified";
            this.dataGridViewTextBoxColumn9.Name = "dataGridViewTextBoxColumn9";
            // 
            // dataGridViewTextBoxColumn10
            // 
            this.dataGridViewTextBoxColumn10.DataPropertyName = "created";
            this.dataGridViewTextBoxColumn10.HeaderText = "Created";
            this.dataGridViewTextBoxColumn10.Name = "dataGridViewTextBoxColumn10";
            // 
            // dataGridViewTextBoxColumn16
            // 
            this.dataGridViewTextBoxColumn16.DataPropertyName = "notes";
            this.dataGridViewTextBoxColumn16.HeaderText = "Notes";
            this.dataGridViewTextBoxColumn16.Name = "dataGridViewTextBoxColumn16";
            // 
            // dTCampaignTypeBindingSource
            // 
            this.dTCampaignTypeBindingSource.DataMember = "DTCampaignType";
            this.dTCampaignTypeBindingSource.DataSource = this.campaignDataSetBindingSource;
            // 
            // accountManagerTableAdapter
            // 
            this.accountManagerTableAdapter.ClearBeforeFill = true;
            // 
            // campaignStatusTableAdapter
            // 
            this.campaignStatusTableAdapter.ClearBeforeFill = true;
            // 
            // adManagerTableAdapter
            // 
            this.adManagerTableAdapter.ClearBeforeFill = true;
            // 
            // advertiserTableAdapter
            // 
            this.advertiserTableAdapter.ClearBeforeFill = true;
            // 
            // dTCampaignStatusTableAdapter
            // 
            this.dTCampaignStatusTableAdapter.ClearBeforeFill = true;
            // 
            // dTCampaignTypeTableAdapter
            // 
            this.dTCampaignTypeTableAdapter.ClearBeforeFill = true;
            // 
            // CampaignsUC
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.campaignDataGridView);
            this.Controls.Add(this.campaignBindingNavigator);
            this.Name = "CampaignsUC";
            this.Size = new System.Drawing.Size(1143, 605);
            this.Load += new System.EventHandler(this.CampaignsUC_Load);
            ((System.ComponentModel.ISupportInitialize)(this.campaignDataSet)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.campaignBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.campaignBindingNavigator)).EndInit();
            this.campaignBindingNavigator.ResumeLayout(false);
            this.campaignBindingNavigator.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.campaignDataGridView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.accountManagerBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.campaignDataSetBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.adManagerBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.advertiserBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.campaignStatusBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dTCampaignStatusBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dTCampaignTypeBindingSource)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private CampaignDataSet campaignDataSet;
        private System.Windows.Forms.BindingSource campaignBindingSource;
        private CampaignDataSetTableAdapters.CampaignTableAdapter campaignTableAdapter;
        private CampaignDataSetTableAdapters.TableAdapterManager tableAdapterManager;
        private System.Windows.Forms.BindingNavigator campaignBindingNavigator;
        private System.Windows.Forms.ToolStripButton bindingNavigatorAddNewItem;
        private System.Windows.Forms.ToolStripButton bindingNavigatorDeleteItem;
        private System.Windows.Forms.ToolStripButton campaignBindingNavigatorSaveItem;
        private System.Windows.Forms.DataGridView campaignDataGridView;
        private System.Windows.Forms.BindingSource accountManagerBindingSource;
        private System.Windows.Forms.BindingSource campaignDataSetBindingSource;
        private System.Windows.Forms.BindingSource campaignStatusBindingSource;
        private CampaignDataSetTableAdapters.AccountManagerTableAdapter accountManagerTableAdapter;
        private CampaignDataSetTableAdapters.CampaignStatusTableAdapter campaignStatusTableAdapter;
        private System.Windows.Forms.BindingSource adManagerBindingSource;
        private System.Windows.Forms.BindingSource advertiserBindingSource;
        private CampaignDataSetTableAdapters.AdManagerTableAdapter adManagerTableAdapter;
        private CampaignDataSetTableAdapters.AdvertiserTableAdapter advertiserTableAdapter;
        private System.Windows.Forms.BindingSource dTCampaignStatusBindingSource;
        private CampaignDataSetTableAdapters.DTCampaignStatusTableAdapter dTCampaignStatusTableAdapter;
        private System.Windows.Forms.BindingSource dTCampaignTypeBindingSource;
        private CampaignDataSetTableAdapters.DTCampaignTypeTableAdapter dTCampaignTypeTableAdapter;
        private System.Windows.Forms.ToolStripLabel toolStripLabel1;
        private System.Windows.Forms.ToolStripLabel toolStripLabel2;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewComboBoxColumn dataGridViewTextBoxColumn2;
        private System.Windows.Forms.DataGridViewComboBoxColumn dataGridViewTextBoxColumn4;
        private System.Windows.Forms.DataGridViewComboBoxColumn dataGridViewTextBoxColumn5;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn6;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn7;
        private System.Windows.Forms.DataGridViewComboBoxColumn dataGridViewTextBoxColumn3;
        private System.Windows.Forms.DataGridViewComboBoxColumn dataGridViewTextBoxColumn11;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn12;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn14;
        private System.Windows.Forms.DataGridViewCheckBoxColumn dataGridViewCheckBoxColumn1;
        private System.Windows.Forms.DataGridViewCheckBoxColumn dataGridViewCheckBoxColumn2;
        private System.Windows.Forms.DataGridViewCheckBoxColumn dataGridViewCheckBoxColumn3;
        private System.Windows.Forms.DataGridViewCheckBoxColumn dataGridViewCheckBoxColumn4;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn15;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn9;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn10;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn16;

    }
}
