namespace LineCommander
{
    partial class MainForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.commandsDataGridView = new System.Windows.Forms.DataGridView();
            this.dataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.commandsBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.dataSet1 = new LineCommander.DataSet1();
            this.commandParametersDataGridView = new System.Windows.Forms.DataGridView();
            this.dataGridViewTextBoxColumn2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ParameterType = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.commandParametersBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.toolStripButton1 = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton2 = new System.Windows.Forms.ToolStripButton();
            this.saveToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.runningCommandsDataGridView = new System.Windows.Forms.DataGridView();
            this.runningCommandsBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.runtimeDataSetBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.runtimeDataSet = new LineCommander.RuntimeDataSet();
            this.dataGridViewTextBoxColumn5 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn6 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.RunStateColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.commandsDataGridView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.commandsBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataSet1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.commandParametersDataGridView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.commandParametersBindingSource)).BeginInit();
            this.toolStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.runningCommandsDataGridView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.runningCommandsBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.runtimeDataSetBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.runtimeDataSet)).BeginInit();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.AutoScroll = true;
            this.splitContainer1.Panel1.Controls.Add(this.commandsDataGridView);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.commandParametersDataGridView);
            this.splitContainer1.Size = new System.Drawing.Size(1064, 496);
            this.splitContainer1.SplitterDistance = 354;
            this.splitContainer1.TabIndex = 1;
            // 
            // commandsDataGridView
            // 
            this.commandsDataGridView.AllowUserToAddRows = false;
            this.commandsDataGridView.AllowUserToDeleteRows = false;
            this.commandsDataGridView.AllowUserToResizeColumns = false;
            this.commandsDataGridView.AllowUserToResizeRows = false;
            this.commandsDataGridView.AutoGenerateColumns = false;
            this.commandsDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.commandsDataGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dataGridViewTextBoxColumn1});
            this.commandsDataGridView.DataSource = this.commandsBindingSource;
            this.commandsDataGridView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.commandsDataGridView.Location = new System.Drawing.Point(0, 0);
            this.commandsDataGridView.MultiSelect = false;
            this.commandsDataGridView.Name = "commandsDataGridView";
            this.commandsDataGridView.ReadOnly = true;
            this.commandsDataGridView.RowHeadersVisible = false;
            this.commandsDataGridView.Size = new System.Drawing.Size(354, 496);
            this.commandsDataGridView.TabIndex = 0;
            // 
            // dataGridViewTextBoxColumn1
            // 
            this.dataGridViewTextBoxColumn1.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.dataGridViewTextBoxColumn1.DataPropertyName = "CommandName";
            this.dataGridViewTextBoxColumn1.HeaderText = "CommandName";
            this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
            this.dataGridViewTextBoxColumn1.ReadOnly = true;
            // 
            // commandsBindingSource
            // 
            this.commandsBindingSource.DataMember = "Commands";
            this.commandsBindingSource.DataSource = this.dataSet1;
            // 
            // dataSet1
            // 
            this.dataSet1.DataSetName = "DataSet1";
            this.dataSet1.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // commandParametersDataGridView
            // 
            this.commandParametersDataGridView.AllowUserToAddRows = false;
            this.commandParametersDataGridView.AllowUserToDeleteRows = false;
            this.commandParametersDataGridView.AllowUserToResizeColumns = false;
            this.commandParametersDataGridView.AllowUserToResizeRows = false;
            this.commandParametersDataGridView.AutoGenerateColumns = false;
            this.commandParametersDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.commandParametersDataGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dataGridViewTextBoxColumn2,
            this.dataGridViewTextBoxColumn3,
            this.dataGridViewTextBoxColumn4,
            this.ParameterType});
            this.commandParametersDataGridView.DataSource = this.commandParametersBindingSource;
            this.commandParametersDataGridView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.commandParametersDataGridView.Location = new System.Drawing.Point(0, 0);
            this.commandParametersDataGridView.MultiSelect = false;
            this.commandParametersDataGridView.Name = "commandParametersDataGridView";
            this.commandParametersDataGridView.RowHeadersVisible = false;
            this.commandParametersDataGridView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            this.commandParametersDataGridView.Size = new System.Drawing.Size(706, 496);
            this.commandParametersDataGridView.TabIndex = 0;
            // 
            // dataGridViewTextBoxColumn2
            // 
            this.dataGridViewTextBoxColumn2.DataPropertyName = "CommandName";
            this.dataGridViewTextBoxColumn2.HeaderText = "CommandName";
            this.dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
            this.dataGridViewTextBoxColumn2.Visible = false;
            // 
            // dataGridViewTextBoxColumn3
            // 
            this.dataGridViewTextBoxColumn3.DataPropertyName = "ParameterName";
            this.dataGridViewTextBoxColumn3.HeaderText = "ParameterName";
            this.dataGridViewTextBoxColumn3.Name = "dataGridViewTextBoxColumn3";
            this.dataGridViewTextBoxColumn3.ReadOnly = true;
            // 
            // dataGridViewTextBoxColumn4
            // 
            this.dataGridViewTextBoxColumn4.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.dataGridViewTextBoxColumn4.DataPropertyName = "ParameterValue";
            this.dataGridViewTextBoxColumn4.HeaderText = "ParameterValue";
            this.dataGridViewTextBoxColumn4.Name = "dataGridViewTextBoxColumn4";
            // 
            // ParameterType
            // 
            this.ParameterType.DataPropertyName = "ParameterType";
            this.ParameterType.HeaderText = "ParameterType";
            this.ParameterType.Name = "ParameterType";
            // 
            // commandParametersBindingSource
            // 
            this.commandParametersBindingSource.DataMember = "Commands_CommandParameters";
            this.commandParametersBindingSource.DataSource = this.commandsBindingSource;
            // 
            // toolStrip1
            // 
            this.toolStrip1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButton1,
            this.toolStripButton2,
            this.saveToolStripButton});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(1064, 25);
            this.toolStrip1.TabIndex = 1;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // toolStripButton1
            // 
            this.toolStripButton1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripButton1.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton1.Image")));
            this.toolStripButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton1.Name = "toolStripButton1";
            this.toolStripButton1.Size = new System.Drawing.Size(32, 22);
            this.toolStripButton1.Text = "&Run";
            this.toolStripButton1.Click += new System.EventHandler(this.toolStripButton1_RunClicked);
            // 
            // toolStripButton2
            // 
            this.toolStripButton2.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripButton2.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton2.Image")));
            this.toolStripButton2.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton2.Name = "toolStripButton2";
            this.toolStripButton2.Size = new System.Drawing.Size(37, 22);
            this.toolStripButton2.Text = "&Load";
            this.toolStripButton2.Click += new System.EventHandler(this.toolStripButton2_Click);
            // 
            // saveToolStripButton
            // 
            this.saveToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.saveToolStripButton.Image = ((System.Drawing.Image)(resources.GetObject("saveToolStripButton.Image")));
            this.saveToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.saveToolStripButton.Name = "saveToolStripButton";
            this.saveToolStripButton.Size = new System.Drawing.Size(35, 22);
            this.saveToolStripButton.Text = "&Save";
            this.saveToolStripButton.Click += new System.EventHandler(this.saveToolStripButton_Click);
            // 
            // splitContainer2
            // 
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer2.Location = new System.Drawing.Point(0, 25);
            this.splitContainer2.Name = "splitContainer2";
            this.splitContainer2.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add(this.splitContainer1);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.AutoScroll = true;
            this.splitContainer2.Panel2.Controls.Add(this.runningCommandsDataGridView);
            this.splitContainer2.Size = new System.Drawing.Size(1064, 686);
            this.splitContainer2.SplitterDistance = 496;
            this.splitContainer2.TabIndex = 2;
            // 
            // runningCommandsDataGridView
            // 
            this.runningCommandsDataGridView.AllowUserToAddRows = false;
            this.runningCommandsDataGridView.AllowUserToDeleteRows = false;
            this.runningCommandsDataGridView.AutoGenerateColumns = false;
            this.runningCommandsDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.runningCommandsDataGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dataGridViewTextBoxColumn5,
            this.dataGridViewTextBoxColumn6,
            this.RunStateColumn});
            this.runningCommandsDataGridView.DataSource = this.runningCommandsBindingSource;
            this.runningCommandsDataGridView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.runningCommandsDataGridView.Location = new System.Drawing.Point(0, 0);
            this.runningCommandsDataGridView.Name = "runningCommandsDataGridView";
            this.runningCommandsDataGridView.ReadOnly = true;
            this.runningCommandsDataGridView.Size = new System.Drawing.Size(1064, 186);
            this.runningCommandsDataGridView.TabIndex = 1;
            // 
            // runningCommandsBindingSource
            // 
            this.runningCommandsBindingSource.DataMember = "RunningCommands";
            this.runningCommandsBindingSource.DataSource = this.runtimeDataSetBindingSource;
            // 
            // runtimeDataSetBindingSource
            // 
            this.runtimeDataSetBindingSource.DataSource = this.runtimeDataSet;
            this.runtimeDataSetBindingSource.Position = 0;
            // 
            // runtimeDataSet
            // 
            this.runtimeDataSet.DataSetName = "RuntimeDataSet";
            this.runtimeDataSet.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // dataGridViewTextBoxColumn5
            // 
            this.dataGridViewTextBoxColumn5.DataPropertyName = "TaskGuid";
            this.dataGridViewTextBoxColumn5.HeaderText = "TaskGuid";
            this.dataGridViewTextBoxColumn5.Name = "dataGridViewTextBoxColumn5";
            this.dataGridViewTextBoxColumn5.ReadOnly = true;
            // 
            // dataGridViewTextBoxColumn6
            // 
            this.dataGridViewTextBoxColumn6.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.dataGridViewTextBoxColumn6.DataPropertyName = "Description";
            this.dataGridViewTextBoxColumn6.HeaderText = "Description";
            this.dataGridViewTextBoxColumn6.Name = "dataGridViewTextBoxColumn6";
            this.dataGridViewTextBoxColumn6.ReadOnly = true;
            // 
            // RunStateColumn
            // 
            this.RunStateColumn.DataPropertyName = "RunState";
            this.RunStateColumn.HeaderText = "RunState";
            this.RunStateColumn.Name = "RunStateColumn";
            this.RunStateColumn.ReadOnly = true;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1064, 711);
            this.Controls.Add(this.splitContainer2);
            this.Controls.Add(this.toolStrip1);
            this.Name = "MainForm";
            this.ShowIcon = false;
            this.Text = "Line Commander";
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.commandsDataGridView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.commandsBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataSet1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.commandParametersDataGridView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.commandParametersBindingSource)).EndInit();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
            this.splitContainer2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.runningCommandsDataGridView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.runningCommandsBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.runtimeDataSetBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.runtimeDataSet)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.BindingSource commandsBindingSource;
        private DataSet1 dataSet1;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.DataGridView commandsDataGridView;
        private System.Windows.Forms.DataGridView commandParametersDataGridView;
        private System.Windows.Forms.BindingSource commandParametersBindingSource;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton toolStripButton1;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn3;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn4;
        private System.Windows.Forms.DataGridViewTextBoxColumn ParameterType;
        private System.Windows.Forms.ToolStripButton saveToolStripButton;
        private System.Windows.Forms.ToolStripButton toolStripButton2;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private System.Windows.Forms.BindingSource runningCommandsBindingSource;
        private System.Windows.Forms.BindingSource runtimeDataSetBindingSource;
        private RuntimeDataSet runtimeDataSet;
        private System.Windows.Forms.DataGridView runningCommandsDataGridView;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn5;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn6;
        private System.Windows.Forms.DataGridViewTextBoxColumn RunStateColumn;


    }
}

