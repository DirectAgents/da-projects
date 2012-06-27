namespace EomApp1.Formss.PubRep1.Controls
{
    partial class Report
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
            this.webBrowser1 = new System.Windows.Forms.WebBrowser();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.printToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.saveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.emailToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripTextBox1 = new System.Windows.Forms.ToolStripTextBox();
            this.payMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.label1 = new System.Windows.Forms.Label();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.toolStripLabel1 = new System.Windows.Forms.ToolStripLabel();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.idDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.vendornameDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.createdbyusernameDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.savedDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.pathtohardcopyDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.vendoridDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colEmailStatusMsg = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.pubReportInstanceBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.publisherReportDataSet1 = new EomApp1.Formss.PubRep1.Data.PublisherReportDataSet1();
            this.dataGridView2 = new System.Windows.Forms.DataGridView();
            this.dataGridViewTextBoxColumn7 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.sent_date = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.to_email_address = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.subject = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.fKPubReportEmailPubReportInstanceBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.pubReportInstanceTableAdapter = new EomApp1.Formss.PubRep1.Data.PublisherReportDataSet1TableAdapters.PubReportInstanceTableAdapter();
            this.toolStripLabel2 = new System.Windows.Forms.ToolStripLabel();
            this.pubReportEmailTableAdapter = new EomApp1.Formss.PubRep1.Data.PublisherReportDataSet1TableAdapters.PubReportEmailTableAdapter();
            this.dataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn5 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn6 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn8 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn9 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn10 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn11 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.menuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pubReportInstanceBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.publisherReportDataSet1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.fKPubReportEmailPubReportInstanceBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // webBrowser1
            // 
            this.webBrowser1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.webBrowser1.Location = new System.Drawing.Point(0, 0);
            this.webBrowser1.MinimumSize = new System.Drawing.Size(20, 20);
            this.webBrowser1.Name = "webBrowser1";
            this.webBrowser1.Size = new System.Drawing.Size(700, 439);
            this.webBrowser1.TabIndex = 0;
            // 
            // menuStrip1
            // 
            this.menuStrip1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.printToolStripMenuItem1,
            this.saveToolStripMenuItem,
            this.emailToolStripMenuItem,
            this.toolStripTextBox1,
            this.payMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(700, 24);
            this.menuStrip1.TabIndex = 3;
            this.menuStrip1.Text = "Report Menu";
            // 
            // printToolStripMenuItem1
            // 
            this.printToolStripMenuItem1.Name = "printToolStripMenuItem1";
            this.printToolStripMenuItem1.Size = new System.Drawing.Size(44, 20);
            this.printToolStripMenuItem1.Text = "Print";
            this.printToolStripMenuItem1.Click += new System.EventHandler(this.printToolStripMenuItem1_Click);
            // 
            // saveToolStripMenuItem
            // 
            this.saveToolStripMenuItem.Name = "saveToolStripMenuItem";
            this.saveToolStripMenuItem.Size = new System.Drawing.Size(43, 20);
            this.saveToolStripMenuItem.Text = "Save";
            this.saveToolStripMenuItem.Click += new System.EventHandler(this.saveToolStripMenuItem_Click);
            // 
            // emailToolStripMenuItem
            // 
            this.emailToolStripMenuItem.Name = "emailToolStripMenuItem";
            this.emailToolStripMenuItem.Size = new System.Drawing.Size(48, 20);
            this.emailToolStripMenuItem.Text = "Email";
            this.emailToolStripMenuItem.Click += new System.EventHandler(this.emailToolStripMenuItem_Click);
            // 
            // toolStripTextBox1
            // 
            this.toolStripTextBox1.AutoSize = false;
            this.toolStripTextBox1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.toolStripTextBox1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.toolStripTextBox1.MaxLength = 255;
            this.toolStripTextBox1.Name = "toolStripTextBox1";
            this.toolStripTextBox1.Size = new System.Drawing.Size(200, 20);
            this.toolStripTextBox1.Enter += new System.EventHandler(this.toolStripTextBox1_Enter);
            this.toolStripTextBox1.Leave += new System.EventHandler(this.toolStripTextBox1_Leave);
            // 
            // payMenuItem
            // 
            this.payMenuItem.Checked = true;
            this.payMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.payMenuItem.Name = "payMenuItem";
            this.payMenuItem.Size = new System.Drawing.Size(38, 20);
            this.payMenuItem.Text = "Pay";
            this.payMenuItem.Click += new System.EventHandler(this.toolStripMenuItem1_Click);
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 24);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.label1);
            this.splitContainer1.Panel1.Controls.Add(this.toolStrip1);
            this.splitContainer1.Panel1.Controls.Add(this.webBrowser1);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.splitContainer2);
            this.splitContainer1.Size = new System.Drawing.Size(700, 676);
            this.splitContainer1.SplitterDistance = 439;
            this.splitContainer1.TabIndex = 4;
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(224)))), ((int)(((byte)(192)))));
            this.label1.Dock = System.Windows.Forms.DockStyle.Top;
            this.label1.Location = new System.Drawing.Point(0, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(700, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "unsent";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.label1.Visible = false;
            // 
            // toolStrip1
            // 
            this.toolStrip1.AutoSize = false;
            this.toolStrip1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.toolStrip1.CanOverflow = false;
            this.toolStrip1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.toolStrip1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripLabel1});
            this.toolStrip1.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.HorizontalStackWithOverflow;
            this.toolStrip1.Location = new System.Drawing.Point(0, 414);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(700, 25);
            this.toolStrip1.TabIndex = 1;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // toolStripLabel1
            // 
            this.toolStripLabel1.Name = "toolStripLabel1";
            this.toolStripLabel1.Size = new System.Drawing.Size(45, 22);
            this.toolStripLabel1.Text = "History";
            this.toolStripLabel1.Click += new System.EventHandler(this.toolStripLabel1_Click_1);
            // 
            // splitContainer2
            // 
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer2.Location = new System.Drawing.Point(0, 0);
            this.splitContainer2.Name = "splitContainer2";
            this.splitContainer2.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add(this.dataGridView1);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.dataGridView2);
            this.splitContainer2.Size = new System.Drawing.Size(700, 233);
            this.splitContainer2.SplitterDistance = 159;
            this.splitContainer2.TabIndex = 2;
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.AutoGenerateColumns = false;
            this.dataGridView1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.idDataGridViewTextBoxColumn,
            this.vendornameDataGridViewTextBoxColumn,
            this.createdbyusernameDataGridViewTextBoxColumn,
            this.savedDataGridViewTextBoxColumn,
            this.pathtohardcopyDataGridViewTextBoxColumn,
            this.vendoridDataGridViewTextBoxColumn,
            this.colEmailStatusMsg});
            this.dataGridView1.DataSource = this.pubReportInstanceBindingSource;
            this.dataGridView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView1.Location = new System.Drawing.Point(0, 0);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.ReadOnly = true;
            this.dataGridView1.RowHeadersVisible = false;
            this.dataGridView1.Size = new System.Drawing.Size(700, 159);
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
            // vendornameDataGridViewTextBoxColumn
            // 
            this.vendornameDataGridViewTextBoxColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.vendornameDataGridViewTextBoxColumn.DataPropertyName = "vendor_name";
            this.vendornameDataGridViewTextBoxColumn.HeaderText = "Publisher";
            this.vendornameDataGridViewTextBoxColumn.Name = "vendornameDataGridViewTextBoxColumn";
            this.vendornameDataGridViewTextBoxColumn.ReadOnly = true;
            this.vendornameDataGridViewTextBoxColumn.Width = 200;
            // 
            // createdbyusernameDataGridViewTextBoxColumn
            // 
            this.createdbyusernameDataGridViewTextBoxColumn.DataPropertyName = "created_by_user_name";
            this.createdbyusernameDataGridViewTextBoxColumn.HeaderText = "Saved By";
            this.createdbyusernameDataGridViewTextBoxColumn.Name = "createdbyusernameDataGridViewTextBoxColumn";
            this.createdbyusernameDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // savedDataGridViewTextBoxColumn
            // 
            this.savedDataGridViewTextBoxColumn.DataPropertyName = "saved";
            this.savedDataGridViewTextBoxColumn.HeaderText = "Time Saved";
            this.savedDataGridViewTextBoxColumn.Name = "savedDataGridViewTextBoxColumn";
            this.savedDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // pathtohardcopyDataGridViewTextBoxColumn
            // 
            this.pathtohardcopyDataGridViewTextBoxColumn.DataPropertyName = "path_to_hard_copy";
            this.pathtohardcopyDataGridViewTextBoxColumn.HeaderText = "On Disk";
            this.pathtohardcopyDataGridViewTextBoxColumn.Name = "pathtohardcopyDataGridViewTextBoxColumn";
            this.pathtohardcopyDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // vendoridDataGridViewTextBoxColumn
            // 
            this.vendoridDataGridViewTextBoxColumn.DataPropertyName = "vendor_id";
            this.vendoridDataGridViewTextBoxColumn.HeaderText = "vendor_id";
            this.vendoridDataGridViewTextBoxColumn.Name = "vendoridDataGridViewTextBoxColumn";
            this.vendoridDataGridViewTextBoxColumn.ReadOnly = true;
            this.vendoridDataGridViewTextBoxColumn.Visible = false;
            // 
            // colEmailStatusMsg
            // 
            this.colEmailStatusMsg.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.colEmailStatusMsg.DataPropertyName = "email_status_msg";
            this.colEmailStatusMsg.HeaderText = "Email Status Msg";
            this.colEmailStatusMsg.Name = "colEmailStatusMsg";
            this.colEmailStatusMsg.ReadOnly = true;
            // 
            // pubReportInstanceBindingSource
            // 
            this.pubReportInstanceBindingSource.DataMember = "PubReportInstance";
            this.pubReportInstanceBindingSource.DataSource = this.publisherReportDataSet1;
            // 
            // publisherReportDataSet1
            // 
            this.publisherReportDataSet1.DataSetName = "PublisherReportDataSet1";
            this.publisherReportDataSet1.EnforceConstraints = false;
            this.publisherReportDataSet1.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // dataGridView2
            // 
            this.dataGridView2.AllowUserToAddRows = false;
            this.dataGridView2.AllowUserToDeleteRows = false;
            this.dataGridView2.AutoGenerateColumns = false;
            this.dataGridView2.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dataGridView2.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView2.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dataGridViewTextBoxColumn7,
            this.sent_date,
            this.to_email_address,
            this.subject});
            this.dataGridView2.DataSource = this.fKPubReportEmailPubReportInstanceBindingSource;
            this.dataGridView2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView2.Location = new System.Drawing.Point(0, 0);
            this.dataGridView2.Name = "dataGridView2";
            this.dataGridView2.ReadOnly = true;
            this.dataGridView2.RowHeadersVisible = false;
            this.dataGridView2.Size = new System.Drawing.Size(700, 70);
            this.dataGridView2.TabIndex = 1;
            // 
            // dataGridViewTextBoxColumn7
            // 
            this.dataGridViewTextBoxColumn7.DataPropertyName = "id";
            this.dataGridViewTextBoxColumn7.HeaderText = "id";
            this.dataGridViewTextBoxColumn7.Name = "dataGridViewTextBoxColumn7";
            this.dataGridViewTextBoxColumn7.ReadOnly = true;
            this.dataGridViewTextBoxColumn7.Visible = false;
            // 
            // sent_date
            // 
            this.sent_date.DataPropertyName = "sent_date";
            this.sent_date.HeaderText = "Sent Date";
            this.sent_date.Name = "sent_date";
            this.sent_date.ReadOnly = true;
            // 
            // to_email_address
            // 
            this.to_email_address.DataPropertyName = "to_email_address";
            this.to_email_address.HeaderText = "To Email";
            this.to_email_address.Name = "to_email_address";
            this.to_email_address.ReadOnly = true;
            // 
            // subject
            // 
            this.subject.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.subject.DataPropertyName = "subject";
            this.subject.HeaderText = "Subject";
            this.subject.Name = "subject";
            this.subject.ReadOnly = true;
            // 
            // fKPubReportEmailPubReportInstanceBindingSource
            // 
            this.fKPubReportEmailPubReportInstanceBindingSource.DataMember = "FK_PubReportEmail_PubReportInstance";
            this.fKPubReportEmailPubReportInstanceBindingSource.DataSource = this.pubReportInstanceBindingSource;
            // 
            // pubReportInstanceTableAdapter
            // 
            this.pubReportInstanceTableAdapter.ClearBeforeFill = true;
            // 
            // toolStripLabel2
            // 
            this.toolStripLabel2.Name = "toolStripLabel2";
            this.toolStripLabel2.Size = new System.Drawing.Size(86, 15);
            this.toolStripLabel2.Text = "toolStripLabel2";
            // 
            // pubReportEmailTableAdapter
            // 
            this.pubReportEmailTableAdapter.ClearBeforeFill = true;
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
            this.dataGridViewTextBoxColumn2.DataPropertyName = "vendor_name";
            this.dataGridViewTextBoxColumn2.HeaderText = "Publisher";
            this.dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
            this.dataGridViewTextBoxColumn2.ReadOnly = true;
            // 
            // dataGridViewTextBoxColumn3
            // 
            this.dataGridViewTextBoxColumn3.DataPropertyName = "created_by_user_name";
            this.dataGridViewTextBoxColumn3.HeaderText = "Saved By";
            this.dataGridViewTextBoxColumn3.Name = "dataGridViewTextBoxColumn3";
            this.dataGridViewTextBoxColumn3.ReadOnly = true;
            // 
            // dataGridViewTextBoxColumn4
            // 
            this.dataGridViewTextBoxColumn4.DataPropertyName = "saved";
            this.dataGridViewTextBoxColumn4.HeaderText = "Time Saved";
            this.dataGridViewTextBoxColumn4.Name = "dataGridViewTextBoxColumn4";
            this.dataGridViewTextBoxColumn4.ReadOnly = true;
            // 
            // dataGridViewTextBoxColumn5
            // 
            this.dataGridViewTextBoxColumn5.DataPropertyName = "path_to_hard_copy";
            this.dataGridViewTextBoxColumn5.HeaderText = "On Disk";
            this.dataGridViewTextBoxColumn5.Name = "dataGridViewTextBoxColumn5";
            this.dataGridViewTextBoxColumn5.ReadOnly = true;
            // 
            // dataGridViewTextBoxColumn6
            // 
            this.dataGridViewTextBoxColumn6.DataPropertyName = "vendor_id";
            this.dataGridViewTextBoxColumn6.HeaderText = "vendor_id";
            this.dataGridViewTextBoxColumn6.Name = "dataGridViewTextBoxColumn6";
            this.dataGridViewTextBoxColumn6.ReadOnly = true;
            this.dataGridViewTextBoxColumn6.Visible = false;
            // 
            // dataGridViewTextBoxColumn8
            // 
            this.dataGridViewTextBoxColumn8.DataPropertyName = "id";
            this.dataGridViewTextBoxColumn8.HeaderText = "id";
            this.dataGridViewTextBoxColumn8.Name = "dataGridViewTextBoxColumn8";
            this.dataGridViewTextBoxColumn8.Visible = false;
            // 
            // dataGridViewTextBoxColumn9
            // 
            this.dataGridViewTextBoxColumn9.DataPropertyName = "sent_date";
            this.dataGridViewTextBoxColumn9.HeaderText = "sent_date";
            this.dataGridViewTextBoxColumn9.Name = "dataGridViewTextBoxColumn9";
            // 
            // dataGridViewTextBoxColumn10
            // 
            this.dataGridViewTextBoxColumn10.DataPropertyName = "to_email_address";
            this.dataGridViewTextBoxColumn10.HeaderText = "to_email_address";
            this.dataGridViewTextBoxColumn10.Name = "dataGridViewTextBoxColumn10";
            // 
            // dataGridViewTextBoxColumn11
            // 
            this.dataGridViewTextBoxColumn11.DataPropertyName = "subject";
            this.dataGridViewTextBoxColumn11.HeaderText = "subject";
            this.dataGridViewTextBoxColumn11.Name = "dataGridViewTextBoxColumn11";
            // 
            // Report
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.menuStrip1);
            this.Name = "Report";
            this.Size = new System.Drawing.Size(700, 700);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
            this.splitContainer2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pubReportInstanceBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.publisherReportDataSet1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.fKPubReportEmailPubReportInstanceBindingSource)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.WebBrowser webBrowser1;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem printToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem saveToolStripMenuItem;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.BindingSource pubReportInstanceBindingSource;
        private Data.PublisherReportDataSet1 publisherReportDataSet1;
        private Data.PublisherReportDataSet1TableAdapters.PubReportInstanceTableAdapter pubReportInstanceTableAdapter;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn3;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn4;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn5;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn6;
        private System.Windows.Forms.ToolStripLabel toolStripLabel2;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripLabel toolStripLabel1;
        private System.Windows.Forms.ToolStripMenuItem emailToolStripMenuItem;
        private System.Windows.Forms.ToolStripTextBox toolStripTextBox1;
        private System.Windows.Forms.DataGridView dataGridView2;
        private System.Windows.Forms.BindingSource fKPubReportEmailPubReportInstanceBindingSource;
        private Data.PublisherReportDataSet1TableAdapters.PubReportEmailTableAdapter pubReportEmailTableAdapter;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn8;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn9;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn10;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn11;
        private System.Windows.Forms.DataGridViewTextBoxColumn idDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn vendornameDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn createdbyusernameDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn savedDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn pathtohardcopyDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn vendoridDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn colEmailStatusMsg;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn7;
        private System.Windows.Forms.DataGridViewTextBoxColumn sent_date;
        private System.Windows.Forms.DataGridViewTextBoxColumn to_email_address;
        private System.Windows.Forms.DataGridViewTextBoxColumn subject;
        private System.Windows.Forms.ToolStripMenuItem payMenuItem;
    }
}
