namespace EomApp1.Screens.MediaBuyerWorkflow
{
    partial class PublisherPayoutsSubform
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
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.affidDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.publisherDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.advertiserDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.pidDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.campaignNameDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.revCurrencyDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.costCurrencyDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.revUnitDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.revUnitUSDDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.costUnitDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.costUnitUSDDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.unitsDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.unitTypeDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.revenueDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.revenueUSDDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.costDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.costUSDDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.marginDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.marginPctDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.mediaBuyerDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.adManagerDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.accountManagerDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.statusDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.accountingStatusDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.mediaBuyerApprovalStatusDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.netTermsDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.affPayMethodDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.pubPayCurrDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.pubPayoutDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.sourceDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.itemIdsDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.publisherPayoutsBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.mediaBuyerWorkflowDataSet = new EomApp1.Screens.MediaBuyerWorkflow.MediaBuyerWorkflowDataSet();
            this.panel1 = new System.Windows.Forms.Panel();
            this.publisherPayoutsTableAdapter = new EomApp1.Screens.MediaBuyerWorkflow.MediaBuyerWorkflowDataSetTableAdapters.PublisherPayoutsTableAdapter();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.publisherPayoutsBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.mediaBuyerWorkflowDataSet)).BeginInit();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.AllowUserToResizeRows = false;
            this.dataGridView1.AutoGenerateColumns = false;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.affidDataGridViewTextBoxColumn,
            this.publisherDataGridViewTextBoxColumn,
            this.advertiserDataGridViewTextBoxColumn,
            this.pidDataGridViewTextBoxColumn,
            this.campaignNameDataGridViewTextBoxColumn,
            this.revCurrencyDataGridViewTextBoxColumn,
            this.costCurrencyDataGridViewTextBoxColumn,
            this.revUnitDataGridViewTextBoxColumn,
            this.revUnitUSDDataGridViewTextBoxColumn,
            this.costUnitDataGridViewTextBoxColumn,
            this.costUnitUSDDataGridViewTextBoxColumn,
            this.unitsDataGridViewTextBoxColumn,
            this.unitTypeDataGridViewTextBoxColumn,
            this.revenueDataGridViewTextBoxColumn,
            this.revenueUSDDataGridViewTextBoxColumn,
            this.costDataGridViewTextBoxColumn,
            this.costUSDDataGridViewTextBoxColumn,
            this.marginDataGridViewTextBoxColumn,
            this.marginPctDataGridViewTextBoxColumn,
            this.mediaBuyerDataGridViewTextBoxColumn,
            this.adManagerDataGridViewTextBoxColumn,
            this.accountManagerDataGridViewTextBoxColumn,
            this.statusDataGridViewTextBoxColumn,
            this.accountingStatusDataGridViewTextBoxColumn,
            this.mediaBuyerApprovalStatusDataGridViewTextBoxColumn,
            this.netTermsDataGridViewTextBoxColumn,
            this.affPayMethodDataGridViewTextBoxColumn,
            this.pubPayCurrDataGridViewTextBoxColumn,
            this.pubPayoutDataGridViewTextBoxColumn,
            this.sourceDataGridViewTextBoxColumn,
            this.itemIdsDataGridViewTextBoxColumn});
            this.dataGridView1.DataSource = this.publisherPayoutsBindingSource;
            this.dataGridView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView1.Location = new System.Drawing.Point(0, 0);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.dataGridView1.Size = new System.Drawing.Size(5000, 300);
            this.dataGridView1.TabIndex = 0;
            // 
            // affidDataGridViewTextBoxColumn
            // 
            this.affidDataGridViewTextBoxColumn.DataPropertyName = "affid";
            this.affidDataGridViewTextBoxColumn.HeaderText = "affid";
            this.affidDataGridViewTextBoxColumn.Name = "affidDataGridViewTextBoxColumn";
            // 
            // publisherDataGridViewTextBoxColumn
            // 
            this.publisherDataGridViewTextBoxColumn.DataPropertyName = "Publisher";
            this.publisherDataGridViewTextBoxColumn.HeaderText = "Publisher";
            this.publisherDataGridViewTextBoxColumn.Name = "publisherDataGridViewTextBoxColumn";
            this.publisherDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // advertiserDataGridViewTextBoxColumn
            // 
            this.advertiserDataGridViewTextBoxColumn.DataPropertyName = "Advertiser";
            this.advertiserDataGridViewTextBoxColumn.HeaderText = "Advertiser";
            this.advertiserDataGridViewTextBoxColumn.Name = "advertiserDataGridViewTextBoxColumn";
            // 
            // pidDataGridViewTextBoxColumn
            // 
            this.pidDataGridViewTextBoxColumn.DataPropertyName = "pid";
            this.pidDataGridViewTextBoxColumn.HeaderText = "pid";
            this.pidDataGridViewTextBoxColumn.Name = "pidDataGridViewTextBoxColumn";
            // 
            // campaignNameDataGridViewTextBoxColumn
            // 
            this.campaignNameDataGridViewTextBoxColumn.DataPropertyName = "Campaign Name";
            this.campaignNameDataGridViewTextBoxColumn.HeaderText = "Campaign Name";
            this.campaignNameDataGridViewTextBoxColumn.Name = "campaignNameDataGridViewTextBoxColumn";
            // 
            // revCurrencyDataGridViewTextBoxColumn
            // 
            this.revCurrencyDataGridViewTextBoxColumn.DataPropertyName = "Rev Currency";
            this.revCurrencyDataGridViewTextBoxColumn.HeaderText = "Rev Currency";
            this.revCurrencyDataGridViewTextBoxColumn.Name = "revCurrencyDataGridViewTextBoxColumn";
            // 
            // costCurrencyDataGridViewTextBoxColumn
            // 
            this.costCurrencyDataGridViewTextBoxColumn.DataPropertyName = "Cost Currency";
            this.costCurrencyDataGridViewTextBoxColumn.HeaderText = "Cost Currency";
            this.costCurrencyDataGridViewTextBoxColumn.Name = "costCurrencyDataGridViewTextBoxColumn";
            // 
            // revUnitDataGridViewTextBoxColumn
            // 
            this.revUnitDataGridViewTextBoxColumn.DataPropertyName = "Rev/Unit";
            this.revUnitDataGridViewTextBoxColumn.HeaderText = "Rev/Unit";
            this.revUnitDataGridViewTextBoxColumn.Name = "revUnitDataGridViewTextBoxColumn";
            // 
            // revUnitUSDDataGridViewTextBoxColumn
            // 
            this.revUnitUSDDataGridViewTextBoxColumn.DataPropertyName = "Rev/Unit USD";
            this.revUnitUSDDataGridViewTextBoxColumn.HeaderText = "Rev/Unit USD";
            this.revUnitUSDDataGridViewTextBoxColumn.Name = "revUnitUSDDataGridViewTextBoxColumn";
            // 
            // costUnitDataGridViewTextBoxColumn
            // 
            this.costUnitDataGridViewTextBoxColumn.DataPropertyName = "Cost/Unit";
            this.costUnitDataGridViewTextBoxColumn.HeaderText = "Cost/Unit";
            this.costUnitDataGridViewTextBoxColumn.Name = "costUnitDataGridViewTextBoxColumn";
            // 
            // costUnitUSDDataGridViewTextBoxColumn
            // 
            this.costUnitUSDDataGridViewTextBoxColumn.DataPropertyName = "Cost/Unit USD";
            this.costUnitUSDDataGridViewTextBoxColumn.HeaderText = "Cost/Unit USD";
            this.costUnitUSDDataGridViewTextBoxColumn.Name = "costUnitUSDDataGridViewTextBoxColumn";
            // 
            // unitsDataGridViewTextBoxColumn
            // 
            this.unitsDataGridViewTextBoxColumn.DataPropertyName = "Units";
            this.unitsDataGridViewTextBoxColumn.HeaderText = "Units";
            this.unitsDataGridViewTextBoxColumn.Name = "unitsDataGridViewTextBoxColumn";
            // 
            // unitTypeDataGridViewTextBoxColumn
            // 
            this.unitTypeDataGridViewTextBoxColumn.DataPropertyName = "Unit Type";
            this.unitTypeDataGridViewTextBoxColumn.HeaderText = "Unit Type";
            this.unitTypeDataGridViewTextBoxColumn.Name = "unitTypeDataGridViewTextBoxColumn";
            // 
            // revenueDataGridViewTextBoxColumn
            // 
            this.revenueDataGridViewTextBoxColumn.DataPropertyName = "Revenue";
            this.revenueDataGridViewTextBoxColumn.HeaderText = "Revenue";
            this.revenueDataGridViewTextBoxColumn.Name = "revenueDataGridViewTextBoxColumn";
            // 
            // revenueUSDDataGridViewTextBoxColumn
            // 
            this.revenueUSDDataGridViewTextBoxColumn.DataPropertyName = "Revenue USD";
            this.revenueUSDDataGridViewTextBoxColumn.HeaderText = "Revenue USD";
            this.revenueUSDDataGridViewTextBoxColumn.Name = "revenueUSDDataGridViewTextBoxColumn";
            // 
            // costDataGridViewTextBoxColumn
            // 
            this.costDataGridViewTextBoxColumn.DataPropertyName = "Cost";
            this.costDataGridViewTextBoxColumn.HeaderText = "Cost";
            this.costDataGridViewTextBoxColumn.Name = "costDataGridViewTextBoxColumn";
            // 
            // costUSDDataGridViewTextBoxColumn
            // 
            this.costUSDDataGridViewTextBoxColumn.DataPropertyName = "Cost USD";
            this.costUSDDataGridViewTextBoxColumn.HeaderText = "Cost USD";
            this.costUSDDataGridViewTextBoxColumn.Name = "costUSDDataGridViewTextBoxColumn";
            // 
            // marginDataGridViewTextBoxColumn
            // 
            this.marginDataGridViewTextBoxColumn.DataPropertyName = "Margin";
            this.marginDataGridViewTextBoxColumn.HeaderText = "Margin";
            this.marginDataGridViewTextBoxColumn.Name = "marginDataGridViewTextBoxColumn";
            this.marginDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // marginPctDataGridViewTextBoxColumn
            // 
            this.marginPctDataGridViewTextBoxColumn.DataPropertyName = "MarginPct";
            this.marginPctDataGridViewTextBoxColumn.HeaderText = "MarginPct";
            this.marginPctDataGridViewTextBoxColumn.Name = "marginPctDataGridViewTextBoxColumn";
            this.marginPctDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // mediaBuyerDataGridViewTextBoxColumn
            // 
            this.mediaBuyerDataGridViewTextBoxColumn.DataPropertyName = "Media Buyer";
            this.mediaBuyerDataGridViewTextBoxColumn.HeaderText = "Media Buyer";
            this.mediaBuyerDataGridViewTextBoxColumn.Name = "mediaBuyerDataGridViewTextBoxColumn";
            // 
            // adManagerDataGridViewTextBoxColumn
            // 
            this.adManagerDataGridViewTextBoxColumn.DataPropertyName = "Ad Manager";
            this.adManagerDataGridViewTextBoxColumn.HeaderText = "Ad Manager";
            this.adManagerDataGridViewTextBoxColumn.Name = "adManagerDataGridViewTextBoxColumn";
            // 
            // accountManagerDataGridViewTextBoxColumn
            // 
            this.accountManagerDataGridViewTextBoxColumn.DataPropertyName = "Account Manager";
            this.accountManagerDataGridViewTextBoxColumn.HeaderText = "Account Manager";
            this.accountManagerDataGridViewTextBoxColumn.Name = "accountManagerDataGridViewTextBoxColumn";
            // 
            // statusDataGridViewTextBoxColumn
            // 
            this.statusDataGridViewTextBoxColumn.DataPropertyName = "Status";
            this.statusDataGridViewTextBoxColumn.HeaderText = "Status";
            this.statusDataGridViewTextBoxColumn.Name = "statusDataGridViewTextBoxColumn";
            // 
            // accountingStatusDataGridViewTextBoxColumn
            // 
            this.accountingStatusDataGridViewTextBoxColumn.DataPropertyName = "Accounting Status";
            this.accountingStatusDataGridViewTextBoxColumn.HeaderText = "Accounting Status";
            this.accountingStatusDataGridViewTextBoxColumn.Name = "accountingStatusDataGridViewTextBoxColumn";
            // 
            // mediaBuyerApprovalStatusDataGridViewTextBoxColumn
            // 
            this.mediaBuyerApprovalStatusDataGridViewTextBoxColumn.DataPropertyName = "Media Buyer Approval Status";
            this.mediaBuyerApprovalStatusDataGridViewTextBoxColumn.HeaderText = "Media Buyer Approval Status";
            this.mediaBuyerApprovalStatusDataGridViewTextBoxColumn.Name = "mediaBuyerApprovalStatusDataGridViewTextBoxColumn";
            // 
            // netTermsDataGridViewTextBoxColumn
            // 
            this.netTermsDataGridViewTextBoxColumn.DataPropertyName = "Net Terms";
            this.netTermsDataGridViewTextBoxColumn.HeaderText = "Net Terms";
            this.netTermsDataGridViewTextBoxColumn.Name = "netTermsDataGridViewTextBoxColumn";
            // 
            // affPayMethodDataGridViewTextBoxColumn
            // 
            this.affPayMethodDataGridViewTextBoxColumn.DataPropertyName = "Aff Pay Method";
            this.affPayMethodDataGridViewTextBoxColumn.HeaderText = "Aff Pay Method";
            this.affPayMethodDataGridViewTextBoxColumn.Name = "affPayMethodDataGridViewTextBoxColumn";
            // 
            // pubPayCurrDataGridViewTextBoxColumn
            // 
            this.pubPayCurrDataGridViewTextBoxColumn.DataPropertyName = "Pub Pay Curr";
            this.pubPayCurrDataGridViewTextBoxColumn.HeaderText = "Pub Pay Curr";
            this.pubPayCurrDataGridViewTextBoxColumn.Name = "pubPayCurrDataGridViewTextBoxColumn";
            // 
            // pubPayoutDataGridViewTextBoxColumn
            // 
            this.pubPayoutDataGridViewTextBoxColumn.DataPropertyName = "Pub Payout";
            this.pubPayoutDataGridViewTextBoxColumn.HeaderText = "Pub Payout";
            this.pubPayoutDataGridViewTextBoxColumn.Name = "pubPayoutDataGridViewTextBoxColumn";
            this.pubPayoutDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // sourceDataGridViewTextBoxColumn
            // 
            this.sourceDataGridViewTextBoxColumn.DataPropertyName = "Source";
            this.sourceDataGridViewTextBoxColumn.HeaderText = "Source";
            this.sourceDataGridViewTextBoxColumn.Name = "sourceDataGridViewTextBoxColumn";
            // 
            // itemIdsDataGridViewTextBoxColumn
            // 
            this.itemIdsDataGridViewTextBoxColumn.DataPropertyName = "ItemIds";
            this.itemIdsDataGridViewTextBoxColumn.HeaderText = "ItemIds";
            this.itemIdsDataGridViewTextBoxColumn.Name = "itemIdsDataGridViewTextBoxColumn";
            // 
            // publisherPayoutsBindingSource
            // 
            this.publisherPayoutsBindingSource.DataMember = "PublisherPayouts";
            this.publisherPayoutsBindingSource.DataSource = this.mediaBuyerWorkflowDataSet;
            // 
            // mediaBuyerWorkflowDataSet
            // 
            this.mediaBuyerWorkflowDataSet.DataSetName = "MediaBuyerWorkflowDataSet";
            this.mediaBuyerWorkflowDataSet.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // panel1
            // 
            this.panel1.AutoScroll = true;
            this.panel1.Controls.Add(this.dataGridView1);
            this.panel1.Location = new System.Drawing.Point(3, 3);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(5000, 300);
            this.panel1.TabIndex = 1;
            // 
            // publisherPayoutsTableAdapter
            // 
            this.publisherPayoutsTableAdapter.ClearBeforeFill = true;
            // 
            // PublisherPayoutsSubform
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.Controls.Add(this.panel1);
            this.Name = "PublisherPayoutsSubform";
            this.Size = new System.Drawing.Size(949, 286);
            this.Load += new System.EventHandler(this.PublisherPayoutsSubform_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.publisherPayoutsBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.mediaBuyerWorkflowDataSet)).EndInit();
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.DataGridViewTextBoxColumn affidDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn publisherDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn advertiserDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn pidDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn campaignNameDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn revCurrencyDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn costCurrencyDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn revUnitDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn revUnitUSDDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn costUnitDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn costUnitUSDDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn unitsDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn unitTypeDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn revenueDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn revenueUSDDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn costDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn costUSDDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn marginDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn marginPctDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn mediaBuyerDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn adManagerDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn accountManagerDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn statusDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn accountingStatusDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn mediaBuyerApprovalStatusDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn netTermsDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn affPayMethodDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn pubPayCurrDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn pubPayoutDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn sourceDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn itemIdsDataGridViewTextBoxColumn;
        private System.Windows.Forms.BindingSource publisherPayoutsBindingSource;
        private MediaBuyerWorkflowDataSet mediaBuyerWorkflowDataSet;
        private MediaBuyerWorkflowDataSetTableAdapters.PublisherPayoutsTableAdapter publisherPayoutsTableAdapter;
        private System.Windows.Forms.Panel panel1;
    }
}
