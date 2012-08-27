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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            this.dataGridViewExtension1 = new DataGridViewExtensions.DataGridViewExtension();
            this.bindingSource1 = new System.Windows.Forms.BindingSource(this.components);
            this.mediaBuyerWorkflowDataSet1 = new EomApp1.Screens.MediaBuyerWorkflow.MediaBuyerWorkflowDataSet();
            this.dataGridViewSubformColumn1 = new DataGridViewExtensions.DataGridViewSubformColumn();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.dataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.PublishersSubformCol = new DataGridViewExtensions.DataGridViewSubformColumn();
            this.mediaBuyerNameCol = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ApprovalActionCol = new EomAppControls.DataGrid.DataGridViewDisableButtonColumn();
            this.Amount = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.NumPublishers = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ItemIdsCol = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewExtension1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.mediaBuyerWorkflowDataSet1)).BeginInit();
            this.SuspendLayout();
            // 
            // dataGridViewExtension1
            // 
            this.dataGridViewExtension1.AllowUserToAddRows = false;
            this.dataGridViewExtension1.AllowUserToDeleteRows = false;
            this.dataGridViewExtension1.AllowUserToResizeRows = false;
            this.dataGridViewExtension1.AlternateRowColor = System.Drawing.Color.LightGray;
            this.dataGridViewExtension1.AutoGenerateColumns = false;
            this.dataGridViewExtension1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewExtension1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.PublishersSubformCol,
            this.mediaBuyerNameCol,
            this.ApprovalActionCol,
            this.Amount,
            this.NumPublishers,
            this.ItemIdsCol});
            this.dataGridViewExtension1.DataSource = this.bindingSource1;
            this.dataGridViewExtension1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridViewExtension1.Location = new System.Drawing.Point(0, 25);
            this.dataGridViewExtension1.Name = "dataGridViewExtension1";
            this.dataGridViewExtension1.ReadOnly = true;
            this.dataGridViewExtension1.ShowAlternateRowColor = false;
            this.dataGridViewExtension1.Size = new System.Drawing.Size(809, 467);
            this.dataGridViewExtension1.SubformsScrollHorizontally = true;
            this.dataGridViewExtension1.TabIndex = 0;
            // 
            // bindingSource1
            // 
            this.bindingSource1.DataMember = "MediaBuyers";
            this.bindingSource1.DataSource = this.mediaBuyerWorkflowDataSet1;
            // 
            // mediaBuyerWorkflowDataSet1
            // 
            this.mediaBuyerWorkflowDataSet1.DataSetName = "MediaBuyerWorkflowDataSet";
            this.mediaBuyerWorkflowDataSet1.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // dataGridViewSubformColumn1
            // 
            this.dataGridViewSubformColumn1.HeaderText = "";
            this.dataGridViewSubformColumn1.Name = "dataGridViewSubformColumn1";
            this.dataGridViewSubformColumn1.ReadOnly = true;
            this.dataGridViewSubformColumn1.Subform = null;
            this.dataGridViewSubformColumn1.Width = 30;
            // 
            // toolStrip1
            // 
            this.toolStrip1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(809, 25);
            this.toolStrip1.TabIndex = 1;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // dataGridViewTextBoxColumn1
            // 
            this.dataGridViewTextBoxColumn1.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.dataGridViewTextBoxColumn1.DataPropertyName = "NumPublishers";
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.dataGridViewTextBoxColumn1.DefaultCellStyle = dataGridViewCellStyle5;
            this.dataGridViewTextBoxColumn1.HeaderText = "NumPublishers";
            this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
            this.dataGridViewTextBoxColumn1.ReadOnly = true;
            // 
            // dataGridViewTextBoxColumn2
            // 
            this.dataGridViewTextBoxColumn2.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.dataGridViewTextBoxColumn2.DataPropertyName = "NumPublishers";
            dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.dataGridViewTextBoxColumn2.DefaultCellStyle = dataGridViewCellStyle6;
            this.dataGridViewTextBoxColumn2.HeaderText = "#Publishers";
            this.dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
            this.dataGridViewTextBoxColumn2.ReadOnly = true;
            // 
            // dataGridViewTextBoxColumn3
            // 
            this.dataGridViewTextBoxColumn3.DataPropertyName = "ItemIds";
            this.dataGridViewTextBoxColumn3.HeaderText = "ItemIds";
            this.dataGridViewTextBoxColumn3.Name = "dataGridViewTextBoxColumn3";
            this.dataGridViewTextBoxColumn3.ReadOnly = true;
            this.dataGridViewTextBoxColumn3.Visible = false;
            // 
            // PublishersSubformCol
            // 
            this.PublishersSubformCol.HeaderText = "";
            this.PublishersSubformCol.Name = "PublishersSubformCol";
            this.PublishersSubformCol.ReadOnly = true;
            this.PublishersSubformCol.Subform = null;
            this.PublishersSubformCol.Width = 30;
            // 
            // mediaBuyerNameCol
            // 
            this.mediaBuyerNameCol.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.mediaBuyerNameCol.DataPropertyName = "MediaBuyerName";
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.Color.Black;
            this.mediaBuyerNameCol.DefaultCellStyle = dataGridViewCellStyle1;
            this.mediaBuyerNameCol.HeaderText = "Media Buyer";
            this.mediaBuyerNameCol.Name = "mediaBuyerNameCol";
            this.mediaBuyerNameCol.ReadOnly = true;
            this.mediaBuyerNameCol.Width = 91;
            // 
            // ApprovalActionCol
            // 
            this.ApprovalActionCol.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.ApprovalActionCol.DefaultCellStyle = dataGridViewCellStyle2;
            this.ApprovalActionCol.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ApprovalActionCol.HeaderText = "Approval";
            this.ApprovalActionCol.Name = "ApprovalActionCol";
            this.ApprovalActionCol.ReadOnly = true;
            this.ApprovalActionCol.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.ApprovalActionCol.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.ApprovalActionCol.Text = "Action";
            this.ApprovalActionCol.UseColumnTextForButtonValue = true;
            this.ApprovalActionCol.Width = 74;
            // 
            // Amount
            // 
            this.Amount.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.Amount.DataPropertyName = "Amount";
            dataGridViewCellStyle3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle3.Format = "N2";
            dataGridViewCellStyle3.NullValue = null;
            this.Amount.DefaultCellStyle = dataGridViewCellStyle3;
            this.Amount.HeaderText = "Amount";
            this.Amount.Name = "Amount";
            this.Amount.ReadOnly = true;
            this.Amount.Width = 68;
            // 
            // NumPublishers
            // 
            this.NumPublishers.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.NumPublishers.DataPropertyName = "NumPublishers";
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            this.NumPublishers.DefaultCellStyle = dataGridViewCellStyle4;
            this.NumPublishers.HeaderText = "#Pubs";
            this.NumPublishers.Name = "NumPublishers";
            this.NumPublishers.ReadOnly = true;
            // 
            // ItemIdsCol
            // 
            this.ItemIdsCol.DataPropertyName = "ItemIds";
            this.ItemIdsCol.HeaderText = "ItemIds";
            this.ItemIdsCol.Name = "ItemIdsCol";
            this.ItemIdsCol.ReadOnly = true;
            this.ItemIdsCol.Visible = false;
            // 
            // MediaBuyerWorkflowForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(809, 492);
            this.Controls.Add(this.dataGridViewExtension1);
            this.Controls.Add(this.toolStrip1);
            this.Name = "MediaBuyerWorkflowForm";
            this.ShowIcon = false;
            this.Text = "Approvals";
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewExtension1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.mediaBuyerWorkflowDataSet1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DataGridViewExtension dataGridViewExtension1;
        private System.Windows.Forms.DataGridViewTextBoxColumn nameDataGridViewTextBoxColumn;
        private System.Windows.Forms.BindingSource bindingSource1;
        private MediaBuyerWorkflowDataSet mediaBuyerWorkflowDataSet1;
        private DataGridViewSubformColumn dataGridViewSubformColumn1;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn3;
        private DataGridViewSubformColumn PublishersSubformCol;
        private System.Windows.Forms.DataGridViewTextBoxColumn mediaBuyerNameCol;
        private EomAppControls.DataGrid.DataGridViewDisableButtonColumn ApprovalActionCol;
        private System.Windows.Forms.DataGridViewTextBoxColumn Amount;
        private System.Windows.Forms.DataGridViewTextBoxColumn NumPublishers;
        private System.Windows.Forms.DataGridViewTextBoxColumn ItemIdsCol;
    }
}