namespace EomApp1.Screens.MediaBuyerWorkflow
{
    partial class MediaBuyerWorkflowSubform
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
            this.bindingSource1 = new System.Windows.Forms.BindingSource(this.components);
            this.mediaBuyerWorkflowDataSet = new EomApp1.Screens.MediaBuyerWorkflow.MediaBuyerWorkflowDataSet();
            this.publisherNameDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.mediaBuyerNameDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.mediaBuyerWorkflowDataSet)).BeginInit();
            this.SuspendLayout();
            // 
            // dataGridView1
            // 
            this.dataGridView1.AutoGenerateColumns = false;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.publisherNameDataGridViewTextBoxColumn,
            this.mediaBuyerNameDataGridViewTextBoxColumn});
            this.dataGridView1.DataSource = this.bindingSource1;
            this.dataGridView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView1.Location = new System.Drawing.Point(0, 0);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.Size = new System.Drawing.Size(835, 309);
            this.dataGridView1.TabIndex = 0;
            // 
            // bindingSource1
            // 
            this.bindingSource1.DataMember = "Publishers";
            this.bindingSource1.DataSource = this.mediaBuyerWorkflowDataSet;
            // 
            // mediaBuyerWorkflowDataSet
            // 
            this.mediaBuyerWorkflowDataSet.DataSetName = "MediaBuyerWorkflowDataSet";
            this.mediaBuyerWorkflowDataSet.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // publisherNameDataGridViewTextBoxColumn
            // 
            this.publisherNameDataGridViewTextBoxColumn.DataPropertyName = "PublisherName";
            this.publisherNameDataGridViewTextBoxColumn.HeaderText = "PublisherName";
            this.publisherNameDataGridViewTextBoxColumn.Name = "publisherNameDataGridViewTextBoxColumn";
            // 
            // mediaBuyerNameDataGridViewTextBoxColumn
            // 
            this.mediaBuyerNameDataGridViewTextBoxColumn.DataPropertyName = "MediaBuyerName";
            this.mediaBuyerNameDataGridViewTextBoxColumn.HeaderText = "MediaBuyerName";
            this.mediaBuyerNameDataGridViewTextBoxColumn.Name = "mediaBuyerNameDataGridViewTextBoxColumn";
            this.mediaBuyerNameDataGridViewTextBoxColumn.Visible = false;
            // 
            // MediaBuyerWorkflowSubform
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.dataGridView1);
            this.Name = "MediaBuyerWorkflowSubform";
            this.Size = new System.Drawing.Size(835, 309);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.mediaBuyerWorkflowDataSet)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.BindingSource bindingSource1;
        private MediaBuyerWorkflowDataSet mediaBuyerWorkflowDataSet;
        private System.Windows.Forms.DataGridViewTextBoxColumn publisherNameDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn mediaBuyerNameDataGridViewTextBoxColumn;
    }
}
