namespace EomApp1.Formss.Final
{
    partial class CampaignNotes
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CampaignNotes));
            this.finalizeDataSet1 = new EomApp1.Formss.Final.FinalizeDataSet1();
            this.campaignBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.campaignTableAdapter = new EomApp1.Formss.Final.FinalizeDataSet1TableAdapters.CampaignTableAdapter();
            this.tableAdapterManager = new EomApp1.Formss.Final.FinalizeDataSet1TableAdapters.TableAdapterManager();
            this.campaignNotesTableAdapter = new EomApp1.Formss.Final.FinalizeDataSet1TableAdapters.CampaignNotesTableAdapter();
            this.campaignDataGridView = new System.Windows.Forms.DataGridView();
            this.dataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.campaignNotesBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.campaignNotesDataGridView = new System.Windows.Forms.DataGridView();
            this.dataGridViewTextBoxColumn11 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn12 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn13 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.campaignCampaignNotesBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.bindingNavigator1 = new System.Windows.Forms.BindingNavigator(this.components);
            this.saveToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.Created = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.finalizeDataSet1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.campaignBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.campaignDataGridView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.campaignNotesBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.campaignNotesDataGridView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.campaignCampaignNotesBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.bindingNavigator1)).BeginInit();
            this.bindingNavigator1.SuspendLayout();
            this.SuspendLayout();
            // 
            // finalizeDataSet1
            // 
            this.finalizeDataSet1.DataSetName = "FinalizeDataSet1";
            this.finalizeDataSet1.EnforceConstraints = false;
            this.finalizeDataSet1.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // campaignBindingSource
            // 
            this.campaignBindingSource.DataMember = "Campaign";
            this.campaignBindingSource.DataSource = this.finalizeDataSet1;
            // 
            // campaignTableAdapter
            // 
            this.campaignTableAdapter.ClearBeforeFill = true;
            // 
            // tableAdapterManager
            // 
            this.tableAdapterManager.BackupDataSetBeforeUpdate = false;
            this.tableAdapterManager.CampaignNotesTableAdapter = this.campaignNotesTableAdapter;
            this.tableAdapterManager.CampaignStatusTableAdapter = null;
            this.tableAdapterManager.UpdateOrder = EomApp1.Formss.Final.FinalizeDataSet1TableAdapters.TableAdapterManager.UpdateOrderOption.InsertUpdateDelete;
            // 
            // campaignNotesTableAdapter
            // 
            this.campaignNotesTableAdapter.ClearBeforeFill = true;
            // 
            // campaignDataGridView
            // 
            this.campaignDataGridView.AutoGenerateColumns = false;
            this.campaignDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.campaignDataGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dataGridViewTextBoxColumn1,
            this.dataGridViewTextBoxColumn2,
            this.dataGridViewTextBoxColumn3});
            this.campaignDataGridView.DataSource = this.campaignBindingSource;
            this.campaignDataGridView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.campaignDataGridView.Location = new System.Drawing.Point(0, 0);
            this.campaignDataGridView.Name = "campaignDataGridView";
            this.campaignDataGridView.Size = new System.Drawing.Size(541, 664);
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
            this.dataGridViewTextBoxColumn2.DataPropertyName = "pid";
            this.dataGridViewTextBoxColumn2.HeaderText = "pid";
            this.dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
            this.dataGridViewTextBoxColumn2.ReadOnly = true;
            this.dataGridViewTextBoxColumn2.Visible = false;
            // 
            // dataGridViewTextBoxColumn3
            // 
            this.dataGridViewTextBoxColumn3.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.dataGridViewTextBoxColumn3.DataPropertyName = "campaign_name";
            this.dataGridViewTextBoxColumn3.HeaderText = "campaign_name";
            this.dataGridViewTextBoxColumn3.Name = "dataGridViewTextBoxColumn3";
            // 
            // campaignNotesBindingSource
            // 
            this.campaignNotesBindingSource.DataMember = "CampaignNotes";
            this.campaignNotesBindingSource.DataSource = this.finalizeDataSet1;
            // 
            // campaignNotesDataGridView
            // 
            this.campaignNotesDataGridView.AllowUserToDeleteRows = false;
            this.campaignNotesDataGridView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.campaignNotesDataGridView.AutoGenerateColumns = false;
            this.campaignNotesDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.campaignNotesDataGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dataGridViewTextBoxColumn11,
            this.dataGridViewTextBoxColumn12,
            this.dataGridViewTextBoxColumn13,
            this.Created});
            this.campaignNotesDataGridView.DataSource = this.campaignCampaignNotesBindingSource;
            this.campaignNotesDataGridView.Location = new System.Drawing.Point(0, 28);
            this.campaignNotesDataGridView.Name = "campaignNotesDataGridView";
            this.campaignNotesDataGridView.Size = new System.Drawing.Size(415, 636);
            this.campaignNotesDataGridView.TabIndex = 2;
            // 
            // dataGridViewTextBoxColumn11
            // 
            this.dataGridViewTextBoxColumn11.DataPropertyName = "id";
            this.dataGridViewTextBoxColumn11.HeaderText = "id";
            this.dataGridViewTextBoxColumn11.Name = "dataGridViewTextBoxColumn11";
            this.dataGridViewTextBoxColumn11.ReadOnly = true;
            this.dataGridViewTextBoxColumn11.Visible = false;
            // 
            // dataGridViewTextBoxColumn12
            // 
            this.dataGridViewTextBoxColumn12.DataPropertyName = "campaign_id";
            this.dataGridViewTextBoxColumn12.HeaderText = "campaign_id";
            this.dataGridViewTextBoxColumn12.Name = "dataGridViewTextBoxColumn12";
            this.dataGridViewTextBoxColumn12.Visible = false;
            // 
            // dataGridViewTextBoxColumn13
            // 
            this.dataGridViewTextBoxColumn13.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.dataGridViewTextBoxColumn13.DataPropertyName = "note";
            this.dataGridViewTextBoxColumn13.HeaderText = "note";
            this.dataGridViewTextBoxColumn13.Name = "dataGridViewTextBoxColumn13";
            // 
            // campaignCampaignNotesBindingSource
            // 
            this.campaignCampaignNotesBindingSource.DataMember = "Campaign_CampaignNotes";
            this.campaignCampaignNotesBindingSource.DataSource = this.campaignBindingSource;
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.campaignDataGridView);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.bindingNavigator1);
            this.splitContainer1.Panel2.Controls.Add(this.campaignNotesDataGridView);
            this.splitContainer1.Size = new System.Drawing.Size(960, 664);
            this.splitContainer1.SplitterDistance = 541;
            this.splitContainer1.TabIndex = 3;
            // 
            // bindingNavigator1
            // 
            this.bindingNavigator1.AddNewItem = null;
            this.bindingNavigator1.BindingSource = this.campaignCampaignNotesBindingSource;
            this.bindingNavigator1.CountItem = null;
            this.bindingNavigator1.DeleteItem = null;
            this.bindingNavigator1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.bindingNavigator1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.saveToolStripButton});
            this.bindingNavigator1.Location = new System.Drawing.Point(0, 0);
            this.bindingNavigator1.MoveFirstItem = null;
            this.bindingNavigator1.MoveLastItem = null;
            this.bindingNavigator1.MoveNextItem = null;
            this.bindingNavigator1.MovePreviousItem = null;
            this.bindingNavigator1.Name = "bindingNavigator1";
            this.bindingNavigator1.PositionItem = null;
            this.bindingNavigator1.Size = new System.Drawing.Size(415, 25);
            this.bindingNavigator1.TabIndex = 3;
            this.bindingNavigator1.Text = "bindingNavigator1";
            // 
            // saveToolStripButton
            // 
            this.saveToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.saveToolStripButton.Image = ((System.Drawing.Image)(resources.GetObject("saveToolStripButton.Image")));
            this.saveToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.saveToolStripButton.Name = "saveToolStripButton";
            this.saveToolStripButton.Size = new System.Drawing.Size(23, 22);
            this.saveToolStripButton.Text = "&Save";
            this.saveToolStripButton.Click += new System.EventHandler(this.saveToolStripButton_Click);
            // 
            // Created
            // 
            this.Created.HeaderText = "Created";
            this.Created.Name = "Created";
            this.Created.ReadOnly = true;
            // 
            // CampaignNotes
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(960, 664);
            this.Controls.Add(this.splitContainer1);
            this.Name = "CampaignNotes";
            this.Text = "CampaignNotes";
            this.Load += new System.EventHandler(this.CampaignNotes_Load);
            ((System.ComponentModel.ISupportInitialize)(this.finalizeDataSet1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.campaignBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.campaignDataGridView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.campaignNotesBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.campaignNotesDataGridView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.campaignCampaignNotesBindingSource)).EndInit();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.bindingNavigator1)).EndInit();
            this.bindingNavigator1.ResumeLayout(false);
            this.bindingNavigator1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private FinalizeDataSet1 finalizeDataSet1;
        private System.Windows.Forms.BindingSource campaignBindingSource;
        private FinalizeDataSet1TableAdapters.CampaignTableAdapter campaignTableAdapter;
        private FinalizeDataSet1TableAdapters.TableAdapterManager tableAdapterManager;
        private FinalizeDataSet1TableAdapters.CampaignNotesTableAdapter campaignNotesTableAdapter;
        private System.Windows.Forms.DataGridView campaignDataGridView;
        private System.Windows.Forms.BindingSource campaignNotesBindingSource;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn3;
        private System.Windows.Forms.DataGridView campaignNotesDataGridView;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn11;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn12;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn13;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.BindingSource campaignCampaignNotesBindingSource;
        private System.Windows.Forms.BindingNavigator bindingNavigator1;
        private System.Windows.Forms.ToolStripButton saveToolStripButton;
        private System.Windows.Forms.DataGridViewTextBoxColumn Created;
    }
}