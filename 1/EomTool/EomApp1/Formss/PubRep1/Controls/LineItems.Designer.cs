namespace EomApp1.Formss.PubRep1.Controls
{
    partial class LineItems
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
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.verifiedLineItemsBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.publisherReportDataSet1 = new EomApp1.Formss.PubRep1.Data.PublisherReportDataSet1();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.payToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.verifiedLineItemsTableAdapter = new EomApp1.Formss.PubRep1.Data.PublisherReportDataSet1TableAdapters.VerifiedLineItemsTableAdapter();
            this.dataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn5 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn6 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn7 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn8 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn9 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn10 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn11 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn12 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.nameDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.item_id = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colaffid = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.item_accounting_status_id = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.item_accounting_status_name = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.email = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.addcodeDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.campaignnameDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.pidDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.statusDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.costcurrencynameDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.custtousdmultiplierDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.costperunitDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.numunitsDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.totalcostDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.paymentcurrencynameDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.paymenttousdmultiplierDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.media_buyer_name = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.verifiedLineItemsBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.publisherReportDataSet1)).BeginInit();
            this.contextMenuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.AutoGenerateColumns = false;
            this.dataGridView1.BackgroundColor = System.Drawing.Color.Navy;
            this.dataGridView1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dataGridView1.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.None;
            this.dataGridView1.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridView1.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.nameDataGridViewTextBoxColumn,
            this.item_id,
            this.colaffid,
            this.item_accounting_status_id,
            this.item_accounting_status_name,
            this.email,
            this.addcodeDataGridViewTextBoxColumn,
            this.campaignnameDataGridViewTextBoxColumn,
            this.pidDataGridViewTextBoxColumn,
            this.statusDataGridViewTextBoxColumn,
            this.costcurrencynameDataGridViewTextBoxColumn,
            this.custtousdmultiplierDataGridViewTextBoxColumn,
            this.costperunitDataGridViewTextBoxColumn,
            this.numunitsDataGridViewTextBoxColumn,
            this.totalcostDataGridViewTextBoxColumn,
            this.paymentcurrencynameDataGridViewTextBoxColumn,
            this.paymenttousdmultiplierDataGridViewTextBoxColumn,
            this.media_buyer_name});
            this.dataGridView1.DataSource = this.verifiedLineItemsBindingSource;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.Navy;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.Color.Red;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridView1.DefaultCellStyle = dataGridViewCellStyle2;
            this.dataGridView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView1.Location = new System.Drawing.Point(0, 0);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.ReadOnly = true;
            this.dataGridView1.RowHeadersVisible = false;
            this.dataGridView1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridView1.Size = new System.Drawing.Size(762, 150);
            this.dataGridView1.TabIndex = 0;
            // 
            // verifiedLineItemsBindingSource
            // 
            this.verifiedLineItemsBindingSource.DataMember = "VerifiedLineItems";
            this.verifiedLineItemsBindingSource.DataSource = this.publisherReportDataSet1;
            // 
            // publisherReportDataSet1
            // 
            this.publisherReportDataSet1.DataSetName = "PublisherReportDataSet1";
            this.publisherReportDataSet1.EnforceConstraints = false;
            this.publisherReportDataSet1.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.payToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(94, 26);
            // 
            // payToolStripMenuItem
            // 
            this.payToolStripMenuItem.Name = "payToolStripMenuItem";
            this.payToolStripMenuItem.Size = new System.Drawing.Size(93, 22);
            this.payToolStripMenuItem.Text = "Pay";
            this.payToolStripMenuItem.Click += new System.EventHandler(this.payToolStripMenuItem_Click);
            // 
            // verifiedLineItemsTableAdapter
            // 
            this.verifiedLineItemsTableAdapter.ClearBeforeFill = false;
            // 
            // dataGridViewTextBoxColumn1
            // 
            this.dataGridViewTextBoxColumn1.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.dataGridViewTextBoxColumn1.DataPropertyName = "name";
            this.dataGridViewTextBoxColumn1.HeaderText = "name";
            this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
            this.dataGridViewTextBoxColumn1.ReadOnly = true;
            // 
            // dataGridViewTextBoxColumn2
            // 
            this.dataGridViewTextBoxColumn2.DataPropertyName = "add_code";
            this.dataGridViewTextBoxColumn2.HeaderText = "add_code";
            this.dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
            this.dataGridViewTextBoxColumn2.ReadOnly = true;
            // 
            // dataGridViewTextBoxColumn3
            // 
            this.dataGridViewTextBoxColumn3.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.dataGridViewTextBoxColumn3.DataPropertyName = "campaign_name";
            this.dataGridViewTextBoxColumn3.HeaderText = "campaign_name";
            this.dataGridViewTextBoxColumn3.Name = "dataGridViewTextBoxColumn3";
            this.dataGridViewTextBoxColumn3.ReadOnly = true;
            // 
            // dataGridViewTextBoxColumn4
            // 
            this.dataGridViewTextBoxColumn4.DataPropertyName = "pid";
            this.dataGridViewTextBoxColumn4.HeaderText = "pid";
            this.dataGridViewTextBoxColumn4.Name = "dataGridViewTextBoxColumn4";
            this.dataGridViewTextBoxColumn4.ReadOnly = true;
            // 
            // dataGridViewTextBoxColumn5
            // 
            this.dataGridViewTextBoxColumn5.DataPropertyName = "status";
            this.dataGridViewTextBoxColumn5.HeaderText = "status";
            this.dataGridViewTextBoxColumn5.Name = "dataGridViewTextBoxColumn5";
            this.dataGridViewTextBoxColumn5.ReadOnly = true;
            // 
            // dataGridViewTextBoxColumn6
            // 
            this.dataGridViewTextBoxColumn6.DataPropertyName = "cost_currency_name";
            this.dataGridViewTextBoxColumn6.HeaderText = "cost_currency_name";
            this.dataGridViewTextBoxColumn6.Name = "dataGridViewTextBoxColumn6";
            this.dataGridViewTextBoxColumn6.ReadOnly = true;
            // 
            // dataGridViewTextBoxColumn7
            // 
            this.dataGridViewTextBoxColumn7.DataPropertyName = "cust_to_usd_multiplier";
            this.dataGridViewTextBoxColumn7.HeaderText = "cust_to_usd_multiplier";
            this.dataGridViewTextBoxColumn7.Name = "dataGridViewTextBoxColumn7";
            this.dataGridViewTextBoxColumn7.ReadOnly = true;
            // 
            // dataGridViewTextBoxColumn8
            // 
            this.dataGridViewTextBoxColumn8.DataPropertyName = "cost_per_unit";
            this.dataGridViewTextBoxColumn8.HeaderText = "cost_per_unit";
            this.dataGridViewTextBoxColumn8.Name = "dataGridViewTextBoxColumn8";
            this.dataGridViewTextBoxColumn8.ReadOnly = true;
            // 
            // dataGridViewTextBoxColumn9
            // 
            this.dataGridViewTextBoxColumn9.DataPropertyName = "num_units";
            this.dataGridViewTextBoxColumn9.HeaderText = "num_units";
            this.dataGridViewTextBoxColumn9.Name = "dataGridViewTextBoxColumn9";
            this.dataGridViewTextBoxColumn9.ReadOnly = true;
            // 
            // dataGridViewTextBoxColumn10
            // 
            this.dataGridViewTextBoxColumn10.DataPropertyName = "total_cost";
            this.dataGridViewTextBoxColumn10.HeaderText = "total_cost";
            this.dataGridViewTextBoxColumn10.Name = "dataGridViewTextBoxColumn10";
            this.dataGridViewTextBoxColumn10.ReadOnly = true;
            // 
            // dataGridViewTextBoxColumn11
            // 
            this.dataGridViewTextBoxColumn11.DataPropertyName = "payment_currency_name";
            this.dataGridViewTextBoxColumn11.HeaderText = "payment_currency_name";
            this.dataGridViewTextBoxColumn11.Name = "dataGridViewTextBoxColumn11";
            this.dataGridViewTextBoxColumn11.ReadOnly = true;
            // 
            // dataGridViewTextBoxColumn12
            // 
            this.dataGridViewTextBoxColumn12.DataPropertyName = "payment_to_usd_multiplier";
            this.dataGridViewTextBoxColumn12.HeaderText = "payment_to_usd_multiplier";
            this.dataGridViewTextBoxColumn12.Name = "dataGridViewTextBoxColumn12";
            this.dataGridViewTextBoxColumn12.ReadOnly = true;
            // 
            // nameDataGridViewTextBoxColumn
            // 
            this.nameDataGridViewTextBoxColumn.DataPropertyName = "name";
            this.nameDataGridViewTextBoxColumn.HeaderText = "name";
            this.nameDataGridViewTextBoxColumn.Name = "nameDataGridViewTextBoxColumn";
            this.nameDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // item_id
            // 
            this.item_id.DataPropertyName = "item_id";
            this.item_id.HeaderText = "item_id";
            this.item_id.Name = "item_id";
            this.item_id.ReadOnly = true;
            // 
            // colaffid
            // 
            this.colaffid.DataPropertyName = "affid";
            this.colaffid.HeaderText = "affid";
            this.colaffid.Name = "colaffid";
            this.colaffid.ReadOnly = true;
            // 
            // item_accounting_status_id
            // 
            this.item_accounting_status_id.DataPropertyName = "item_accounting_status_id";
            this.item_accounting_status_id.HeaderText = "item_accounting_status_id";
            this.item_accounting_status_id.Name = "item_accounting_status_id";
            this.item_accounting_status_id.ReadOnly = true;
            this.item_accounting_status_id.Visible = false;
            // 
            // item_accounting_status_name
            // 
            this.item_accounting_status_name.DataPropertyName = "item_accounting_status_name";
            this.item_accounting_status_name.HeaderText = "item_accounting_status_name";
            this.item_accounting_status_name.Name = "item_accounting_status_name";
            this.item_accounting_status_name.ReadOnly = true;
            // 
            // email
            // 
            this.email.DataPropertyName = "email";
            this.email.HeaderText = "email";
            this.email.Name = "email";
            this.email.ReadOnly = true;
            // 
            // addcodeDataGridViewTextBoxColumn
            // 
            this.addcodeDataGridViewTextBoxColumn.DataPropertyName = "add_code";
            this.addcodeDataGridViewTextBoxColumn.HeaderText = "add_code";
            this.addcodeDataGridViewTextBoxColumn.Name = "addcodeDataGridViewTextBoxColumn";
            this.addcodeDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // campaignnameDataGridViewTextBoxColumn
            // 
            this.campaignnameDataGridViewTextBoxColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.campaignnameDataGridViewTextBoxColumn.DataPropertyName = "campaign_name";
            this.campaignnameDataGridViewTextBoxColumn.HeaderText = "campaign_name";
            this.campaignnameDataGridViewTextBoxColumn.Name = "campaignnameDataGridViewTextBoxColumn";
            this.campaignnameDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // pidDataGridViewTextBoxColumn
            // 
            this.pidDataGridViewTextBoxColumn.DataPropertyName = "pid";
            this.pidDataGridViewTextBoxColumn.HeaderText = "pid";
            this.pidDataGridViewTextBoxColumn.Name = "pidDataGridViewTextBoxColumn";
            this.pidDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // statusDataGridViewTextBoxColumn
            // 
            this.statusDataGridViewTextBoxColumn.DataPropertyName = "status";
            this.statusDataGridViewTextBoxColumn.HeaderText = "status";
            this.statusDataGridViewTextBoxColumn.Name = "statusDataGridViewTextBoxColumn";
            this.statusDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // costcurrencynameDataGridViewTextBoxColumn
            // 
            this.costcurrencynameDataGridViewTextBoxColumn.DataPropertyName = "cost_currency_name";
            this.costcurrencynameDataGridViewTextBoxColumn.HeaderText = "cost_currency_name";
            this.costcurrencynameDataGridViewTextBoxColumn.Name = "costcurrencynameDataGridViewTextBoxColumn";
            this.costcurrencynameDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // custtousdmultiplierDataGridViewTextBoxColumn
            // 
            this.custtousdmultiplierDataGridViewTextBoxColumn.DataPropertyName = "cust_to_usd_multiplier";
            this.custtousdmultiplierDataGridViewTextBoxColumn.HeaderText = "cust_to_usd_multiplier";
            this.custtousdmultiplierDataGridViewTextBoxColumn.Name = "custtousdmultiplierDataGridViewTextBoxColumn";
            this.custtousdmultiplierDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // costperunitDataGridViewTextBoxColumn
            // 
            this.costperunitDataGridViewTextBoxColumn.DataPropertyName = "cost_per_unit";
            this.costperunitDataGridViewTextBoxColumn.HeaderText = "cost_per_unit";
            this.costperunitDataGridViewTextBoxColumn.Name = "costperunitDataGridViewTextBoxColumn";
            this.costperunitDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // numunitsDataGridViewTextBoxColumn
            // 
            this.numunitsDataGridViewTextBoxColumn.DataPropertyName = "num_units";
            this.numunitsDataGridViewTextBoxColumn.HeaderText = "num_units";
            this.numunitsDataGridViewTextBoxColumn.Name = "numunitsDataGridViewTextBoxColumn";
            this.numunitsDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // totalcostDataGridViewTextBoxColumn
            // 
            this.totalcostDataGridViewTextBoxColumn.DataPropertyName = "total_cost";
            this.totalcostDataGridViewTextBoxColumn.HeaderText = "total_cost";
            this.totalcostDataGridViewTextBoxColumn.Name = "totalcostDataGridViewTextBoxColumn";
            this.totalcostDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // paymentcurrencynameDataGridViewTextBoxColumn
            // 
            this.paymentcurrencynameDataGridViewTextBoxColumn.DataPropertyName = "payment_currency_name";
            this.paymentcurrencynameDataGridViewTextBoxColumn.HeaderText = "payment_currency_name";
            this.paymentcurrencynameDataGridViewTextBoxColumn.Name = "paymentcurrencynameDataGridViewTextBoxColumn";
            this.paymentcurrencynameDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // paymenttousdmultiplierDataGridViewTextBoxColumn
            // 
            this.paymenttousdmultiplierDataGridViewTextBoxColumn.DataPropertyName = "payment_to_usd_multiplier";
            this.paymenttousdmultiplierDataGridViewTextBoxColumn.HeaderText = "payment_to_usd_multiplier";
            this.paymenttousdmultiplierDataGridViewTextBoxColumn.Name = "paymenttousdmultiplierDataGridViewTextBoxColumn";
            this.paymenttousdmultiplierDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // media_buyer_name
            // 
            this.media_buyer_name.DataPropertyName = "media_buyer_name";
            this.media_buyer_name.HeaderText = "media_buyer_name";
            this.media_buyer_name.Name = "media_buyer_name";
            this.media_buyer_name.ReadOnly = true;
            // 
            // LineItems
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.dataGridView1);
            this.Name = "LineItems";
            this.Size = new System.Drawing.Size(762, 150);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.verifiedLineItemsBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.publisherReportDataSet1)).EndInit();
            this.contextMenuStrip1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.BindingSource verifiedLineItemsBindingSource;
        private Data.PublisherReportDataSet1 publisherReportDataSet1;
        private Data.PublisherReportDataSet1TableAdapters.VerifiedLineItemsTableAdapter verifiedLineItemsTableAdapter;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn3;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn4;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn5;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn6;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn7;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn8;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn9;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn10;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn11;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn12;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem payToolStripMenuItem;
        private System.Windows.Forms.DataGridViewTextBoxColumn nameDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn item_id;
        private System.Windows.Forms.DataGridViewTextBoxColumn colaffid;
        private System.Windows.Forms.DataGridViewTextBoxColumn item_accounting_status_id;
        private System.Windows.Forms.DataGridViewTextBoxColumn item_accounting_status_name;
        private System.Windows.Forms.DataGridViewTextBoxColumn email;
        private System.Windows.Forms.DataGridViewTextBoxColumn addcodeDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn campaignnameDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn pidDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn statusDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn costcurrencynameDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn custtousdmultiplierDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn costperunitDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn numunitsDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn totalcostDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn paymentcurrencynameDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn paymenttousdmultiplierDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn media_buyer_name;
    }
}
