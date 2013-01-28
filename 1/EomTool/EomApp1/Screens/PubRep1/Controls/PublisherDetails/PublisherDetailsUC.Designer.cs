namespace EomApp1.Screens.PubRep1.Controls.PublisherDetails
{
    partial class PublisherDetailsUC
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
            this.publisherNotes1 = new EomApp1.Screens.PubRep1.Controls.PublisherDetails.NotesUC();
            this.publisherAttachments1 = new EomApp1.Screens.PubRep1.Controls.PublisherDetails.AttachmentsUC();
            this.pubNoteBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.dataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            ((System.ComponentModel.ISupportInitialize)(this.pubNoteBindingSource)).BeginInit();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // publisherNotes1
            // 
            this.publisherNotes1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.publisherNotes1.Location = new System.Drawing.Point(3, 3);
            this.publisherNotes1.Name = "publisherNotes1";
            this.publisherNotes1.Size = new System.Drawing.Size(408, 250);
            this.publisherNotes1.TabIndex = 0;
            // 
            // publisherAttachments1
            // 
            this.publisherAttachments1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.publisherAttachments1.Location = new System.Drawing.Point(417, 3);
            this.publisherAttachments1.Name = "publisherAttachments1";
            this.publisherAttachments1.Size = new System.Drawing.Size(409, 250);
            this.publisherAttachments1.TabIndex = 0;
            // 
            // pubNoteBindingSource
            // 
            this.pubNoteBindingSource.DataSource = typeof(EomTool.Domain.Entities.PubNote);
            // 
            // dataGridViewTextBoxColumn1
            // 
            this.dataGridViewTextBoxColumn1.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.dataGridViewTextBoxColumn1.DataPropertyName = "note";
            this.dataGridViewTextBoxColumn1.HeaderText = "Note";
            this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
            this.dataGridViewTextBoxColumn1.ReadOnly = true;
            // 
            // dataGridViewTextBoxColumn2
            // 
            this.dataGridViewTextBoxColumn2.DataPropertyName = "added_by_system_user";
            this.dataGridViewTextBoxColumn2.HeaderText = "Author";
            this.dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
            this.dataGridViewTextBoxColumn2.ReadOnly = true;
            // 
            // dataGridViewTextBoxColumn3
            // 
            this.dataGridViewTextBoxColumn3.DataPropertyName = "created";
            this.dataGridViewTextBoxColumn3.HeaderText = "Created";
            this.dataGridViewTextBoxColumn3.Name = "dataGridViewTextBoxColumn3";
            this.dataGridViewTextBoxColumn3.ReadOnly = true;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Controls.Add(this.publisherNotes1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.publisherAttachments1, 0, 0);
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(829, 256);
            this.tableLayoutPanel1.TabIndex = 1;
            // 
            // PublisherDetailsUC
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "PublisherDetailsUC";
            this.Size = new System.Drawing.Size(829, 256);
            ((System.ComponentModel.ISupportInitialize)(this.pubNoteBindingSource)).EndInit();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.BindingSource pubNoteBindingSource;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn3;
        private EomApp1.Screens.PubRep1.Controls.PublisherDetails.NotesUC publisherNotes1;
        private EomApp1.Screens.PubRep1.Controls.PublisherDetails.AttachmentsUC publisherAttachments1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
    }
}
