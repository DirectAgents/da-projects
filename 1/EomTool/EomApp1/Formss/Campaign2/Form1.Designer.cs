namespace EomApp1.Formss.Campaign2
{
    partial class Form1
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
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.dADatabaseApril2012_before_cakeDataSet = new EomApp1.DADatabaseApril2012_before_cakeDataSet();
            this.campaignBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.campaignTableAdapter = new EomApp1.DADatabaseApril2012_before_cakeDataSetTableAdapters.CampaignTableAdapter();
            this.tableAdapterManager = new EomApp1.DADatabaseApril2012_before_cakeDataSetTableAdapters.TableAdapterManager();
            this.accountManagerBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.accountManagerTableAdapter = new EomApp1.DADatabaseApril2012_before_cakeDataSetTableAdapters.AccountManagerTableAdapter();
            this.accountManagerDataGridView = new System.Windows.Forms.DataGridView();
            this.dataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.idDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.accountmanageridDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.campaignstatusidDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.admanageridDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.advertiseridDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.pidDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.campaignnameDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.campaigntypeDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.modifiedDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.createdDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dtcampaignstatusDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dtcampaignurlDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dtallowedcountrynamesDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.isemailDataGridViewCheckBoxColumn = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.issearchDataGridViewCheckBoxColumn = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.isdisplayDataGridViewCheckBoxColumn = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.iscoregDataGridViewCheckBoxColumn = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.maxscrubDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.notesDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dADatabaseApril2012_before_cakeDataSet)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.campaignBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.accountManagerBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.accountManagerDataGridView)).BeginInit();
            this.SuspendLayout();
            // 
            // dataGridView1
            // 
            this.dataGridView1.AutoGenerateColumns = false;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.idDataGridViewTextBoxColumn,
            this.accountmanageridDataGridViewTextBoxColumn,
            this.campaignstatusidDataGridViewTextBoxColumn,
            this.admanageridDataGridViewTextBoxColumn,
            this.advertiseridDataGridViewTextBoxColumn,
            this.pidDataGridViewTextBoxColumn,
            this.campaignnameDataGridViewTextBoxColumn,
            this.campaigntypeDataGridViewTextBoxColumn,
            this.modifiedDataGridViewTextBoxColumn,
            this.createdDataGridViewTextBoxColumn,
            this.dtcampaignstatusDataGridViewTextBoxColumn,
            this.dtcampaignurlDataGridViewTextBoxColumn,
            this.dtallowedcountrynamesDataGridViewTextBoxColumn,
            this.isemailDataGridViewCheckBoxColumn,
            this.issearchDataGridViewCheckBoxColumn,
            this.isdisplayDataGridViewCheckBoxColumn,
            this.iscoregDataGridViewCheckBoxColumn,
            this.maxscrubDataGridViewTextBoxColumn,
            this.notesDataGridViewTextBoxColumn});
            this.dataGridView1.DataSource = this.campaignBindingSource;
            this.dataGridView1.Location = new System.Drawing.Point(12, 3);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.Size = new System.Drawing.Size(1253, 232);
            this.dataGridView1.TabIndex = 0;
            // 
            // dADatabaseApril2012_before_cakeDataSet
            // 
            this.dADatabaseApril2012_before_cakeDataSet.DataSetName = "DADatabaseApril2012_before_cakeDataSet";
            this.dADatabaseApril2012_before_cakeDataSet.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // campaignBindingSource
            // 
            this.campaignBindingSource.DataMember = "Campaign";
            this.campaignBindingSource.DataSource = this.dADatabaseApril2012_before_cakeDataSet;
            // 
            // campaignTableAdapter
            // 
            this.campaignTableAdapter.ClearBeforeFill = true;
            // 
            // tableAdapterManager
            // 
            this.tableAdapterManager.AccountManagerTableAdapter = this.accountManagerTableAdapter;
            this.tableAdapterManager.BackupDataSetBeforeUpdate = false;
            this.tableAdapterManager.CampaignTableAdapter = this.campaignTableAdapter;
            this.tableAdapterManager.UpdateOrder = EomApp1.DADatabaseApril2012_before_cakeDataSetTableAdapters.TableAdapterManager.UpdateOrderOption.InsertUpdateDelete;
            // 
            // accountManagerBindingSource
            // 
            this.accountManagerBindingSource.DataMember = "AccountManager";
            this.accountManagerBindingSource.DataSource = this.dADatabaseApril2012_before_cakeDataSet;
            // 
            // accountManagerTableAdapter
            // 
            this.accountManagerTableAdapter.ClearBeforeFill = true;
            // 
            // accountManagerDataGridView
            // 
            this.accountManagerDataGridView.AutoGenerateColumns = false;
            this.accountManagerDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.accountManagerDataGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dataGridViewTextBoxColumn1,
            this.dataGridViewTextBoxColumn2});
            this.accountManagerDataGridView.DataSource = this.accountManagerBindingSource;
            this.accountManagerDataGridView.Location = new System.Drawing.Point(47, 288);
            this.accountManagerDataGridView.Name = "accountManagerDataGridView";
            this.accountManagerDataGridView.Size = new System.Drawing.Size(300, 220);
            this.accountManagerDataGridView.TabIndex = 1;
            // 
            // dataGridViewTextBoxColumn1
            // 
            this.dataGridViewTextBoxColumn1.DataPropertyName = "id";
            this.dataGridViewTextBoxColumn1.HeaderText = "id";
            this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
            this.dataGridViewTextBoxColumn1.ReadOnly = true;
            // 
            // dataGridViewTextBoxColumn2
            // 
            this.dataGridViewTextBoxColumn2.DataPropertyName = "name";
            this.dataGridViewTextBoxColumn2.HeaderText = "name";
            this.dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
            // 
            // idDataGridViewTextBoxColumn
            // 
            this.idDataGridViewTextBoxColumn.DataPropertyName = "id";
            this.idDataGridViewTextBoxColumn.HeaderText = "id";
            this.idDataGridViewTextBoxColumn.Name = "idDataGridViewTextBoxColumn";
            this.idDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // accountmanageridDataGridViewTextBoxColumn
            // 
            this.accountmanageridDataGridViewTextBoxColumn.DataPropertyName = "account_manager_id";
            this.accountmanageridDataGridViewTextBoxColumn.DataSource = this.accountManagerBindingSource;
            this.accountmanageridDataGridViewTextBoxColumn.DisplayMember = "name";
            this.accountmanageridDataGridViewTextBoxColumn.HeaderText = "account_manager_id";
            this.accountmanageridDataGridViewTextBoxColumn.Name = "accountmanageridDataGridViewTextBoxColumn";
            this.accountmanageridDataGridViewTextBoxColumn.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.accountmanageridDataGridViewTextBoxColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.accountmanageridDataGridViewTextBoxColumn.ValueMember = "id";
            // 
            // campaignstatusidDataGridViewTextBoxColumn
            // 
            this.campaignstatusidDataGridViewTextBoxColumn.DataPropertyName = "campaign_status_id";
            this.campaignstatusidDataGridViewTextBoxColumn.HeaderText = "campaign_status_id";
            this.campaignstatusidDataGridViewTextBoxColumn.Name = "campaignstatusidDataGridViewTextBoxColumn";
            // 
            // admanageridDataGridViewTextBoxColumn
            // 
            this.admanageridDataGridViewTextBoxColumn.DataPropertyName = "ad_manager_id";
            this.admanageridDataGridViewTextBoxColumn.HeaderText = "ad_manager_id";
            this.admanageridDataGridViewTextBoxColumn.Name = "admanageridDataGridViewTextBoxColumn";
            // 
            // advertiseridDataGridViewTextBoxColumn
            // 
            this.advertiseridDataGridViewTextBoxColumn.DataPropertyName = "advertiser_id";
            this.advertiseridDataGridViewTextBoxColumn.HeaderText = "advertiser_id";
            this.advertiseridDataGridViewTextBoxColumn.Name = "advertiseridDataGridViewTextBoxColumn";
            // 
            // pidDataGridViewTextBoxColumn
            // 
            this.pidDataGridViewTextBoxColumn.DataPropertyName = "pid";
            this.pidDataGridViewTextBoxColumn.HeaderText = "pid";
            this.pidDataGridViewTextBoxColumn.Name = "pidDataGridViewTextBoxColumn";
            // 
            // campaignnameDataGridViewTextBoxColumn
            // 
            this.campaignnameDataGridViewTextBoxColumn.DataPropertyName = "campaign_name";
            this.campaignnameDataGridViewTextBoxColumn.HeaderText = "campaign_name";
            this.campaignnameDataGridViewTextBoxColumn.Name = "campaignnameDataGridViewTextBoxColumn";
            // 
            // campaigntypeDataGridViewTextBoxColumn
            // 
            this.campaigntypeDataGridViewTextBoxColumn.DataPropertyName = "campaign_type";
            this.campaigntypeDataGridViewTextBoxColumn.HeaderText = "campaign_type";
            this.campaigntypeDataGridViewTextBoxColumn.Name = "campaigntypeDataGridViewTextBoxColumn";
            // 
            // modifiedDataGridViewTextBoxColumn
            // 
            this.modifiedDataGridViewTextBoxColumn.DataPropertyName = "modified";
            this.modifiedDataGridViewTextBoxColumn.HeaderText = "modified";
            this.modifiedDataGridViewTextBoxColumn.Name = "modifiedDataGridViewTextBoxColumn";
            this.modifiedDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // createdDataGridViewTextBoxColumn
            // 
            this.createdDataGridViewTextBoxColumn.DataPropertyName = "created";
            this.createdDataGridViewTextBoxColumn.HeaderText = "created";
            this.createdDataGridViewTextBoxColumn.Name = "createdDataGridViewTextBoxColumn";
            // 
            // dtcampaignstatusDataGridViewTextBoxColumn
            // 
            this.dtcampaignstatusDataGridViewTextBoxColumn.DataPropertyName = "dt_campaign_status";
            this.dtcampaignstatusDataGridViewTextBoxColumn.HeaderText = "dt_campaign_status";
            this.dtcampaignstatusDataGridViewTextBoxColumn.Name = "dtcampaignstatusDataGridViewTextBoxColumn";
            // 
            // dtcampaignurlDataGridViewTextBoxColumn
            // 
            this.dtcampaignurlDataGridViewTextBoxColumn.DataPropertyName = "dt_campaign_url";
            this.dtcampaignurlDataGridViewTextBoxColumn.HeaderText = "dt_campaign_url";
            this.dtcampaignurlDataGridViewTextBoxColumn.Name = "dtcampaignurlDataGridViewTextBoxColumn";
            // 
            // dtallowedcountrynamesDataGridViewTextBoxColumn
            // 
            this.dtallowedcountrynamesDataGridViewTextBoxColumn.DataPropertyName = "dt_allowed_country_names";
            this.dtallowedcountrynamesDataGridViewTextBoxColumn.HeaderText = "dt_allowed_country_names";
            this.dtallowedcountrynamesDataGridViewTextBoxColumn.Name = "dtallowedcountrynamesDataGridViewTextBoxColumn";
            // 
            // isemailDataGridViewCheckBoxColumn
            // 
            this.isemailDataGridViewCheckBoxColumn.DataPropertyName = "is_email";
            this.isemailDataGridViewCheckBoxColumn.HeaderText = "is_email";
            this.isemailDataGridViewCheckBoxColumn.Name = "isemailDataGridViewCheckBoxColumn";
            // 
            // issearchDataGridViewCheckBoxColumn
            // 
            this.issearchDataGridViewCheckBoxColumn.DataPropertyName = "is_search";
            this.issearchDataGridViewCheckBoxColumn.HeaderText = "is_search";
            this.issearchDataGridViewCheckBoxColumn.Name = "issearchDataGridViewCheckBoxColumn";
            // 
            // isdisplayDataGridViewCheckBoxColumn
            // 
            this.isdisplayDataGridViewCheckBoxColumn.DataPropertyName = "is_display";
            this.isdisplayDataGridViewCheckBoxColumn.HeaderText = "is_display";
            this.isdisplayDataGridViewCheckBoxColumn.Name = "isdisplayDataGridViewCheckBoxColumn";
            // 
            // iscoregDataGridViewCheckBoxColumn
            // 
            this.iscoregDataGridViewCheckBoxColumn.DataPropertyName = "is_coreg";
            this.iscoregDataGridViewCheckBoxColumn.HeaderText = "is_coreg";
            this.iscoregDataGridViewCheckBoxColumn.Name = "iscoregDataGridViewCheckBoxColumn";
            // 
            // maxscrubDataGridViewTextBoxColumn
            // 
            this.maxscrubDataGridViewTextBoxColumn.DataPropertyName = "max_scrub";
            this.maxscrubDataGridViewTextBoxColumn.HeaderText = "max_scrub";
            this.maxscrubDataGridViewTextBoxColumn.Name = "maxscrubDataGridViewTextBoxColumn";
            // 
            // notesDataGridViewTextBoxColumn
            // 
            this.notesDataGridViewTextBoxColumn.DataPropertyName = "notes";
            this.notesDataGridViewTextBoxColumn.HeaderText = "notes";
            this.notesDataGridViewTextBoxColumn.Name = "notesDataGridViewTextBoxColumn";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1285, 583);
            this.Controls.Add(this.accountManagerDataGridView);
            this.Controls.Add(this.dataGridView1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dADatabaseApril2012_before_cakeDataSet)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.campaignBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.accountManagerBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.accountManagerDataGridView)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dataGridView1;
        private DADatabaseApril2012_before_cakeDataSet dADatabaseApril2012_before_cakeDataSet;
        private System.Windows.Forms.BindingSource campaignBindingSource;
        private DADatabaseApril2012_before_cakeDataSetTableAdapters.CampaignTableAdapter campaignTableAdapter;
        private DADatabaseApril2012_before_cakeDataSetTableAdapters.TableAdapterManager tableAdapterManager;
        private DADatabaseApril2012_before_cakeDataSetTableAdapters.AccountManagerTableAdapter accountManagerTableAdapter;
        private System.Windows.Forms.BindingSource accountManagerBindingSource;
        private System.Windows.Forms.DataGridViewTextBoxColumn idDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewComboBoxColumn accountmanageridDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn campaignstatusidDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn admanageridDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn advertiseridDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn pidDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn campaignnameDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn campaigntypeDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn modifiedDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn createdDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn dtcampaignstatusDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn dtcampaignurlDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn dtallowedcountrynamesDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewCheckBoxColumn isemailDataGridViewCheckBoxColumn;
        private System.Windows.Forms.DataGridViewCheckBoxColumn issearchDataGridViewCheckBoxColumn;
        private System.Windows.Forms.DataGridViewCheckBoxColumn isdisplayDataGridViewCheckBoxColumn;
        private System.Windows.Forms.DataGridViewCheckBoxColumn iscoregDataGridViewCheckBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn maxscrubDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn notesDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridView accountManagerDataGridView;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
    }
}