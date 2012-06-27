namespace EomApp1
{
    partial class CampaignSynchRequestForm
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
            this.accountManagerBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.dataSet11BindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.dataSet11 = new EomApp1.data.DataSet1();
            this.campaignBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.dataSet1 = new EomApp1.data.DataSet1();
            this.zRequestBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.campaignTableAdapter = new EomApp1.data.DataSet1TableAdapters.CampaignTableAdapter();
            this.zRequestTableAdapter = new EomApp1.data.DataSet1TableAdapters.zRequestTableAdapter();
            this.accountManagerTableAdapter = new EomApp1.data.DataSet1TableAdapters.AccountManagerTableAdapter();
            this.campaignListContextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.requestSynchToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.campaignDataGridView = new System.Windows.Forms.DataGridView();
            this.dataGridViewTextBoxColumn2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.zRequestDataGridView = new System.Windows.Forms.DataGridView();
            this.dataGridViewTextBoxColumn4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn5 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn6 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.accountManagerBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataSet11BindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataSet11)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.campaignBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataSet1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.zRequestBindingSource)).BeginInit();
            this.campaignListContextMenuStrip.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.campaignDataGridView)).BeginInit();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.zRequestDataGridView)).BeginInit();
            this.SuspendLayout();
            // 
            // accountManagerBindingSource
            // 
            this.accountManagerBindingSource.DataMember = "AccountManager";
            this.accountManagerBindingSource.DataSource = this.dataSet11BindingSource;
            // 
            // dataSet11BindingSource
            // 
            this.dataSet11BindingSource.DataSource = this.dataSet11;
            this.dataSet11BindingSource.Position = 0;
            // 
            // dataSet11
            // 
            this.dataSet11.DataSetName = "DataSet1";
            this.dataSet11.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // campaignBindingSource
            // 
            this.campaignBindingSource.DataMember = "Campaign";
            this.campaignBindingSource.DataSource = this.dataSet1;
            // 
            // dataSet1
            // 
            this.dataSet1.DataSetName = "DataSet1";
            this.dataSet1.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // zRequestBindingSource
            // 
            this.zRequestBindingSource.DataMember = "zRequest";
            this.zRequestBindingSource.DataSource = this.dataSet1;
            // 
            // campaignTableAdapter
            // 
            this.campaignTableAdapter.ClearBeforeFill = true;
            // 
            // zRequestTableAdapter
            // 
            this.zRequestTableAdapter.ClearBeforeFill = true;
            // 
            // accountManagerTableAdapter
            // 
            this.accountManagerTableAdapter.ClearBeforeFill = true;
            // 
            // campaignListContextMenuStrip
            // 
            this.campaignListContextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.requestSynchToolStripMenuItem});
            this.campaignListContextMenuStrip.Name = "campaignListContextMenuStrip";
            this.campaignListContextMenuStrip.Size = new System.Drawing.Size(152, 26);
            // 
            // requestSynchToolStripMenuItem
            // 
            this.requestSynchToolStripMenuItem.Name = "requestSynchToolStripMenuItem";
            this.requestSynchToolStripMenuItem.Size = new System.Drawing.Size(151, 22);
            this.requestSynchToolStripMenuItem.Text = "Request Synch";
            // 
            // campaignDataGridView
            // 
            this.campaignDataGridView.AllowUserToAddRows = false;
            this.campaignDataGridView.AllowUserToDeleteRows = false;
            this.campaignDataGridView.AllowUserToResizeColumns = false;
            this.campaignDataGridView.AllowUserToResizeRows = false;
            this.campaignDataGridView.AutoGenerateColumns = false;
            this.campaignDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.campaignDataGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dataGridViewTextBoxColumn2,
            this.dataGridViewTextBoxColumn3});
            this.campaignDataGridView.ContextMenuStrip = this.campaignListContextMenuStrip;
            this.campaignDataGridView.DataSource = this.campaignBindingSource;
            this.campaignDataGridView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.campaignDataGridView.Location = new System.Drawing.Point(0, 0);
            this.campaignDataGridView.Name = "campaignDataGridView";
            this.campaignDataGridView.ReadOnly = true;
            this.campaignDataGridView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.campaignDataGridView.Size = new System.Drawing.Size(414, 288);
            this.campaignDataGridView.TabIndex = 0;
            // 
            // dataGridViewTextBoxColumn2
            // 
            this.dataGridViewTextBoxColumn2.DataPropertyName = "pid";
            this.dataGridViewTextBoxColumn2.HeaderText = "pid";
            this.dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
            this.dataGridViewTextBoxColumn2.ReadOnly = true;
            this.dataGridViewTextBoxColumn2.Width = 50;
            // 
            // dataGridViewTextBoxColumn3
            // 
            this.dataGridViewTextBoxColumn3.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.dataGridViewTextBoxColumn3.DataPropertyName = "campaign_name";
            this.dataGridViewTextBoxColumn3.HeaderText = "campaign_name";
            this.dataGridViewTextBoxColumn3.Name = "dataGridViewTextBoxColumn3";
            this.dataGridViewTextBoxColumn3.ReadOnly = true;
            // 
            // comboBox1
            // 
            this.comboBox1.DataBindings.Add(new System.Windows.Forms.Binding("SelectedValue", this.campaignBindingSource, "account_manager_id", true));
            this.comboBox1.DataSource = this.accountManagerBindingSource;
            this.comboBox1.DisplayMember = "account_manager_name";
            this.comboBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.comboBox1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Location = new System.Drawing.Point(0, 0);
            this.comboBox1.Margin = new System.Windows.Forms.Padding(0);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(414, 21);
            this.comboBox1.TabIndex = 1;
            this.comboBox1.ValueMember = "id";
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.comboBox1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.splitContainer1, 0, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 5.429864F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 94.57014F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(414, 442);
            this.tableLayoutPanel1.TabIndex = 2;
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 23);
            this.splitContainer1.Margin = new System.Windows.Forms.Padding(0);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.campaignDataGridView);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.zRequestDataGridView);
            this.splitContainer1.Size = new System.Drawing.Size(414, 419);
            this.splitContainer1.SplitterDistance = 288;
            this.splitContainer1.TabIndex = 2;
            // 
            // zRequestDataGridView
            // 
            this.zRequestDataGridView.AllowUserToAddRows = false;
            this.zRequestDataGridView.AllowUserToDeleteRows = false;
            this.zRequestDataGridView.AutoGenerateColumns = false;
            this.zRequestDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.zRequestDataGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dataGridViewTextBoxColumn4,
            this.dataGridViewTextBoxColumn5,
            this.dataGridViewTextBoxColumn6});
            this.zRequestDataGridView.DataSource = this.zRequestBindingSource;
            this.zRequestDataGridView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.zRequestDataGridView.Location = new System.Drawing.Point(0, 0);
            this.zRequestDataGridView.Name = "zRequestDataGridView";
            this.zRequestDataGridView.ReadOnly = true;
            this.zRequestDataGridView.Size = new System.Drawing.Size(414, 127);
            this.zRequestDataGridView.TabIndex = 0;
            // 
            // dataGridViewTextBoxColumn4
            // 
            this.dataGridViewTextBoxColumn4.DataPropertyName = "request_id";
            this.dataGridViewTextBoxColumn4.HeaderText = "request_id";
            this.dataGridViewTextBoxColumn4.Name = "dataGridViewTextBoxColumn4";
            this.dataGridViewTextBoxColumn4.ReadOnly = true;
            // 
            // dataGridViewTextBoxColumn5
            // 
            this.dataGridViewTextBoxColumn5.DataPropertyName = "pid";
            this.dataGridViewTextBoxColumn5.HeaderText = "pid";
            this.dataGridViewTextBoxColumn5.Name = "dataGridViewTextBoxColumn5";
            this.dataGridViewTextBoxColumn5.ReadOnly = true;
            this.dataGridViewTextBoxColumn5.Width = 50;
            // 
            // dataGridViewTextBoxColumn6
            // 
            this.dataGridViewTextBoxColumn6.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.dataGridViewTextBoxColumn6.DataPropertyName = "status";
            this.dataGridViewTextBoxColumn6.HeaderText = "status";
            this.dataGridViewTextBoxColumn6.Name = "dataGridViewTextBoxColumn6";
            this.dataGridViewTextBoxColumn6.ReadOnly = true;
            // 
            // timer1
            // 
            this.timer1.Interval = 250;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // CampaignSynchRequestForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(414, 442);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "CampaignSynchRequestForm";
            this.Text = "Campaign Synch Request";
            this.Load += new System.EventHandler(this.CampaignSynchRequestForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.accountManagerBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataSet11BindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataSet11)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.campaignBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataSet1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.zRequestBindingSource)).EndInit();
            this.campaignListContextMenuStrip.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.campaignDataGridView)).EndInit();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.zRequestDataGridView)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private data.DataSet1 dataSet1;
        private System.Windows.Forms.BindingSource campaignBindingSource;
        private data.DataSet1TableAdapters.CampaignTableAdapter campaignTableAdapter;
        private System.Windows.Forms.BindingSource zRequestBindingSource;
        private data.DataSet1TableAdapters.zRequestTableAdapter zRequestTableAdapter;
        private System.Windows.Forms.BindingSource dataSet11BindingSource;
        private data.DataSet1 dataSet11;
        private System.Windows.Forms.BindingSource accountManagerBindingSource;
        private data.DataSet1TableAdapters.AccountManagerTableAdapter accountManagerTableAdapter;
        private System.Windows.Forms.ContextMenuStrip campaignListContextMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem requestSynchToolStripMenuItem;
        private System.Windows.Forms.DataGridView campaignDataGridView;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn3;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.DataGridView zRequestDataGridView;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn4;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn5;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn6;
        private System.Windows.Forms.Timer timer1;
    }
}