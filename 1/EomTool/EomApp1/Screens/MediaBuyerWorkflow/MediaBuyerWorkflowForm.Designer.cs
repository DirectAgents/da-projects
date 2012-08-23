using DataGridViewExtensions;
namespace EomApp1.Screens.MediaBuyerWorkflow
{
    partial class MediaBuyerWorkflowForm
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
            this.dataGridViewExtension1 = new DataGridViewExtensions.DataGridViewExtension();
            this.mediaBuyerWorkflowDataSet1 = new EomApp1.Screens.MediaBuyerWorkflow.MediaBuyerWorkflowDataSet();
            this.bindingSource1 = new System.Windows.Forms.BindingSource(this.components);
            this.nameDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewExtension1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.mediaBuyerWorkflowDataSet1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource1)).BeginInit();
            this.SuspendLayout();
            // 
            // dataGridViewExtension1
            // 
            this.dataGridViewExtension1.AlternateRowColor = System.Drawing.Color.LightGray;
            this.dataGridViewExtension1.AutoGenerateColumns = false;
            this.dataGridViewExtension1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewExtension1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.nameDataGridViewTextBoxColumn});
            this.dataGridViewExtension1.DataSource = this.bindingSource1;
            this.dataGridViewExtension1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridViewExtension1.Location = new System.Drawing.Point(0, 0);
            this.dataGridViewExtension1.Name = "dataGridViewExtension1";
            this.dataGridViewExtension1.Size = new System.Drawing.Size(1248, 592);
            this.dataGridViewExtension1.TabIndex = 0;
            // 
            // mediaBuyerWorkflowDataSet1
            // 
            this.mediaBuyerWorkflowDataSet1.DataSetName = "MediaBuyerWorkflowDataSet";
            this.mediaBuyerWorkflowDataSet1.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // bindingSource1
            // 
            this.bindingSource1.DataMember = "MediaBuyers";
            this.bindingSource1.DataSource = this.mediaBuyerWorkflowDataSet1;
            // 
            // nameDataGridViewTextBoxColumn
            // 
            this.nameDataGridViewTextBoxColumn.DataPropertyName = "Name";
            this.nameDataGridViewTextBoxColumn.HeaderText = "Name";
            this.nameDataGridViewTextBoxColumn.Name = "nameDataGridViewTextBoxColumn";
            // 
            // MediaBuyerWorkflowForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1248, 592);
            this.Controls.Add(this.dataGridViewExtension1);
            this.Name = "MediaBuyerWorkflowForm";
            this.Text = "MediaBuyerWorkflowForm";
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewExtension1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.mediaBuyerWorkflowDataSet1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DataGridViewExtension dataGridViewExtension1;
        private System.Windows.Forms.DataGridViewTextBoxColumn nameDataGridViewTextBoxColumn;
        private System.Windows.Forms.BindingSource bindingSource1;
        private MediaBuyerWorkflowDataSet mediaBuyerWorkflowDataSet1;
    }
}