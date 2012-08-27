using DataGridViewExtensions;
namespace EomApp1.Screens.MediaBuyerWorkflow
{
    partial class PublishersSubform
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
            this.dataGridView1 = new DataGridViewExtensions.DataGridViewExtension();
            this.PublisherPayoutsSubform = new DataGridViewExtensions.DataGridViewSubformColumn();
            this.publisherNameDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.mediaBuyerNameDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.bindingSource1 = new System.Windows.Forms.BindingSource(this.components);
            this.mediaBuyerWorkflowDataSet1 = new EomApp1.Screens.MediaBuyerWorkflow.MediaBuyerWorkflowDataSet();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.mediaBuyerWorkflowDataSet1)).BeginInit();
            this.SuspendLayout();
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.AlternateRowColor = System.Drawing.Color.LightGray;
            this.dataGridView1.AutoGenerateColumns = false;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.PublisherPayoutsSubform,
            this.publisherNameDataGridViewTextBoxColumn,
            this.mediaBuyerNameDataGridViewTextBoxColumn});
            this.dataGridView1.DataSource = this.bindingSource1;
            this.dataGridView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView1.Location = new System.Drawing.Point(0, 0);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.ReadOnly = true;
            this.dataGridView1.ShowAlternateRowColor = false;
            this.dataGridView1.Size = new System.Drawing.Size(1020, 356);
            this.dataGridView1.SubformsScrollHorizontally = true;
            this.dataGridView1.TabIndex = 0;
            // 
            // PublisherPayoutsSubform
            // 
            this.PublisherPayoutsSubform.HeaderText = "";
            this.PublisherPayoutsSubform.Name = "PublisherPayoutsSubform";
            this.PublisherPayoutsSubform.ReadOnly = true;
            this.PublisherPayoutsSubform.Subform = null;
            this.PublisherPayoutsSubform.Width = 30;
            // 
            // publisherNameDataGridViewTextBoxColumn
            // 
            this.publisherNameDataGridViewTextBoxColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.publisherNameDataGridViewTextBoxColumn.DataPropertyName = "PublisherName";
            this.publisherNameDataGridViewTextBoxColumn.HeaderText = "PublisherName";
            this.publisherNameDataGridViewTextBoxColumn.Name = "publisherNameDataGridViewTextBoxColumn";
            this.publisherNameDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // mediaBuyerNameDataGridViewTextBoxColumn
            // 
            this.mediaBuyerNameDataGridViewTextBoxColumn.DataPropertyName = "MediaBuyerName";
            this.mediaBuyerNameDataGridViewTextBoxColumn.HeaderText = "MediaBuyerName";
            this.mediaBuyerNameDataGridViewTextBoxColumn.Name = "mediaBuyerNameDataGridViewTextBoxColumn";
            this.mediaBuyerNameDataGridViewTextBoxColumn.ReadOnly = true;
            this.mediaBuyerNameDataGridViewTextBoxColumn.Visible = false;
            // 
            // bindingSource1
            // 
            this.bindingSource1.DataMember = "Publishers";
            this.bindingSource1.DataSource = this.mediaBuyerWorkflowDataSet1;
            // 
            // mediaBuyerWorkflowDataSet1
            // 
            this.mediaBuyerWorkflowDataSet1.DataSetName = "MediaBuyerWorkflowDataSet";
            this.mediaBuyerWorkflowDataSet1.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // PublishersSubform
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.Controls.Add(this.dataGridView1);
            this.Name = "PublishersSubform";
            this.Size = new System.Drawing.Size(1020, 356);
            this.Load += new System.EventHandler(this.MediaBuyerWorkflowSubform_Load_1);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.mediaBuyerWorkflowDataSet1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DataGridViewExtension dataGridView1;
        private System.Windows.Forms.BindingSource bindingSource1;
        private MediaBuyerWorkflowDataSet mediaBuyerWorkflowDataSet1;
        private DataGridViewSubformColumn PublisherPayoutsSubform;
        private System.Windows.Forms.DataGridViewTextBoxColumn publisherNameDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn mediaBuyerNameDataGridViewTextBoxColumn;
    }
}
