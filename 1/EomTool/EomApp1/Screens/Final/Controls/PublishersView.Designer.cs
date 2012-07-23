namespace EomApp1.Screens.Final.Controls
{
    partial class PublishersView
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PublishersView));
            this.grid = new System.Windows.Forms.DataGridView();
            this.publisherDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.currDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.revenueDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.NetTermsCol = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.finalizePublishersBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.finalizePublishersDataSet = new EomApp1.Screens.Final.Data.PublishersDataSet();
            this.toolStripContainer1 = new System.Windows.Forms.ToolStripContainer();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.toolStripLabel1 = new System.Windows.Forms.ToolStripLabel();
            this.selectAllButton = new System.Windows.Forms.ToolStripButton();
            this.selectNoneButton = new System.Windows.Forms.ToolStripButton();
            this.actionButton = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.netTermsLabel = new System.Windows.Forms.ToolStripLabel();
            this.netTermsComboBox = new System.Windows.Forms.ToolStripComboBox();
            this.dataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.grid)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.finalizePublishersBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.finalizePublishersDataSet)).BeginInit();
            this.toolStripContainer1.ContentPanel.SuspendLayout();
            this.toolStripContainer1.TopToolStripPanel.SuspendLayout();
            this.toolStripContainer1.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // grid
            // 
            this.grid.AllowUserToAddRows = false;
            this.grid.AllowUserToDeleteRows = false;
            this.grid.AllowUserToResizeRows = false;
            this.grid.AutoGenerateColumns = false;
            this.grid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.grid.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.publisherDataGridViewTextBoxColumn,
            this.currDataGridViewTextBoxColumn,
            this.revenueDataGridViewTextBoxColumn,
            this.NetTermsCol});
            this.grid.DataSource = this.finalizePublishersBindingSource;
            this.grid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grid.Location = new System.Drawing.Point(0, 0);
            this.grid.Name = "grid";
            this.grid.RowHeadersVisible = false;
            this.grid.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.grid.Size = new System.Drawing.Size(818, 123);
            this.grid.TabIndex = 0;
            this.grid.SelectionChanged += new System.EventHandler(this.SelectionChanged);
            // 
            // publisherDataGridViewTextBoxColumn
            // 
            this.publisherDataGridViewTextBoxColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.publisherDataGridViewTextBoxColumn.DataPropertyName = "Publisher";
            this.publisherDataGridViewTextBoxColumn.HeaderText = "Publisher";
            this.publisherDataGridViewTextBoxColumn.Name = "publisherDataGridViewTextBoxColumn";
            this.publisherDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // currDataGridViewTextBoxColumn
            // 
            this.currDataGridViewTextBoxColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.DisplayedCells;
            this.currDataGridViewTextBoxColumn.DataPropertyName = "Curr";
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.currDataGridViewTextBoxColumn.DefaultCellStyle = dataGridViewCellStyle1;
            this.currDataGridViewTextBoxColumn.HeaderText = "Curr";
            this.currDataGridViewTextBoxColumn.Name = "currDataGridViewTextBoxColumn";
            this.currDataGridViewTextBoxColumn.ReadOnly = true;
            this.currDataGridViewTextBoxColumn.Width = 51;
            // 
            // revenueDataGridViewTextBoxColumn
            // 
            this.revenueDataGridViewTextBoxColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.DisplayedCells;
            this.revenueDataGridViewTextBoxColumn.DataPropertyName = "Revenue";
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.Format = "N2";
            dataGridViewCellStyle2.NullValue = null;
            this.revenueDataGridViewTextBoxColumn.DefaultCellStyle = dataGridViewCellStyle2;
            this.revenueDataGridViewTextBoxColumn.HeaderText = "Revenue";
            this.revenueDataGridViewTextBoxColumn.Name = "revenueDataGridViewTextBoxColumn";
            this.revenueDataGridViewTextBoxColumn.ReadOnly = true;
            this.revenueDataGridViewTextBoxColumn.Width = 76;
            // 
            // NetTermsCol
            // 
            this.NetTermsCol.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.NetTermsCol.DataPropertyName = "NetTerms";
            this.NetTermsCol.HeaderText = "Net Terms";
            this.NetTermsCol.Name = "NetTermsCol";
            this.NetTermsCol.ReadOnly = true;
            this.NetTermsCol.Width = 81;
            // 
            // finalizePublishersBindingSource
            // 
            this.finalizePublishersBindingSource.DataMember = "Publishers";
            this.finalizePublishersBindingSource.DataSource = this.finalizePublishersDataSet;
            // 
            // finalizePublishersDataSet
            // 
            this.finalizePublishersDataSet.DataSetName = "PublishersDataSet";
            this.finalizePublishersDataSet.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // toolStripContainer1
            // 
            this.toolStripContainer1.BottomToolStripPanelVisible = false;
            // 
            // toolStripContainer1.ContentPanel
            // 
            this.toolStripContainer1.ContentPanel.Controls.Add(this.grid);
            this.toolStripContainer1.ContentPanel.Size = new System.Drawing.Size(818, 123);
            this.toolStripContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.toolStripContainer1.LeftToolStripPanelVisible = false;
            this.toolStripContainer1.Location = new System.Drawing.Point(0, 0);
            this.toolStripContainer1.Name = "toolStripContainer1";
            this.toolStripContainer1.RightToolStripPanelVisible = false;
            this.toolStripContainer1.Size = new System.Drawing.Size(818, 273);
            this.toolStripContainer1.TabIndex = 1;
            this.toolStripContainer1.Text = "toolStripContainer1";
            // 
            // toolStripContainer1.TopToolStripPanel
            // 
            this.toolStripContainer1.TopToolStripPanel.Controls.Add(this.toolStrip1);
            // 
            // toolStrip1
            // 
            this.toolStrip1.AutoSize = false;
            this.toolStrip1.Dock = System.Windows.Forms.DockStyle.None;
            this.toolStrip1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripLabel1,
            this.selectAllButton,
            this.selectNoneButton,
            this.actionButton,
            this.toolStripSeparator1,
            this.netTermsLabel,
            this.netTermsComboBox});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(818, 150);
            this.toolStrip1.Stretch = true;
            this.toolStrip1.TabIndex = 0;
            // 
            // toolStripLabel1
            // 
            this.toolStripLabel1.Name = "toolStripLabel1";
            this.toolStripLabel1.Size = new System.Drawing.Size(41, 147);
            this.toolStripLabel1.Text = "Select:";
            // 
            // selectAllButton
            // 
            this.selectAllButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.selectAllButton.Image = ((System.Drawing.Image)(resources.GetObject("selectAllButton.Image")));
            this.selectAllButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.selectAllButton.Name = "selectAllButton";
            this.selectAllButton.Size = new System.Drawing.Size(25, 147);
            this.selectAllButton.Text = "All";
            this.selectAllButton.Click += new System.EventHandler(this.SelectAllClicked);
            // 
            // selectNoneButton
            // 
            this.selectNoneButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.selectNoneButton.Image = ((System.Drawing.Image)(resources.GetObject("selectNoneButton.Image")));
            this.selectNoneButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.selectNoneButton.Name = "selectNoneButton";
            this.selectNoneButton.Size = new System.Drawing.Size(40, 147);
            this.selectNoneButton.Text = "None";
            this.selectNoneButton.Click += new System.EventHandler(this.SelectNoneClicked);
            // 
            // actionButton
            // 
            this.actionButton.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.actionButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.actionButton.Enabled = false;
            this.actionButton.Image = ((System.Drawing.Image)(resources.GetObject("actionButton.Image")));
            this.actionButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.actionButton.Name = "actionButton";
            this.actionButton.Size = new System.Drawing.Size(46, 147);
            this.actionButton.Text = "Action";
            this.actionButton.Click += new System.EventHandler(this.ActionButtonClicked);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 150);
            // 
            // netTermsLabel
            // 
            this.netTermsLabel.Name = "netTermsLabel";
            this.netTermsLabel.Size = new System.Drawing.Size(62, 147);
            this.netTermsLabel.Text = "Net Terms";
            // 
            // netTermsComboBox
            // 
            this.netTermsComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.netTermsComboBox.Name = "netTermsComboBox";
            this.netTermsComboBox.Size = new System.Drawing.Size(75, 150);
            this.netTermsComboBox.SelectedIndexChanged += new System.EventHandler(this.netTermsComboBox_SelectedIndexChanged);
            // 
            // dataGridViewTextBoxColumn1
            // 
            this.dataGridViewTextBoxColumn1.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.dataGridViewTextBoxColumn1.DataPropertyName = "NetTerms";
            this.dataGridViewTextBoxColumn1.HeaderText = "Net Terms";
            this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
            this.dataGridViewTextBoxColumn1.ReadOnly = true;
            // 
            // PublishersView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.toolStripContainer1);
            this.Name = "PublishersView";
            this.Size = new System.Drawing.Size(818, 273);
            ((System.ComponentModel.ISupportInitialize)(this.grid)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.finalizePublishersBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.finalizePublishersDataSet)).EndInit();
            this.toolStripContainer1.ContentPanel.ResumeLayout(false);
            this.toolStripContainer1.TopToolStripPanel.ResumeLayout(false);
            this.toolStripContainer1.ResumeLayout(false);
            this.toolStripContainer1.PerformLayout();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView grid;
        private System.Windows.Forms.BindingSource finalizePublishersBindingSource;
        private Data.PublishersDataSet finalizePublishersDataSet;
        private System.Windows.Forms.ToolStripContainer toolStripContainer1;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton selectAllButton;
        private System.Windows.Forms.ToolStripButton selectNoneButton;
        private System.Windows.Forms.ToolStripButton actionButton;
        private System.Windows.Forms.ToolStripLabel toolStripLabel1;
        private System.Windows.Forms.DataGridViewTextBoxColumn publisherDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn currDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn revenueDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn NetTermsCol;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripLabel netTermsLabel;
        private System.Windows.Forms.ToolStripComboBox netTermsComboBox;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
    }
}
