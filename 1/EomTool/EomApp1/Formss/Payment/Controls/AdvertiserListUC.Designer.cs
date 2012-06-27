namespace EomApp1.Formss.Payment.Controls
{
    partial class AdvertiserListUC
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
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.idDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.nameDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.advertisercreditlimitcurrencyidDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.advertisercreditlimitDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.advertiserBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.paymentsDataSet = new EomApp1.Formss.Payment.DataSets.PaymentsDataSet();
            this.advertiserTableAdapter = new EomApp1.Formss.Payment.DataSets.PaymentsDataSetTableAdapters.AdvertiserTableAdapter();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.advertiserBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.paymentsDataSet)).BeginInit();
            this.SuspendLayout();
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.AutoGenerateColumns = false;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.idDataGridViewTextBoxColumn,
            this.nameDataGridViewTextBoxColumn,
            this.advertisercreditlimitcurrencyidDataGridViewTextBoxColumn,
            this.advertisercreditlimitDataGridViewTextBoxColumn});
            this.dataGridView1.DataSource = this.advertiserBindingSource;
            this.dataGridView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView1.Location = new System.Drawing.Point(0, 0);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.ReadOnly = true;
            this.dataGridView1.RowHeadersVisible = false;
            this.dataGridView1.Size = new System.Drawing.Size(553, 639);
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
            // nameDataGridViewTextBoxColumn
            // 
            this.nameDataGridViewTextBoxColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.nameDataGridViewTextBoxColumn.DataPropertyName = "name";
            this.nameDataGridViewTextBoxColumn.HeaderText = "Advertiser";
            this.nameDataGridViewTextBoxColumn.Name = "nameDataGridViewTextBoxColumn";
            this.nameDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // advertisercreditlimitcurrencyidDataGridViewTextBoxColumn
            // 
            this.advertisercreditlimitcurrencyidDataGridViewTextBoxColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.advertisercreditlimitcurrencyidDataGridViewTextBoxColumn.DataPropertyName = "advertiser_credit_limit_currency_id";
            this.advertisercreditlimitcurrencyidDataGridViewTextBoxColumn.HeaderText = "Curr";
            this.advertisercreditlimitcurrencyidDataGridViewTextBoxColumn.Name = "advertisercreditlimitcurrencyidDataGridViewTextBoxColumn";
            this.advertisercreditlimitcurrencyidDataGridViewTextBoxColumn.ReadOnly = true;
            this.advertisercreditlimitcurrencyidDataGridViewTextBoxColumn.Width = 51;
            // 
            // advertisercreditlimitDataGridViewTextBoxColumn
            // 
            this.advertisercreditlimitDataGridViewTextBoxColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.advertisercreditlimitDataGridViewTextBoxColumn.DataPropertyName = "advertiser_credit_limit";
            dataGridViewCellStyle1.Format = "N2";
            dataGridViewCellStyle1.NullValue = null;
            this.advertisercreditlimitDataGridViewTextBoxColumn.DefaultCellStyle = dataGridViewCellStyle1;
            this.advertisercreditlimitDataGridViewTextBoxColumn.HeaderText = "Limit";
            this.advertisercreditlimitDataGridViewTextBoxColumn.Name = "advertisercreditlimitDataGridViewTextBoxColumn";
            this.advertisercreditlimitDataGridViewTextBoxColumn.ReadOnly = true;
            this.advertisercreditlimitDataGridViewTextBoxColumn.Width = 53;
            // 
            // advertiserBindingSource
            // 
            this.advertiserBindingSource.DataMember = "Advertiser";
            this.advertiserBindingSource.DataSource = this.paymentsDataSet;
            // 
            // paymentsDataSet
            // 
            this.paymentsDataSet.DataSetName = "PaymentsDataSet";
            this.paymentsDataSet.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // advertiserTableAdapter
            // 
            this.advertiserTableAdapter.ClearBeforeFill = true;
            // 
            // AdvertiserListUC
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.dataGridView1);
            this.Name = "AdvertiserListUC";
            this.Size = new System.Drawing.Size(553, 639);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.advertiserBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.paymentsDataSet)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.BindingSource advertiserBindingSource;
        private DataSets.PaymentsDataSet paymentsDataSet;
        private DataSets.PaymentsDataSetTableAdapters.AdvertiserTableAdapter advertiserTableAdapter;
        private System.Windows.Forms.DataGridViewTextBoxColumn idDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn nameDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn advertisercreditlimitcurrencyidDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn advertisercreditlimitDataGridViewTextBoxColumn;


    }
}
