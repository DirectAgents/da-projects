namespace EomApp1.PublisherNameCombine
{
    partial class PublisherCombine
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PublisherCombine));
            this.publisherCombineDataSet = new EomApp1.PublisherNameCombine.PublisherCombineDataSet();
            this.publisherBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.publisherTableAdapter = new EomApp1.PublisherNameCombine.PublisherCombineDataSetTableAdapters.PublisherTableAdapter();
            this.tableAdapterManager = new EomApp1.PublisherNameCombine.PublisherCombineDataSetTableAdapters.TableAdapterManager();
            this.publisherBindingNavigator = new System.Windows.Forms.BindingNavigator(this.components);
            this.bindingNavigatorAddNewItem = new System.Windows.Forms.ToolStripButton();
            this.bindingNavigatorDeleteItem = new System.Windows.Forms.ToolStripButton();
            this.publisherBindingNavigatorSaveItem = new System.Windows.Forms.ToolStripButton();
            this.publisherDataGridView = new System.Windows.Forms.DataGridView();
            this.dataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.toolStripLabel1 = new System.Windows.Forms.ToolStripLabel();
            this.toolStripLabel2 = new System.Windows.Forms.ToolStripLabel();
            ((System.ComponentModel.ISupportInitialize)(this.publisherCombineDataSet)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.publisherBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.publisherBindingNavigator)).BeginInit();
            this.publisherBindingNavigator.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.publisherDataGridView)).BeginInit();
            this.SuspendLayout();
            // 
            // publisherCombineDataSet
            // 
            this.publisherCombineDataSet.DataSetName = "PublisherCombineDataSet";
            this.publisherCombineDataSet.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // publisherBindingSource
            // 
            this.publisherBindingSource.DataMember = "Publisher";
            this.publisherBindingSource.DataSource = this.publisherCombineDataSet;
            // 
            // publisherTableAdapter
            // 
            this.publisherTableAdapter.ClearBeforeFill = true;
            // 
            // tableAdapterManager
            // 
            this.tableAdapterManager.BackupDataSetBeforeUpdate = false;
            this.tableAdapterManager.PublisherTableAdapter = this.publisherTableAdapter;
            this.tableAdapterManager.UpdateOrder = EomApp1.PublisherNameCombine.PublisherCombineDataSetTableAdapters.TableAdapterManager.UpdateOrderOption.InsertUpdateDelete;
            // 
            // publisherBindingNavigator
            // 
            this.publisherBindingNavigator.AddNewItem = this.bindingNavigatorAddNewItem;
            this.publisherBindingNavigator.BackColor = System.Drawing.Color.Black;
            this.publisherBindingNavigator.BindingSource = this.publisherBindingSource;
            this.publisherBindingNavigator.CountItem = null;
            this.publisherBindingNavigator.DeleteItem = this.bindingNavigatorDeleteItem;
            this.publisherBindingNavigator.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.publisherBindingNavigator.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.bindingNavigatorAddNewItem,
            this.toolStripLabel2,
            this.bindingNavigatorDeleteItem,
            this.toolStripLabel1,
            this.publisherBindingNavigatorSaveItem});
            this.publisherBindingNavigator.Location = new System.Drawing.Point(0, 0);
            this.publisherBindingNavigator.MoveFirstItem = null;
            this.publisherBindingNavigator.MoveLastItem = null;
            this.publisherBindingNavigator.MoveNextItem = null;
            this.publisherBindingNavigator.MovePreviousItem = null;
            this.publisherBindingNavigator.Name = "publisherBindingNavigator";
            this.publisherBindingNavigator.PositionItem = null;
            this.publisherBindingNavigator.Size = new System.Drawing.Size(504, 25);
            this.publisherBindingNavigator.TabIndex = 0;
            this.publisherBindingNavigator.Text = "bindingNavigator1";
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
            // publisherBindingNavigatorSaveItem
            // 
            this.publisherBindingNavigatorSaveItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.publisherBindingNavigatorSaveItem.Image = ((System.Drawing.Image)(resources.GetObject("publisherBindingNavigatorSaveItem.Image")));
            this.publisherBindingNavigatorSaveItem.Name = "publisherBindingNavigatorSaveItem";
            this.publisherBindingNavigatorSaveItem.Size = new System.Drawing.Size(23, 22);
            this.publisherBindingNavigatorSaveItem.Text = "Save Data";
            this.publisherBindingNavigatorSaveItem.Click += new System.EventHandler(this.publisherBindingNavigatorSaveItem_Click);
            // 
            // publisherDataGridView
            // 
            this.publisherDataGridView.AutoGenerateColumns = false;
            this.publisherDataGridView.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(80)))), ((int)(((byte)(80)))), ((int)(((byte)(120)))));
            this.publisherDataGridView.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.publisherDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.publisherDataGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dataGridViewTextBoxColumn1,
            this.dataGridViewTextBoxColumn2,
            this.dataGridViewTextBoxColumn3});
            this.publisherDataGridView.DataSource = this.publisherBindingSource;
            this.publisherDataGridView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.publisherDataGridView.Location = new System.Drawing.Point(0, 25);
            this.publisherDataGridView.Name = "publisherDataGridView";
            this.publisherDataGridView.RowHeadersVisible = false;
            this.publisherDataGridView.Size = new System.Drawing.Size(504, 640);
            this.publisherDataGridView.TabIndex = 1;
            // 
            // dataGridViewTextBoxColumn1
            // 
            this.dataGridViewTextBoxColumn1.DataPropertyName = "id";
            this.dataGridViewTextBoxColumn1.HeaderText = "id";
            this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
            this.dataGridViewTextBoxColumn1.ReadOnly = true;
            this.dataGridViewTextBoxColumn1.Visible = false;
            // 
            // dataGridViewTextBoxColumn2
            // 
            this.dataGridViewTextBoxColumn2.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.dataGridViewTextBoxColumn2.DataPropertyName = "name";
            this.dataGridViewTextBoxColumn2.HeaderText = "name";
            this.dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
            // 
            // dataGridViewTextBoxColumn3
            // 
            this.dataGridViewTextBoxColumn3.DataPropertyName = "affid";
            this.dataGridViewTextBoxColumn3.HeaderText = "affid";
            this.dataGridViewTextBoxColumn3.Name = "dataGridViewTextBoxColumn3";
            // 
            // toolStripLabel1
            // 
            this.toolStripLabel1.Name = "toolStripLabel1";
            this.toolStripLabel1.Size = new System.Drawing.Size(37, 22);
            this.toolStripLabel1.Text = "          ";
            // 
            // toolStripLabel2
            // 
            this.toolStripLabel2.Name = "toolStripLabel2";
            this.toolStripLabel2.Size = new System.Drawing.Size(37, 22);
            this.toolStripLabel2.Text = "          ";
            // 
            // PublisherCombine
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(504, 665);
            this.Controls.Add(this.publisherDataGridView);
            this.Controls.Add(this.publisherBindingNavigator);
            this.Name = "PublisherCombine";
            this.ShowIcon = false;
            this.Text = "Publisher Map";
            this.Load += new System.EventHandler(this.PublisherCombine_Load);
            ((System.ComponentModel.ISupportInitialize)(this.publisherCombineDataSet)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.publisherBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.publisherBindingNavigator)).EndInit();
            this.publisherBindingNavigator.ResumeLayout(false);
            this.publisherBindingNavigator.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.publisherDataGridView)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private PublisherCombineDataSet publisherCombineDataSet;
        private System.Windows.Forms.BindingSource publisherBindingSource;
        private PublisherCombineDataSetTableAdapters.PublisherTableAdapter publisherTableAdapter;
        private PublisherCombineDataSetTableAdapters.TableAdapterManager tableAdapterManager;
        private System.Windows.Forms.BindingNavigator publisherBindingNavigator;
        private System.Windows.Forms.ToolStripButton bindingNavigatorAddNewItem;
        private System.Windows.Forms.ToolStripButton bindingNavigatorDeleteItem;
        private System.Windows.Forms.ToolStripButton publisherBindingNavigatorSaveItem;
        private System.Windows.Forms.DataGridView publisherDataGridView;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn3;
        private System.Windows.Forms.ToolStripLabel toolStripLabel2;
        private System.Windows.Forms.ToolStripLabel toolStripLabel1;
    }
}