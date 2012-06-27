namespace EomApp1
{
    partial class SourceForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SourceForm));
            System.Windows.Forms.Label idLabel;
            System.Windows.Forms.Label source_NameLabel;
            this.dataSetForSource = new EomApp1.data.DataSetForSource();
            this.sourceBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.sourceTableAdapter = new EomApp1.data.DataSetForSourceTableAdapters.SourceTableAdapter();
            this.tableAdapterManager = new EomApp1.data.DataSetForSourceTableAdapters.TableAdapterManager();
            this.sourceBindingNavigator = new System.Windows.Forms.BindingNavigator(this.components);
            this.bindingNavigatorAddNewItem = new System.Windows.Forms.ToolStripButton();
            this.bindingNavigatorDeleteItem = new System.Windows.Forms.ToolStripButton();
            this.sourceBindingNavigatorSaveItem = new System.Windows.Forms.ToolStripButton();
            this.sourceDataGridView = new System.Windows.Forms.DataGridView();
            this.idTextBox = new System.Windows.Forms.TextBox();
            this.source_NameTextBox = new System.Windows.Forms.TextBox();
            this.dataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            idLabel = new System.Windows.Forms.Label();
            source_NameLabel = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.dataSetForSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.sourceBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.sourceBindingNavigator)).BeginInit();
            this.sourceBindingNavigator.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.sourceDataGridView)).BeginInit();
            this.SuspendLayout();
            // 
            // dataSetForSource
            // 
            this.dataSetForSource.DataSetName = "DataSetForSource";
            this.dataSetForSource.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // sourceBindingSource
            // 
            this.sourceBindingSource.DataMember = "Source";
            this.sourceBindingSource.DataSource = this.dataSetForSource;
            // 
            // sourceTableAdapter
            // 
            this.sourceTableAdapter.ClearBeforeFill = true;
            // 
            // tableAdapterManager
            // 
            this.tableAdapterManager.BackupDataSetBeforeUpdate = false;
            this.tableAdapterManager.SourceTableAdapter = this.sourceTableAdapter;
            this.tableAdapterManager.UpdateOrder = EomApp1.data.DataSetForSourceTableAdapters.TableAdapterManager.UpdateOrderOption.InsertUpdateDelete;
            // 
            // sourceBindingNavigator
            // 
            this.sourceBindingNavigator.AddNewItem = this.bindingNavigatorAddNewItem;
            this.sourceBindingNavigator.BindingSource = this.sourceBindingSource;
            this.sourceBindingNavigator.CountItem = null;
            this.sourceBindingNavigator.DeleteItem = this.bindingNavigatorDeleteItem;
            this.sourceBindingNavigator.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.sourceBindingNavigator.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.sourceBindingNavigator.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.bindingNavigatorAddNewItem,
            this.bindingNavigatorDeleteItem,
            this.sourceBindingNavigatorSaveItem});
            this.sourceBindingNavigator.Location = new System.Drawing.Point(0, 299);
            this.sourceBindingNavigator.MoveFirstItem = null;
            this.sourceBindingNavigator.MoveLastItem = null;
            this.sourceBindingNavigator.MoveNextItem = null;
            this.sourceBindingNavigator.MovePreviousItem = null;
            this.sourceBindingNavigator.Name = "sourceBindingNavigator";
            this.sourceBindingNavigator.PositionItem = null;
            this.sourceBindingNavigator.Size = new System.Drawing.Size(630, 25);
            this.sourceBindingNavigator.TabIndex = 0;
            this.sourceBindingNavigator.Text = "bindingNavigator1";
            // 
            // bindingNavigatorAddNewItem
            // 
            this.bindingNavigatorAddNewItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.bindingNavigatorAddNewItem.Image = ((System.Drawing.Image)(resources.GetObject("bindingNavigatorAddNewItem.Image")));
            this.bindingNavigatorAddNewItem.Name = "bindingNavigatorAddNewItem";
            this.bindingNavigatorAddNewItem.RightToLeftAutoMirrorImage = true;
            this.bindingNavigatorAddNewItem.Size = new System.Drawing.Size(23, 22);
            this.bindingNavigatorAddNewItem.Text = "Add new";
            // 
            // bindingNavigatorDeleteItem
            // 
            this.bindingNavigatorDeleteItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.bindingNavigatorDeleteItem.Image = ((System.Drawing.Image)(resources.GetObject("bindingNavigatorDeleteItem.Image")));
            this.bindingNavigatorDeleteItem.Name = "bindingNavigatorDeleteItem";
            this.bindingNavigatorDeleteItem.RightToLeftAutoMirrorImage = true;
            this.bindingNavigatorDeleteItem.Size = new System.Drawing.Size(23, 22);
            this.bindingNavigatorDeleteItem.Text = "Delete";
            // 
            // sourceBindingNavigatorSaveItem
            // 
            this.sourceBindingNavigatorSaveItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.sourceBindingNavigatorSaveItem.Image = ((System.Drawing.Image)(resources.GetObject("sourceBindingNavigatorSaveItem.Image")));
            this.sourceBindingNavigatorSaveItem.Name = "sourceBindingNavigatorSaveItem";
            this.sourceBindingNavigatorSaveItem.Size = new System.Drawing.Size(23, 22);
            this.sourceBindingNavigatorSaveItem.Text = "Save Data";
            this.sourceBindingNavigatorSaveItem.Click += new System.EventHandler(this.sourceBindingNavigatorSaveItem_Click);
            // 
            // sourceDataGridView
            // 
            this.sourceDataGridView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.sourceDataGridView.AutoGenerateColumns = false;
            this.sourceDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.sourceDataGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dataGridViewTextBoxColumn1,
            this.dataGridViewTextBoxColumn2});
            this.sourceDataGridView.DataSource = this.sourceBindingSource;
            this.sourceDataGridView.Location = new System.Drawing.Point(0, 0);
            this.sourceDataGridView.Name = "sourceDataGridView";
            this.sourceDataGridView.Size = new System.Drawing.Size(630, 217);
            this.sourceDataGridView.TabIndex = 1;
            // 
            // idLabel
            // 
            idLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            idLabel.AutoSize = true;
            idLabel.Location = new System.Drawing.Point(12, 239);
            idLabel.Name = "idLabel";
            idLabel.Size = new System.Drawing.Size(19, 13);
            idLabel.TabIndex = 2;
            idLabel.Text = "Id:";
            // 
            // idTextBox
            // 
            this.idTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.idTextBox.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.sourceBindingSource, "Id", true));
            this.idTextBox.Location = new System.Drawing.Point(93, 236);
            this.idTextBox.Name = "idTextBox";
            this.idTextBox.Size = new System.Drawing.Size(140, 20);
            this.idTextBox.TabIndex = 3;
            // 
            // source_NameLabel
            // 
            source_NameLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            source_NameLabel.AutoSize = true;
            source_NameLabel.Location = new System.Drawing.Point(12, 265);
            source_NameLabel.Name = "source_NameLabel";
            source_NameLabel.Size = new System.Drawing.Size(75, 13);
            source_NameLabel.TabIndex = 4;
            source_NameLabel.Text = "Source Name:";
            // 
            // source_NameTextBox
            // 
            this.source_NameTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.source_NameTextBox.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.sourceBindingSource, "Source Name", true));
            this.source_NameTextBox.Location = new System.Drawing.Point(93, 262);
            this.source_NameTextBox.Name = "source_NameTextBox";
            this.source_NameTextBox.Size = new System.Drawing.Size(140, 20);
            this.source_NameTextBox.TabIndex = 5;
            // 
            // dataGridViewTextBoxColumn1
            // 
            this.dataGridViewTextBoxColumn1.DataPropertyName = "Id";
            this.dataGridViewTextBoxColumn1.HeaderText = "Id";
            this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
            this.dataGridViewTextBoxColumn1.ReadOnly = true;
            this.dataGridViewTextBoxColumn1.Width = 50;
            // 
            // dataGridViewTextBoxColumn2
            // 
            this.dataGridViewTextBoxColumn2.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.dataGridViewTextBoxColumn2.DataPropertyName = "Source Name";
            this.dataGridViewTextBoxColumn2.HeaderText = "Source Name";
            this.dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
            // 
            // SourceForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(630, 324);
            this.Controls.Add(idLabel);
            this.Controls.Add(this.idTextBox);
            this.Controls.Add(source_NameLabel);
            this.Controls.Add(this.source_NameTextBox);
            this.Controls.Add(this.sourceDataGridView);
            this.Controls.Add(this.sourceBindingNavigator);
            this.Name = "SourceForm";
            this.ShowIcon = false;
            this.Text = "Sources";
            this.Load += new System.EventHandler(this.SourceForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataSetForSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.sourceBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.sourceBindingNavigator)).EndInit();
            this.sourceBindingNavigator.ResumeLayout(false);
            this.sourceBindingNavigator.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.sourceDataGridView)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private data.DataSetForSource dataSetForSource;
        private System.Windows.Forms.BindingSource sourceBindingSource;
        private data.DataSetForSourceTableAdapters.SourceTableAdapter sourceTableAdapter;
        private data.DataSetForSourceTableAdapters.TableAdapterManager tableAdapterManager;
        private System.Windows.Forms.BindingNavigator sourceBindingNavigator;
        private System.Windows.Forms.ToolStripButton bindingNavigatorAddNewItem;
        private System.Windows.Forms.ToolStripButton bindingNavigatorDeleteItem;
        private System.Windows.Forms.ToolStripButton sourceBindingNavigatorSaveItem;
        private System.Windows.Forms.DataGridView sourceDataGridView;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
        private System.Windows.Forms.TextBox idTextBox;
        private System.Windows.Forms.TextBox source_NameTextBox;
    }
}