namespace EomApp1.Screens.Campaign
{
    partial class CampaignsRevenueForm
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
            this.dataGridView1 = new EomAppControls.ExtendedDataGridView();
            this.idDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.pidDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.advertiserDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.campaignnameDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.statusDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.AM = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.noteDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.currDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.revenueDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.unfinalizedDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.finalizedDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.verifiedDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.campaignsRevenueBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.campaignsRevenueDataSet = new EomApp1.Screens.Campaign.CampaignsRevenueDataSet();
            this.campaignWorkflowTableAdapter = new EomApp1.Screens.Campaign.CampaignsRevenueDataSetTableAdapters.CampaignWorkflowTableAdapter();
            this.BottomToolStripPanel = new System.Windows.Forms.ToolStripPanel();
            this.TopToolStripPanel = new System.Windows.Forms.ToolStripPanel();
            this.RightToolStripPanel = new System.Windows.Forms.ToolStripPanel();
            this.LeftToolStripPanel = new System.Windows.Forms.ToolStripPanel();
            this.ContentPanel = new System.Windows.Forms.ToolStripContentPanel();
            this.buttonRefresh = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.campaignsRevenueBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.campaignsRevenueDataSet)).BeginInit();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.AutoGenerateColumns = false;
            this.dataGridView1.ColumnFiltersEnabled = true;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.idDataGridViewTextBoxColumn,
            this.pidDataGridViewTextBoxColumn,
            this.advertiserDataGridViewTextBoxColumn,
            this.campaignnameDataGridViewTextBoxColumn,
            this.statusDataGridViewTextBoxColumn,
            this.AM,
            this.noteDataGridViewTextBoxColumn,
            this.currDataGridViewTextBoxColumn,
            this.revenueDataGridViewTextBoxColumn,
            this.unfinalizedDataGridViewTextBoxColumn,
            this.finalizedDataGridViewTextBoxColumn,
            this.verifiedDataGridViewTextBoxColumn});
            this.dataGridView1.ColumnSelectorEnabled = true;
            this.dataGridView1.DataSource = this.campaignsRevenueBindingSource;
            this.dataGridView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView1.Location = new System.Drawing.Point(0, 28);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.ReadOnly = true;
            this.dataGridView1.Size = new System.Drawing.Size(1372, 483);
            this.dataGridView1.TabIndex = 0;
            // 
            // idDataGridViewTextBoxColumn
            // 
            this.idDataGridViewTextBoxColumn.DataPropertyName = "id";
            this.idDataGridViewTextBoxColumn.HeaderText = "id";
            this.idDataGridViewTextBoxColumn.Name = "idDataGridViewTextBoxColumn";
            this.idDataGridViewTextBoxColumn.ReadOnly = true;
            this.idDataGridViewTextBoxColumn.Visible = false;
            // 
            // pidDataGridViewTextBoxColumn
            // 
            this.pidDataGridViewTextBoxColumn.DataPropertyName = "pid";
            this.pidDataGridViewTextBoxColumn.HeaderText = "pid";
            this.pidDataGridViewTextBoxColumn.Name = "pidDataGridViewTextBoxColumn";
            this.pidDataGridViewTextBoxColumn.ReadOnly = true;
            this.pidDataGridViewTextBoxColumn.Visible = false;
            // 
            // advertiserDataGridViewTextBoxColumn
            // 
            this.advertiserDataGridViewTextBoxColumn.DataPropertyName = "Advertiser";
            this.advertiserDataGridViewTextBoxColumn.HeaderText = "Advertiser";
            this.advertiserDataGridViewTextBoxColumn.Name = "advertiserDataGridViewTextBoxColumn";
            this.advertiserDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // campaignnameDataGridViewTextBoxColumn
            // 
            this.campaignnameDataGridViewTextBoxColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.campaignnameDataGridViewTextBoxColumn.DataPropertyName = "campaign_name";
            this.campaignnameDataGridViewTextBoxColumn.HeaderText = "Campaign";
            this.campaignnameDataGridViewTextBoxColumn.Name = "campaignnameDataGridViewTextBoxColumn";
            this.campaignnameDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // statusDataGridViewTextBoxColumn
            // 
            this.statusDataGridViewTextBoxColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.statusDataGridViewTextBoxColumn.DataPropertyName = "Status";
            this.statusDataGridViewTextBoxColumn.HeaderText = "Status";
            this.statusDataGridViewTextBoxColumn.Name = "statusDataGridViewTextBoxColumn";
            this.statusDataGridViewTextBoxColumn.ReadOnly = true;
            this.statusDataGridViewTextBoxColumn.Width = 62;
            // 
            // AM
            // 
            this.AM.DataPropertyName = "AM";
            this.AM.HeaderText = "AM";
            this.AM.Name = "AM";
            this.AM.ReadOnly = true;
            this.AM.Width = 150;
            // 
            // noteDataGridViewTextBoxColumn
            // 
            this.noteDataGridViewTextBoxColumn.DataPropertyName = "Note";
            this.noteDataGridViewTextBoxColumn.HeaderText = "Note";
            this.noteDataGridViewTextBoxColumn.Name = "noteDataGridViewTextBoxColumn";
            this.noteDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // currDataGridViewTextBoxColumn
            // 
            this.currDataGridViewTextBoxColumn.DataPropertyName = "Curr";
            this.currDataGridViewTextBoxColumn.HeaderText = "Curr";
            this.currDataGridViewTextBoxColumn.Name = "currDataGridViewTextBoxColumn";
            this.currDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // revenueDataGridViewTextBoxColumn
            // 
            this.revenueDataGridViewTextBoxColumn.DataPropertyName = "Revenue";
            this.revenueDataGridViewTextBoxColumn.HeaderText = "Revenue";
            this.revenueDataGridViewTextBoxColumn.Name = "revenueDataGridViewTextBoxColumn";
            this.revenueDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // unfinalizedDataGridViewTextBoxColumn
            // 
            this.unfinalizedDataGridViewTextBoxColumn.DataPropertyName = "Unfinalized";
            this.unfinalizedDataGridViewTextBoxColumn.HeaderText = "Unfinalized";
            this.unfinalizedDataGridViewTextBoxColumn.Name = "unfinalizedDataGridViewTextBoxColumn";
            this.unfinalizedDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // finalizedDataGridViewTextBoxColumn
            // 
            this.finalizedDataGridViewTextBoxColumn.DataPropertyName = "Finalized";
            this.finalizedDataGridViewTextBoxColumn.HeaderText = "Finalized";
            this.finalizedDataGridViewTextBoxColumn.Name = "finalizedDataGridViewTextBoxColumn";
            this.finalizedDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // verifiedDataGridViewTextBoxColumn
            // 
            this.verifiedDataGridViewTextBoxColumn.DataPropertyName = "Verified";
            this.verifiedDataGridViewTextBoxColumn.HeaderText = "Verified";
            this.verifiedDataGridViewTextBoxColumn.Name = "verifiedDataGridViewTextBoxColumn";
            this.verifiedDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // campaignsRevenueBindingSource
            // 
            this.campaignsRevenueBindingSource.DataMember = "CampaignWorkflow";
            this.campaignsRevenueBindingSource.DataSource = this.campaignsRevenueDataSet;
            // 
            // campaignsRevenueDataSet
            // 
            this.campaignsRevenueDataSet.DataSetName = "CampaignsRevenueDataSet";
            this.campaignsRevenueDataSet.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // campaignWorkflowTableAdapter
            // 
            this.campaignWorkflowTableAdapter.ClearBeforeFill = true;
            // 
            // BottomToolStripPanel
            // 
            this.BottomToolStripPanel.Location = new System.Drawing.Point(0, 0);
            this.BottomToolStripPanel.Name = "BottomToolStripPanel";
            this.BottomToolStripPanel.Orientation = System.Windows.Forms.Orientation.Horizontal;
            this.BottomToolStripPanel.RowMargin = new System.Windows.Forms.Padding(3, 0, 0, 0);
            this.BottomToolStripPanel.Size = new System.Drawing.Size(0, 0);
            // 
            // TopToolStripPanel
            // 
            this.TopToolStripPanel.Location = new System.Drawing.Point(0, 0);
            this.TopToolStripPanel.Name = "TopToolStripPanel";
            this.TopToolStripPanel.Orientation = System.Windows.Forms.Orientation.Horizontal;
            this.TopToolStripPanel.RowMargin = new System.Windows.Forms.Padding(3, 0, 0, 0);
            this.TopToolStripPanel.Size = new System.Drawing.Size(0, 0);
            // 
            // RightToolStripPanel
            // 
            this.RightToolStripPanel.Location = new System.Drawing.Point(0, 0);
            this.RightToolStripPanel.Name = "RightToolStripPanel";
            this.RightToolStripPanel.Orientation = System.Windows.Forms.Orientation.Horizontal;
            this.RightToolStripPanel.RowMargin = new System.Windows.Forms.Padding(3, 0, 0, 0);
            this.RightToolStripPanel.Size = new System.Drawing.Size(0, 0);
            // 
            // LeftToolStripPanel
            // 
            this.LeftToolStripPanel.Location = new System.Drawing.Point(0, 0);
            this.LeftToolStripPanel.Name = "LeftToolStripPanel";
            this.LeftToolStripPanel.Orientation = System.Windows.Forms.Orientation.Horizontal;
            this.LeftToolStripPanel.RowMargin = new System.Windows.Forms.Padding(3, 0, 0, 0);
            this.LeftToolStripPanel.Size = new System.Drawing.Size(0, 0);
            // 
            // ContentPanel
            // 
            this.ContentPanel.Size = new System.Drawing.Size(498, 30);
            // 
            // buttonRefresh
            // 
            this.buttonRefresh.BackgroundImage = global::EomApp1.Properties.Resources.Refresh1;
            this.buttonRefresh.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.buttonRefresh.Location = new System.Drawing.Point(3, 3);
            this.buttonRefresh.Name = "buttonRefresh";
            this.buttonRefresh.Size = new System.Drawing.Size(25, 21);
            this.buttonRefresh.TabIndex = 2;
            this.buttonRefresh.UseVisualStyleBackColor = true;
            this.buttonRefresh.Click += new System.EventHandler(this.buttonRefresh_Click);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.buttonRefresh);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1372, 28);
            this.panel1.TabIndex = 3;
            // 
            // CampaignsRevenueForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1372, 511);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.panel1);
            this.Name = "CampaignsRevenueForm";
            this.Text = "Revenue Summary";
            this.Load += new System.EventHandler(this.CampaignsRevenueForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.campaignsRevenueBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.campaignsRevenueDataSet)).EndInit();
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.BindingSource campaignsRevenueBindingSource;
        private CampaignsRevenueDataSet campaignsRevenueDataSet;
        private CampaignsRevenueDataSetTableAdapters.CampaignWorkflowTableAdapter campaignWorkflowTableAdapter;
        private System.Windows.Forms.ToolStripPanel BottomToolStripPanel;
        private System.Windows.Forms.ToolStripPanel TopToolStripPanel;
        private System.Windows.Forms.ToolStripPanel RightToolStripPanel;
        private System.Windows.Forms.ToolStripPanel LeftToolStripPanel;
        private System.Windows.Forms.ToolStripContentPanel ContentPanel;
        private System.Windows.Forms.Button buttonRefresh;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.DataGridViewTextBoxColumn idDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn pidDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn advertiserDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn campaignnameDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn statusDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn AM;
        private System.Windows.Forms.DataGridViewTextBoxColumn noteDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn currDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn revenueDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn unfinalizedDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn finalizedDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn verifiedDataGridViewTextBoxColumn;
        private EomAppControls.ExtendedDataGridView dataGridView1;
    }
}