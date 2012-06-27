namespace EomApp1.Formss.Settings
{
    partial class SelectDatabaseForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SelectDatabaseForm));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            this.settingsDataSet1 = new EomApp1.Formss.Settings.SettingsDataSet1();
            this.dADatabaseBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.dADatabaseTableAdapter = new EomApp1.Formss.Settings.SettingsDataSet1TableAdapters.DADatabaseTableAdapter();
            this.tableAdapterManager = new EomApp1.Formss.Settings.SettingsDataSet1TableAdapters.TableAdapterManager();
            this.dADatabaseBindingNavigator = new System.Windows.Forms.BindingNavigator(this.components);
            this.toolStripButton1 = new System.Windows.Forms.ToolStripButton();
            this.dADatabaseDataGridView = new System.Windows.Forms.DataGridView();
            this.dataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.settingsDataSet1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dADatabaseBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dADatabaseBindingNavigator)).BeginInit();
            this.dADatabaseBindingNavigator.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dADatabaseDataGridView)).BeginInit();
            this.SuspendLayout();
            // 
            // settingsDataSet1
            // 
            this.settingsDataSet1.DataSetName = "SettingsDataSet1";
            this.settingsDataSet1.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // dADatabaseBindingSource
            // 
            this.dADatabaseBindingSource.DataMember = "DADatabase";
            this.dADatabaseBindingSource.DataSource = this.settingsDataSet1;
            // 
            // dADatabaseTableAdapter
            // 
            this.dADatabaseTableAdapter.ClearBeforeFill = true;
            // 
            // tableAdapterManager
            // 
            this.tableAdapterManager.BackupDataSetBeforeUpdate = false;
            this.tableAdapterManager.DADatabaseTableAdapter = this.dADatabaseTableAdapter;
            this.tableAdapterManager.UpdateOrder = EomApp1.Formss.Settings.SettingsDataSet1TableAdapters.TableAdapterManager.UpdateOrderOption.InsertUpdateDelete;
            // 
            // dADatabaseBindingNavigator
            // 
            this.dADatabaseBindingNavigator.AddNewItem = null;
            this.dADatabaseBindingNavigator.BindingSource = this.dADatabaseBindingSource;
            this.dADatabaseBindingNavigator.CountItem = null;
            this.dADatabaseBindingNavigator.DeleteItem = null;
            this.dADatabaseBindingNavigator.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.dADatabaseBindingNavigator.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.dADatabaseBindingNavigator.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButton1});
            this.dADatabaseBindingNavigator.Location = new System.Drawing.Point(0, 222);
            this.dADatabaseBindingNavigator.MoveFirstItem = null;
            this.dADatabaseBindingNavigator.MoveLastItem = null;
            this.dADatabaseBindingNavigator.MoveNextItem = null;
            this.dADatabaseBindingNavigator.MovePreviousItem = null;
            this.dADatabaseBindingNavigator.Name = "dADatabaseBindingNavigator";
            this.dADatabaseBindingNavigator.PositionItem = null;
            this.dADatabaseBindingNavigator.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.dADatabaseBindingNavigator.Size = new System.Drawing.Size(172, 25);
            this.dADatabaseBindingNavigator.TabIndex = 0;
            this.dADatabaseBindingNavigator.Text = "bindingNavigator1";
            // 
            // toolStripButton1
            // 
            this.toolStripButton1.BackColor = System.Drawing.Color.Chartreuse;
            this.toolStripButton1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripButton1.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.toolStripButton1.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton1.Image")));
            this.toolStripButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton1.Name = "toolStripButton1";
            this.toolStripButton1.Size = new System.Drawing.Size(113, 22);
            this.toolStripButton1.Text = "Select and Restart";
            this.toolStripButton1.Click += new System.EventHandler(this.toolStripButton1_Click);
            // 
            // dADatabaseDataGridView
            // 
            this.dADatabaseDataGridView.AllowUserToAddRows = false;
            this.dADatabaseDataGridView.AllowUserToDeleteRows = false;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.dADatabaseDataGridView.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.dADatabaseDataGridView.AutoGenerateColumns = false;
            this.dADatabaseDataGridView.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dADatabaseDataGridView.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.None;
            this.dADatabaseDataGridView.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            this.dADatabaseDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dADatabaseDataGridView.ColumnHeadersVisible = false;
            this.dADatabaseDataGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dataGridViewTextBoxColumn1,
            this.colName,
            this.dataGridViewTextBoxColumn3,
            this.dataGridViewTextBoxColumn4});
            this.dADatabaseDataGridView.DataSource = this.dADatabaseBindingSource;
            this.dADatabaseDataGridView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dADatabaseDataGridView.Location = new System.Drawing.Point(0, 0);
            this.dADatabaseDataGridView.Name = "dADatabaseDataGridView";
            this.dADatabaseDataGridView.ReadOnly = true;
            this.dADatabaseDataGridView.RowHeadersVisible = false;
            this.dADatabaseDataGridView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dADatabaseDataGridView.Size = new System.Drawing.Size(172, 222);
            this.dADatabaseDataGridView.TabIndex = 1;
            // 
            // dataGridViewTextBoxColumn1
            // 
            this.dataGridViewTextBoxColumn1.DataPropertyName = "id";
            this.dataGridViewTextBoxColumn1.HeaderText = "id";
            this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
            this.dataGridViewTextBoxColumn1.ReadOnly = true;
            this.dataGridViewTextBoxColumn1.Visible = false;
            // 
            // colName
            // 
            this.colName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.colName.DataPropertyName = "name";
            this.colName.HeaderText = "Name";
            this.colName.Name = "colName";
            this.colName.ReadOnly = true;
            // 
            // dataGridViewTextBoxColumn3
            // 
            this.dataGridViewTextBoxColumn3.DataPropertyName = "connection_string";
            this.dataGridViewTextBoxColumn3.HeaderText = "Connection";
            this.dataGridViewTextBoxColumn3.Name = "dataGridViewTextBoxColumn3";
            this.dataGridViewTextBoxColumn3.ReadOnly = true;
            this.dataGridViewTextBoxColumn3.Visible = false;
            this.dataGridViewTextBoxColumn3.Width = 422;
            // 
            // dataGridViewTextBoxColumn4
            // 
            this.dataGridViewTextBoxColumn4.DataPropertyName = "effective_date";
            this.dataGridViewTextBoxColumn4.HeaderText = "Date";
            this.dataGridViewTextBoxColumn4.Name = "dataGridViewTextBoxColumn4";
            this.dataGridViewTextBoxColumn4.ReadOnly = true;
            this.dataGridViewTextBoxColumn4.Visible = false;
            // 
            // SelectDatabaseForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(172, 247);
            this.Controls.Add(this.dADatabaseDataGridView);
            this.Controls.Add(this.dADatabaseBindingNavigator);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SelectDatabaseForm";
            this.ShowIcon = false;
            this.Text = "Select Database";
            this.Load += new System.EventHandler(this.SelectDatabaseForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.settingsDataSet1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dADatabaseBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dADatabaseBindingNavigator)).EndInit();
            this.dADatabaseBindingNavigator.ResumeLayout(false);
            this.dADatabaseBindingNavigator.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dADatabaseDataGridView)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private SettingsDataSet1 settingsDataSet1;
        private System.Windows.Forms.BindingSource dADatabaseBindingSource;
        private SettingsDataSet1TableAdapters.DADatabaseTableAdapter dADatabaseTableAdapter;
        private SettingsDataSet1TableAdapters.TableAdapterManager tableAdapterManager;
        private System.Windows.Forms.BindingNavigator dADatabaseBindingNavigator;
        private System.Windows.Forms.ToolStripButton toolStripButton1;
        private System.Windows.Forms.DataGridView dADatabaseDataGridView;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn colName;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn3;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn4;
    }
}