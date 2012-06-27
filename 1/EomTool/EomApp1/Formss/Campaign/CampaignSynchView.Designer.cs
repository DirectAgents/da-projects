namespace EomApp1.Formss.Campaign
{
    partial class CampaignSynchView
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.activeDataGridViewCheckBoxColumn = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.campaignnameDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.pidDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.intervalMinutesDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.lastSynchedDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.campaignSynchDataSetBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.campaignSynchDataSet = new EomApp1.Formss.Campaign.CampaignSynchDataSet();
            this._topPanelLeftFlow = new System.Windows.Forms.FlowLayoutPanel();
            this.searchControl1 = new EomApp1.Formss.Campaign.SearchControl();
            this._startLink = new System.Windows.Forms.LinkLabel();
            this._topPanelRightFlow = new System.Windows.Forms.FlowLayoutPanel();
            this._topPanel = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.campaignSynchDataTableTableAdapter = new EomApp1.Formss.Campaign.CampaignSynchDataSetTableAdapters.CampaignSynchDataTableTableAdapter();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this._toDay = new System.Windows.Forms.TextBox();
            this._fromDay = new System.Windows.Forms.TextBox();
            this._statsCheckBox = new System.Windows.Forms.CheckBox();
            this._payoutsCheckBox = new System.Windows.Forms.CheckBox();
            this.dataGridViewCheckBoxColumn1 = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.dataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.flowLayoutPanel2 = new System.Windows.Forms.FlowLayoutPanel();
            this._loopCheckBox = new System.Windows.Forms.CheckBox();
            this.panel1 = new System.Windows.Forms.Panel();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.campaignSynchDataSetBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.campaignSynchDataSet)).BeginInit();
            this._topPanelLeftFlow.SuspendLayout();
            this._topPanelRightFlow.SuspendLayout();
            this._topPanel.SuspendLayout();
            this.flowLayoutPanel1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.AllowUserToResizeColumns = false;
            this.dataGridView1.AllowUserToResizeRows = false;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.AliceBlue;
            this.dataGridView1.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.dataGridView1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridView1.AutoGenerateColumns = false;
            this.dataGridView1.BackgroundColor = System.Drawing.Color.WhiteSmoke;
            this.dataGridView1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dataGridView1.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridView1.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.activeDataGridViewCheckBoxColumn,
            this.campaignnameDataGridViewTextBoxColumn,
            this.pidDataGridViewTextBoxColumn,
            this.intervalMinutesDataGridViewTextBoxColumn,
            this.lastSynchedDataGridViewTextBoxColumn});
            this.dataGridView1.DataSource = this.campaignSynchDataSetBindingSource;
            this.dataGridView1.GridColor = System.Drawing.Color.WhiteSmoke;
            this.dataGridView1.Location = new System.Drawing.Point(0, 13);
            this.dataGridView1.Margin = new System.Windows.Forms.Padding(3, 0, 3, 3);
            this.dataGridView1.Name = "dataGridView1";
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.Color.AliceBlue;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridView1.RowHeadersDefaultCellStyle = dataGridViewCellStyle3;
            this.dataGridView1.RowHeadersVisible = false;
            this.dataGridView1.ShowCellErrors = false;
            this.dataGridView1.ShowCellToolTips = false;
            this.dataGridView1.ShowEditingIcon = false;
            this.dataGridView1.ShowRowErrors = false;
            this.dataGridView1.Size = new System.Drawing.Size(959, 488);
            this.dataGridView1.TabIndex = 1;
            // 
            // activeDataGridViewCheckBoxColumn
            // 
            this.activeDataGridViewCheckBoxColumn.DataPropertyName = "Active";
            this.activeDataGridViewCheckBoxColumn.HeaderText = "Active";
            this.activeDataGridViewCheckBoxColumn.Name = "activeDataGridViewCheckBoxColumn";
            this.activeDataGridViewCheckBoxColumn.Width = 50;
            // 
            // campaignnameDataGridViewTextBoxColumn
            // 
            this.campaignnameDataGridViewTextBoxColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.campaignnameDataGridViewTextBoxColumn.DataPropertyName = "campaign_name";
            this.campaignnameDataGridViewTextBoxColumn.HeaderText = "Campaign";
            this.campaignnameDataGridViewTextBoxColumn.Name = "campaignnameDataGridViewTextBoxColumn";
            this.campaignnameDataGridViewTextBoxColumn.ReadOnly = true;
            this.campaignnameDataGridViewTextBoxColumn.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            // 
            // pidDataGridViewTextBoxColumn
            // 
            this.pidDataGridViewTextBoxColumn.DataPropertyName = "pid";
            this.pidDataGridViewTextBoxColumn.HeaderText = "PID";
            this.pidDataGridViewTextBoxColumn.Name = "pidDataGridViewTextBoxColumn";
            this.pidDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // intervalMinutesDataGridViewTextBoxColumn
            // 
            this.intervalMinutesDataGridViewTextBoxColumn.DataPropertyName = "IntervalMinutes";
            this.intervalMinutesDataGridViewTextBoxColumn.HeaderText = "Interval Minutes";
            this.intervalMinutesDataGridViewTextBoxColumn.Name = "intervalMinutesDataGridViewTextBoxColumn";
            this.intervalMinutesDataGridViewTextBoxColumn.Width = 125;
            // 
            // lastSynchedDataGridViewTextBoxColumn
            // 
            this.lastSynchedDataGridViewTextBoxColumn.DataPropertyName = "LastSynched";
            this.lastSynchedDataGridViewTextBoxColumn.HeaderText = "Last Synched";
            this.lastSynchedDataGridViewTextBoxColumn.Name = "lastSynchedDataGridViewTextBoxColumn";
            this.lastSynchedDataGridViewTextBoxColumn.ReadOnly = true;
            this.lastSynchedDataGridViewTextBoxColumn.Width = 175;
            // 
            // campaignSynchDataSetBindingSource
            // 
            this.campaignSynchDataSetBindingSource.DataMember = "CampaignSynchDataTable";
            this.campaignSynchDataSetBindingSource.DataSource = this.campaignSynchDataSet;
            this.campaignSynchDataSetBindingSource.Sort = "campaign_name";
            // 
            // campaignSynchDataSet
            // 
            this.campaignSynchDataSet.DataSetName = "CampaignSynchDataSet";
            this.campaignSynchDataSet.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // _topPanelLeftFlow
            // 
            this._topPanelLeftFlow.Controls.Add(this.searchControl1);
            this._topPanelLeftFlow.Dock = System.Windows.Forms.DockStyle.Left;
            this._topPanelLeftFlow.Location = new System.Drawing.Point(0, 0);
            this._topPanelLeftFlow.Margin = new System.Windows.Forms.Padding(0);
            this._topPanelLeftFlow.Name = "_topPanelLeftFlow";
            this._topPanelLeftFlow.Size = new System.Drawing.Size(326, 16);
            this._topPanelLeftFlow.TabIndex = 5;
            // 
            // searchControl1
            // 
            this.searchControl1.Location = new System.Drawing.Point(0, 0);
            this.searchControl1.Margin = new System.Windows.Forms.Padding(0);
            this.searchControl1.Name = "searchControl1";
            this.searchControl1.SearchLabelText = "&Find";
            this.searchControl1.Size = new System.Drawing.Size(300, 16);
            this.searchControl1.TabIndex = 3;
            // 
            // _startLink
            // 
            this._startLink.AutoSize = true;
            this._startLink.Location = new System.Drawing.Point(195, 0);
            this._startLink.Name = "_startLink";
            this._startLink.Size = new System.Drawing.Size(29, 13);
            this._startLink.TabIndex = 4;
            this._startLink.TabStop = true;
            this._startLink.Text = "Start";
            this._startLink.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this._startLink_LinkClicked);
            // 
            // _topPanelRightFlow
            // 
            this._topPanelRightFlow.Controls.Add(this._startLink);
            this._topPanelRightFlow.Dock = System.Windows.Forms.DockStyle.Right;
            this._topPanelRightFlow.FlowDirection = System.Windows.Forms.FlowDirection.RightToLeft;
            this._topPanelRightFlow.Location = new System.Drawing.Point(732, 0);
            this._topPanelRightFlow.Name = "_topPanelRightFlow";
            this._topPanelRightFlow.Size = new System.Drawing.Size(227, 16);
            this._topPanelRightFlow.TabIndex = 5;
            // 
            // _topPanel
            // 
            this._topPanel.Controls.Add(this._topPanelLeftFlow);
            this._topPanel.Controls.Add(this._topPanelRightFlow);
            this._topPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this._topPanel.Location = new System.Drawing.Point(0, 0);
            this._topPanel.Margin = new System.Windows.Forms.Padding(0);
            this._topPanel.Name = "_topPanel";
            this._topPanel.Size = new System.Drawing.Size(959, 16);
            this._topPanel.TabIndex = 6;
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(354, 6);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(16, 13);
            this.label1.TabIndex = 8;
            this.label1.Text = "to";
            // 
            // campaignSynchDataTableTableAdapter
            // 
            this.campaignSynchDataTableTableAdapter.ClearBeforeFill = true;
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.BackColor = System.Drawing.Color.LightYellow;
            this.flowLayoutPanel1.Controls.Add(this._toDay);
            this.flowLayoutPanel1.Controls.Add(this.label1);
            this.flowLayoutPanel1.Controls.Add(this._fromDay);
            this.flowLayoutPanel1.Controls.Add(this._statsCheckBox);
            this.flowLayoutPanel1.Controls.Add(this._payoutsCheckBox);
            this.flowLayoutPanel1.Controls.Add(this._loopCheckBox);
            this.flowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Right;
            this.flowLayoutPanel1.FlowDirection = System.Windows.Forms.FlowDirection.RightToLeft;
            this.flowLayoutPanel1.Location = new System.Drawing.Point(552, 0);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(407, 28);
            this.flowLayoutPanel1.TabIndex = 10;
            this.flowLayoutPanel1.Leave += new System.EventHandler(this.flowLayoutPanel1_Leave);
            // 
            // _toDay
            // 
            this._toDay.DataBindings.Add(new System.Windows.Forms.Binding("Text", global::EomApp1.Properties.Settings.Default, "CampaignSynchViewToDay", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this._toDay.Location = new System.Drawing.Point(376, 3);
            this._toDay.Name = "_toDay";
            this._toDay.Size = new System.Drawing.Size(28, 20);
            this._toDay.TabIndex = 7;
            this._toDay.Text = global::EomApp1.Properties.Settings.Default.CampaignSynchViewToDay;
            // 
            // _fromDay
            // 
            this._fromDay.DataBindings.Add(new System.Windows.Forms.Binding("Text", global::EomApp1.Properties.Settings.Default, "CampaignSynchViewFromDay", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this._fromDay.Location = new System.Drawing.Point(320, 3);
            this._fromDay.Name = "_fromDay";
            this._fromDay.Size = new System.Drawing.Size(28, 20);
            this._fromDay.TabIndex = 7;
            this._fromDay.Text = global::EomApp1.Properties.Settings.Default.CampaignSynchViewFromDay;
            // 
            // _statsCheckBox
            // 
            this._statsCheckBox.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this._statsCheckBox.AutoSize = true;
            this._statsCheckBox.Checked = global::EomApp1.Properties.Settings.Default.CampaignSynchViewStatsChecked;
            this._statsCheckBox.DataBindings.Add(new System.Windows.Forms.Binding("Checked", global::EomApp1.Properties.Settings.Default, "CampaignSynchViewStatsChecked", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this._statsCheckBox.Location = new System.Drawing.Point(264, 6);
            this._statsCheckBox.Name = "_statsCheckBox";
            this._statsCheckBox.Size = new System.Drawing.Size(50, 17);
            this._statsCheckBox.TabIndex = 9;
            this._statsCheckBox.Text = "Stats";
            this._statsCheckBox.UseVisualStyleBackColor = true;
            // 
            // _payoutsCheckBox
            // 
            this._payoutsCheckBox.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this._payoutsCheckBox.AutoSize = true;
            this._payoutsCheckBox.Checked = global::EomApp1.Properties.Settings.Default.CampaignSynchViewPayoutsChecked;
            this._payoutsCheckBox.DataBindings.Add(new System.Windows.Forms.Binding("Checked", global::EomApp1.Properties.Settings.Default, "CampaignSynchViewPayoutsChecked", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this._payoutsCheckBox.Location = new System.Drawing.Point(194, 6);
            this._payoutsCheckBox.Name = "_payoutsCheckBox";
            this._payoutsCheckBox.Size = new System.Drawing.Size(64, 17);
            this._payoutsCheckBox.TabIndex = 9;
            this._payoutsCheckBox.Text = "Payouts";
            this._payoutsCheckBox.UseVisualStyleBackColor = true;
            // 
            // dataGridViewCheckBoxColumn1
            // 
            this.dataGridViewCheckBoxColumn1.DataPropertyName = "Active";
            this.dataGridViewCheckBoxColumn1.HeaderText = "Active";
            this.dataGridViewCheckBoxColumn1.Name = "dataGridViewCheckBoxColumn1";
            this.dataGridViewCheckBoxColumn1.Width = 50;
            // 
            // dataGridViewTextBoxColumn1
            // 
            this.dataGridViewTextBoxColumn1.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.dataGridViewTextBoxColumn1.DataPropertyName = "pid";
            this.dataGridViewTextBoxColumn1.HeaderText = "pid";
            this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
            this.dataGridViewTextBoxColumn1.ReadOnly = true;
            this.dataGridViewTextBoxColumn1.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            // 
            // dataGridViewTextBoxColumn2
            // 
            this.dataGridViewTextBoxColumn2.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.dataGridViewTextBoxColumn2.DataPropertyName = "campaign_name";
            this.dataGridViewTextBoxColumn2.HeaderText = "campaign_name";
            this.dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
            this.dataGridViewTextBoxColumn2.ReadOnly = true;
            // 
            // dataGridViewTextBoxColumn3
            // 
            this.dataGridViewTextBoxColumn3.DataPropertyName = "IntervalMinutes";
            this.dataGridViewTextBoxColumn3.HeaderText = "IntervalMinutes";
            this.dataGridViewTextBoxColumn3.Name = "dataGridViewTextBoxColumn3";
            this.dataGridViewTextBoxColumn3.Width = 75;
            // 
            // dataGridViewTextBoxColumn4
            // 
            this.dataGridViewTextBoxColumn4.DataPropertyName = "LastSynched";
            this.dataGridViewTextBoxColumn4.HeaderText = "LastSynched";
            this.dataGridViewTextBoxColumn4.Name = "dataGridViewTextBoxColumn4";
            this.dataGridViewTextBoxColumn4.ReadOnly = true;
            this.dataGridViewTextBoxColumn4.Width = 175;
            // 
            // flowLayoutPanel2
            // 
            this.flowLayoutPanel2.BackColor = System.Drawing.Color.LightYellow;
            this.flowLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Left;
            this.flowLayoutPanel2.FlowDirection = System.Windows.Forms.FlowDirection.RightToLeft;
            this.flowLayoutPanel2.Location = new System.Drawing.Point(0, 0);
            this.flowLayoutPanel2.Name = "flowLayoutPanel2";
            this.flowLayoutPanel2.Size = new System.Drawing.Size(300, 28);
            this.flowLayoutPanel2.TabIndex = 11;
            // 
            // _loopCheckBox
            // 
            this._loopCheckBox.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this._loopCheckBox.AutoSize = true;
            this._loopCheckBox.Checked = global::EomApp1.Properties.Settings.Default.CampaignSynchViewPayoutsChecked;
            this._loopCheckBox.DataBindings.Add(new System.Windows.Forms.Binding("Checked", global::EomApp1.Properties.Settings.Default, "CampaignSynchViewPayoutsChecked", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this._loopCheckBox.Location = new System.Drawing.Point(138, 6);
            this._loopCheckBox.Name = "_loopCheckBox";
            this._loopCheckBox.Size = new System.Drawing.Size(50, 17);
            this._loopCheckBox.TabIndex = 9;
            this._loopCheckBox.Text = "Loop";
            this._loopCheckBox.UseVisualStyleBackColor = true;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.flowLayoutPanel2);
            this.panel1.Controls.Add(this.flowLayoutPanel1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 507);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(959, 28);
            this.panel1.TabIndex = 12;
            // 
            // CampaignSynchView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panel1);
            this.Controls.Add(this._topPanel);
            this.Controls.Add(this.dataGridView1);
            this.Name = "CampaignSynchView";
            this.Size = new System.Drawing.Size(959, 535);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.campaignSynchDataSetBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.campaignSynchDataSet)).EndInit();
            this._topPanelLeftFlow.ResumeLayout(false);
            this._topPanelRightFlow.ResumeLayout(false);
            this._topPanelRightFlow.PerformLayout();
            this._topPanel.ResumeLayout(false);
            this.flowLayoutPanel1.ResumeLayout(false);
            this.flowLayoutPanel1.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.BindingSource campaignSynchDataSetBindingSource;
        private CampaignSynchDataSet campaignSynchDataSet;
        private CampaignSynchDataSetTableAdapters.CampaignSynchDataTableTableAdapter campaignSynchDataTableTableAdapter;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn3;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn4;
        private SearchControl searchControl1;
        private System.Windows.Forms.DataGridViewCheckBoxColumn activeDataGridViewCheckBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn campaignnameDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn pidDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn intervalMinutesDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn lastSynchedDataGridViewTextBoxColumn;
        private System.Windows.Forms.FlowLayoutPanel _topPanelLeftFlow;
        private System.Windows.Forms.LinkLabel _startLink;
        private System.Windows.Forms.FlowLayoutPanel _topPanelRightFlow;
        private System.Windows.Forms.Panel _topPanel;
        private System.Windows.Forms.DataGridViewCheckBoxColumn dataGridViewCheckBoxColumn1;
        private System.Windows.Forms.TextBox _fromDay;
        private System.Windows.Forms.TextBox _toDay;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckBox _statsCheckBox;
        private System.Windows.Forms.CheckBox _payoutsCheckBox;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.CheckBox _loopCheckBox;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel2;
        private System.Windows.Forms.Panel panel1;
    }
}
