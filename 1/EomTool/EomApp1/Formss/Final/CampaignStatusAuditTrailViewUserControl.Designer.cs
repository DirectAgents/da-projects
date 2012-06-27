namespace EomApp1.Formss.Final
{
    partial class CampaignStatusAuditTrailViewUserControl
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
            this.campaignStatusAuditTrailViewBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.dADatabaseR1DataSet = new EomApp1.DADatabaseR1DataSet();
            this.campaignStatusAuditTrailViewTableAdapter = new EomApp1.DADatabaseR1DataSetTableAdapters.CampaignStatusAuditTrailViewTableAdapter();
            this.button1 = new System.Windows.Forms.Button();
            this.dataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.statuschangedatetimeDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.changedbysystemuserDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.campaignnameDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.nameDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.campaignStatusAuditTrailViewBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dADatabaseR1DataSet)).BeginInit();
            this.SuspendLayout();
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridView1.AutoGenerateColumns = false;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.statuschangedatetimeDataGridViewTextBoxColumn,
            this.changedbysystemuserDataGridViewTextBoxColumn,
            this.campaignnameDataGridViewTextBoxColumn,
            this.nameDataGridViewTextBoxColumn});
            this.dataGridView1.DataSource = this.campaignStatusAuditTrailViewBindingSource;
            this.dataGridView1.Location = new System.Drawing.Point(0, 28);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.ReadOnly = true;
            this.dataGridView1.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.dataGridView1.Size = new System.Drawing.Size(510, 420);
            this.dataGridView1.TabIndex = 0;
            // 
            // campaignStatusAuditTrailViewBindingSource
            // 
            this.campaignStatusAuditTrailViewBindingSource.DataMember = "CampaignStatusAuditTrailView";
            this.campaignStatusAuditTrailViewBindingSource.DataSource = this.dADatabaseR1DataSet;
            // 
            // dADatabaseR1DataSet
            // 
            this.dADatabaseR1DataSet.DataSetName = "DADatabaseR1DataSet";
            this.dADatabaseR1DataSet.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // campaignStatusAuditTrailViewTableAdapter
            // 
            this.campaignStatusAuditTrailViewTableAdapter.ClearBeforeFill = true;
            // 
            // button1
            // 
            this.button1.BackgroundImage = global::EomApp1.Properties.Resources.Refresh1;
            this.button1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.button1.Location = new System.Drawing.Point(3, 3);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(25, 21);
            this.button1.TabIndex = 2;
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // dataGridViewTextBoxColumn1
            // 
            this.dataGridViewTextBoxColumn1.DataPropertyName = "status_change_datetime";
            this.dataGridViewTextBoxColumn1.HeaderText = "status_change_datetime";
            this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
            this.dataGridViewTextBoxColumn1.ReadOnly = true;
            // 
            // dataGridViewTextBoxColumn2
            // 
            this.dataGridViewTextBoxColumn2.DataPropertyName = "campaign_name";
            this.dataGridViewTextBoxColumn2.HeaderText = "campaign_name";
            this.dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
            this.dataGridViewTextBoxColumn2.ReadOnly = true;
            // 
            // dataGridViewTextBoxColumn3
            // 
            this.dataGridViewTextBoxColumn3.DataPropertyName = "name";
            this.dataGridViewTextBoxColumn3.HeaderText = "name";
            this.dataGridViewTextBoxColumn3.Name = "dataGridViewTextBoxColumn3";
            this.dataGridViewTextBoxColumn3.ReadOnly = true;
            // 
            // dataGridViewTextBoxColumn4
            // 
            this.dataGridViewTextBoxColumn4.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.dataGridViewTextBoxColumn4.DataPropertyName = "name";
            this.dataGridViewTextBoxColumn4.HeaderText = "name";
            this.dataGridViewTextBoxColumn4.Name = "dataGridViewTextBoxColumn4";
            this.dataGridViewTextBoxColumn4.ReadOnly = true;
            // 
            // statuschangedatetimeDataGridViewTextBoxColumn
            // 
            this.statuschangedatetimeDataGridViewTextBoxColumn.DataPropertyName = "status_change_datetime";
            this.statuschangedatetimeDataGridViewTextBoxColumn.HeaderText = "Time";
            this.statuschangedatetimeDataGridViewTextBoxColumn.Name = "statuschangedatetimeDataGridViewTextBoxColumn";
            this.statuschangedatetimeDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // changedbysystemuserDataGridViewTextBoxColumn
            // 
            this.changedbysystemuserDataGridViewTextBoxColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.changedbysystemuserDataGridViewTextBoxColumn.DataPropertyName = "changed_by_system_user";
            this.changedbysystemuserDataGridViewTextBoxColumn.HeaderText = "User";
            this.changedbysystemuserDataGridViewTextBoxColumn.Name = "changedbysystemuserDataGridViewTextBoxColumn";
            this.changedbysystemuserDataGridViewTextBoxColumn.ReadOnly = true;
            this.changedbysystemuserDataGridViewTextBoxColumn.Width = 54;
            // 
            // campaignnameDataGridViewTextBoxColumn
            // 
            this.campaignnameDataGridViewTextBoxColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.campaignnameDataGridViewTextBoxColumn.DataPropertyName = "campaign_name";
            this.campaignnameDataGridViewTextBoxColumn.HeaderText = "Campaign";
            this.campaignnameDataGridViewTextBoxColumn.Name = "campaignnameDataGridViewTextBoxColumn";
            this.campaignnameDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // nameDataGridViewTextBoxColumn
            // 
            this.nameDataGridViewTextBoxColumn.DataPropertyName = "name";
            this.nameDataGridViewTextBoxColumn.HeaderText = "Status";
            this.nameDataGridViewTextBoxColumn.Name = "nameDataGridViewTextBoxColumn";
            this.nameDataGridViewTextBoxColumn.ReadOnly = true;
            this.nameDataGridViewTextBoxColumn.Width = 166;
            // 
            // CampaignStatusAuditTrailViewUserControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.button1);
            this.Controls.Add(this.dataGridView1);
            this.Name = "CampaignStatusAuditTrailViewUserControl";
            this.Size = new System.Drawing.Size(509, 453);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.campaignStatusAuditTrailViewBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dADatabaseR1DataSet)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn3;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.BindingSource campaignStatusAuditTrailViewBindingSource;
        private DADatabaseR1DataSet dADatabaseR1DataSet;
        private DADatabaseR1DataSetTableAdapters.CampaignStatusAuditTrailViewTableAdapter campaignStatusAuditTrailViewTableAdapter;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn4;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.DataGridViewTextBoxColumn statuschangedatetimeDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn changedbysystemuserDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn campaignnameDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn nameDataGridViewTextBoxColumn;

    }
}
