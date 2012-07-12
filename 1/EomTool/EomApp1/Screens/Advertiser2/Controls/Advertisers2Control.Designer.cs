namespace EomApp1.Screens.Advertiser2.Controls
{
    partial class Advertisers2Control
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Advertisers2Control));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            this.advertiser2DataSet = new EomApp1.Screens.Advertiser2.Data.Advertiser2DataSet();
            this.advertiserBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.advertiserTableAdapter = new EomApp1.Screens.Advertiser2.Data.Advertiser2DataSetTableAdapters.AdvertiserTableAdapter();
            this.tableAdapterManager = new EomApp1.Screens.Advertiser2.Data.Advertiser2DataSetTableAdapters.TableAdapterManager();
            this.advertiserBindingNavigator = new System.Windows.Forms.BindingNavigator(this.components);
            this.bindingNavigatorAddNewItem = new System.Windows.Forms.ToolStripButton();
            this.bindingNavigatorDeleteItem = new System.Windows.Forms.ToolStripButton();
            this.advertiserBindingNavigatorSaveItem = new System.Windows.Forms.ToolStripButton();
            this.advertiserDataGridView = new System.Windows.Forms.DataGridView();
            this.dataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.toolStripLabel1 = new System.Windows.Forms.ToolStripLabel();
            this.toolStripLabel2 = new System.Windows.Forms.ToolStripLabel();
            ((System.ComponentModel.ISupportInitialize)(this.advertiser2DataSet)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.advertiserBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.advertiserBindingNavigator)).BeginInit();
            this.advertiserBindingNavigator.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.advertiserDataGridView)).BeginInit();
            this.SuspendLayout();
            // 
            // advertiser2DataSet
            // 
            this.advertiser2DataSet.DataSetName = "Advertiser2DataSet";
            this.advertiser2DataSet.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // advertiserBindingSource
            // 
            this.advertiserBindingSource.DataMember = "Advertiser";
            this.advertiserBindingSource.DataSource = this.advertiser2DataSet;
            // 
            // advertiserTableAdapter
            // 
            this.advertiserTableAdapter.ClearBeforeFill = true;
            // 
            // tableAdapterManager
            // 
            this.tableAdapterManager.AdvertiserTableAdapter = this.advertiserTableAdapter;
            this.tableAdapterManager.BackupDataSetBeforeUpdate = false;
            this.tableAdapterManager.UpdateOrder = EomApp1.Screens.Advertiser2.Data.Advertiser2DataSetTableAdapters.TableAdapterManager.UpdateOrderOption.InsertUpdateDelete;
            // 
            // advertiserBindingNavigator
            // 
            this.advertiserBindingNavigator.AddNewItem = this.bindingNavigatorAddNewItem;
            this.advertiserBindingNavigator.BindingSource = this.advertiserBindingSource;
            this.advertiserBindingNavigator.CountItem = null;
            this.advertiserBindingNavigator.DeleteItem = this.bindingNavigatorDeleteItem;
            this.advertiserBindingNavigator.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.advertiserBindingNavigator.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.bindingNavigatorAddNewItem,
            this.toolStripLabel2,
            this.bindingNavigatorDeleteItem,
            this.toolStripLabel1,
            this.advertiserBindingNavigatorSaveItem});
            this.advertiserBindingNavigator.Location = new System.Drawing.Point(0, 0);
            this.advertiserBindingNavigator.MoveFirstItem = null;
            this.advertiserBindingNavigator.MoveLastItem = null;
            this.advertiserBindingNavigator.MoveNextItem = null;
            this.advertiserBindingNavigator.MovePreviousItem = null;
            this.advertiserBindingNavigator.Name = "advertiserBindingNavigator";
            this.advertiserBindingNavigator.PositionItem = null;
            this.advertiserBindingNavigator.Size = new System.Drawing.Size(669, 25);
            this.advertiserBindingNavigator.TabIndex = 0;
            this.advertiserBindingNavigator.Text = "bindingNavigator1";
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
            // advertiserBindingNavigatorSaveItem
            // 
            this.advertiserBindingNavigatorSaveItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.advertiserBindingNavigatorSaveItem.Image = ((System.Drawing.Image)(resources.GetObject("advertiserBindingNavigatorSaveItem.Image")));
            this.advertiserBindingNavigatorSaveItem.Name = "advertiserBindingNavigatorSaveItem";
            this.advertiserBindingNavigatorSaveItem.Size = new System.Drawing.Size(23, 22);
            this.advertiserBindingNavigatorSaveItem.Text = "Save Data";
            this.advertiserBindingNavigatorSaveItem.Click += new System.EventHandler(this.advertiserBindingNavigatorSaveItem_Click);
            // 
            // advertiserDataGridView
            // 
            this.advertiserDataGridView.AutoGenerateColumns = false;
            this.advertiserDataGridView.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.advertiserDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.advertiserDataGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dataGridViewTextBoxColumn1,
            this.dataGridViewTextBoxColumn2});
            this.advertiserDataGridView.DataSource = this.advertiserBindingSource;
            this.advertiserDataGridView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.advertiserDataGridView.Location = new System.Drawing.Point(0, 25);
            this.advertiserDataGridView.Name = "advertiserDataGridView";
            this.advertiserDataGridView.RowHeadersVisible = false;
            this.advertiserDataGridView.Size = new System.Drawing.Size(669, 599);
            this.advertiserDataGridView.TabIndex = 1;
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
            this.dataGridViewTextBoxColumn2.HeaderText = "Advertiser";
            this.dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
            // 
            // toolStripLabel1
            // 
            this.toolStripLabel1.Name = "toolStripLabel1";
            this.toolStripLabel1.Size = new System.Drawing.Size(22, 22);
            this.toolStripLabel1.Text = "     ";
            // 
            // toolStripLabel2
            // 
            this.toolStripLabel2.Name = "toolStripLabel2";
            this.toolStripLabel2.Size = new System.Drawing.Size(22, 22);
            this.toolStripLabel2.Text = "     ";
            // 
            // Advertisers2Control
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.advertiserDataGridView);
            this.Controls.Add(this.advertiserBindingNavigator);
            this.Name = "Advertisers2Control";
            this.Size = new System.Drawing.Size(669, 624);
            this.Load += new System.EventHandler(this.Advertisers2Control_Load);
            ((System.ComponentModel.ISupportInitialize)(this.advertiser2DataSet)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.advertiserBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.advertiserBindingNavigator)).EndInit();
            this.advertiserBindingNavigator.ResumeLayout(false);
            this.advertiserBindingNavigator.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.advertiserDataGridView)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Data.Advertiser2DataSet advertiser2DataSet;
        private System.Windows.Forms.BindingSource advertiserBindingSource;
        private Data.Advertiser2DataSetTableAdapters.AdvertiserTableAdapter advertiserTableAdapter;
        private Data.Advertiser2DataSetTableAdapters.TableAdapterManager tableAdapterManager;
        private System.Windows.Forms.BindingNavigator advertiserBindingNavigator;
        private System.Windows.Forms.ToolStripButton bindingNavigatorAddNewItem;
        private System.Windows.Forms.ToolStripButton bindingNavigatorDeleteItem;
        private System.Windows.Forms.ToolStripButton advertiserBindingNavigatorSaveItem;
        private System.Windows.Forms.DataGridView advertiserDataGridView;
        private System.Windows.Forms.ToolStripLabel toolStripLabel2;
        private System.Windows.Forms.ToolStripLabel toolStripLabel1;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
        private System.Windows.Forms.BindingSource currencyBindingSource;
    }
}
