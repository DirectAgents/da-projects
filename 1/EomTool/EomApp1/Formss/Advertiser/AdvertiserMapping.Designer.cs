namespace EomApp1.Formss.Advertiser
{
    partial class AdvertiserMapping
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AdvertiserMapping));
            this.advertisersDataSet1 = new EomApp1.Formss.Advertiser.Data.AdvertisersDataSet1();
            this.campaignBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.campaignTableAdapter = new EomApp1.Formss.Advertiser.Data.AdvertisersDataSet1TableAdapters.CampaignTableAdapter();
            this.tableAdapterManager = new EomApp1.Formss.Advertiser.Data.AdvertisersDataSet1TableAdapters.TableAdapterManager();
            this.campaignBindingNavigator = new System.Windows.Forms.BindingNavigator(this.components);
            this.bindingNavigatorAddNewItem = new System.Windows.Forms.ToolStripButton();
            this.bindingNavigatorCountItem = new System.Windows.Forms.ToolStripLabel();
            this.bindingNavigatorDeleteItem = new System.Windows.Forms.ToolStripButton();
            this.bindingNavigatorMoveFirstItem = new System.Windows.Forms.ToolStripButton();
            this.bindingNavigatorMovePreviousItem = new System.Windows.Forms.ToolStripButton();
            this.bindingNavigatorSeparator = new System.Windows.Forms.ToolStripSeparator();
            this.bindingNavigatorPositionItem = new System.Windows.Forms.ToolStripTextBox();
            this.bindingNavigatorSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.bindingNavigatorMoveNextItem = new System.Windows.Forms.ToolStripButton();
            this.bindingNavigatorMoveLastItem = new System.Windows.Forms.ToolStripButton();
            this.bindingNavigatorSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripButton1 = new System.Windows.Forms.ToolStripButton();
            this.campaignBindingNavigatorSaveItem = new System.Windows.Forms.ToolStripButton();
            this.toolStripLabel1 = new System.Windows.Forms.ToolStripLabel();
            this.toolStripLabel2 = new System.Windows.Forms.ToolStripLabel();
            this.campaignDataGridView = new System.Windows.Forms.DataGridView();
            this.dataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn5 = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.advertiserBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.dataGridViewTextBoxColumn7 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn6 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn9 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.advertiserTableAdapter = new EomApp1.Formss.Advertiser.Data.AdvertisersDataSet1TableAdapters.AdvertiserTableAdapter();
            this.campaignBindingSource1 = new System.Windows.Forms.BindingSource(this.components);
            this.advertiserBindingSource1 = new System.Windows.Forms.BindingSource(this.components);
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            ((System.ComponentModel.ISupportInitialize)(this.advertisersDataSet1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.campaignBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.campaignBindingNavigator)).BeginInit();
            this.campaignBindingNavigator.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.campaignDataGridView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.advertiserBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.campaignBindingSource1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.advertiserBindingSource1)).BeginInit();
            this.SuspendLayout();
            // 
            // advertisersDataSet1
            // 
            this.advertisersDataSet1.DataSetName = "AdvertisersDataSet1";
            this.advertisersDataSet1.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // campaignBindingSource
            // 
            this.campaignBindingSource.DataMember = "Campaign";
            this.campaignBindingSource.DataSource = this.advertisersDataSet1;
            // 
            // campaignTableAdapter
            // 
            this.campaignTableAdapter.ClearBeforeFill = true;
            // 
            // tableAdapterManager
            // 
            this.tableAdapterManager.AdvertiserTableAdapter = null;
            this.tableAdapterManager.BackupDataSetBeforeUpdate = false;
            this.tableAdapterManager.CampaignTableAdapter = this.campaignTableAdapter;
            this.tableAdapterManager.UpdateOrder = EomApp1.Formss.Advertiser.Data.AdvertisersDataSet1TableAdapters.TableAdapterManager.UpdateOrderOption.InsertUpdateDelete;
            // 
            // campaignBindingNavigator
            // 
            this.campaignBindingNavigator.AddNewItem = this.bindingNavigatorAddNewItem;
            this.campaignBindingNavigator.BindingSource = this.campaignBindingSource;
            this.campaignBindingNavigator.CountItem = this.bindingNavigatorCountItem;
            this.campaignBindingNavigator.DeleteItem = this.bindingNavigatorDeleteItem;
            this.campaignBindingNavigator.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.bindingNavigatorMoveFirstItem,
            this.bindingNavigatorMovePreviousItem,
            this.bindingNavigatorSeparator,
            this.bindingNavigatorPositionItem,
            this.bindingNavigatorCountItem,
            this.bindingNavigatorSeparator1,
            this.bindingNavigatorMoveNextItem,
            this.bindingNavigatorMoveLastItem,
            this.bindingNavigatorSeparator2,
            this.toolStripButton1,
            this.campaignBindingNavigatorSaveItem,
            this.toolStripLabel1,
            this.bindingNavigatorAddNewItem,
            this.toolStripLabel2,
            this.bindingNavigatorDeleteItem});
            this.campaignBindingNavigator.Location = new System.Drawing.Point(0, 0);
            this.campaignBindingNavigator.MoveFirstItem = this.bindingNavigatorMoveFirstItem;
            this.campaignBindingNavigator.MoveLastItem = this.bindingNavigatorMoveLastItem;
            this.campaignBindingNavigator.MoveNextItem = this.bindingNavigatorMoveNextItem;
            this.campaignBindingNavigator.MovePreviousItem = this.bindingNavigatorMovePreviousItem;
            this.campaignBindingNavigator.Name = "campaignBindingNavigator";
            this.campaignBindingNavigator.PositionItem = this.bindingNavigatorPositionItem;
            this.campaignBindingNavigator.Size = new System.Drawing.Size(863, 25);
            this.campaignBindingNavigator.TabIndex = 0;
            this.campaignBindingNavigator.Text = "bindingNavigator1";
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
            // bindingNavigatorCountItem
            // 
            this.bindingNavigatorCountItem.Name = "bindingNavigatorCountItem";
            this.bindingNavigatorCountItem.Size = new System.Drawing.Size(35, 22);
            this.bindingNavigatorCountItem.Text = "of {0}";
            this.bindingNavigatorCountItem.ToolTipText = "Total number of items";
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
            // bindingNavigatorMoveFirstItem
            // 
            this.bindingNavigatorMoveFirstItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.bindingNavigatorMoveFirstItem.Image = ((System.Drawing.Image)(resources.GetObject("bindingNavigatorMoveFirstItem.Image")));
            this.bindingNavigatorMoveFirstItem.Name = "bindingNavigatorMoveFirstItem";
            this.bindingNavigatorMoveFirstItem.RightToLeftAutoMirrorImage = true;
            this.bindingNavigatorMoveFirstItem.Size = new System.Drawing.Size(23, 22);
            this.bindingNavigatorMoveFirstItem.Text = "Move first";
            // 
            // bindingNavigatorMovePreviousItem
            // 
            this.bindingNavigatorMovePreviousItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.bindingNavigatorMovePreviousItem.Image = ((System.Drawing.Image)(resources.GetObject("bindingNavigatorMovePreviousItem.Image")));
            this.bindingNavigatorMovePreviousItem.Name = "bindingNavigatorMovePreviousItem";
            this.bindingNavigatorMovePreviousItem.RightToLeftAutoMirrorImage = true;
            this.bindingNavigatorMovePreviousItem.Size = new System.Drawing.Size(23, 22);
            this.bindingNavigatorMovePreviousItem.Text = "Move previous";
            // 
            // bindingNavigatorSeparator
            // 
            this.bindingNavigatorSeparator.Name = "bindingNavigatorSeparator";
            this.bindingNavigatorSeparator.Size = new System.Drawing.Size(6, 25);
            // 
            // bindingNavigatorPositionItem
            // 
            this.bindingNavigatorPositionItem.AccessibleName = "Position";
            this.bindingNavigatorPositionItem.AutoSize = false;
            this.bindingNavigatorPositionItem.Name = "bindingNavigatorPositionItem";
            this.bindingNavigatorPositionItem.Size = new System.Drawing.Size(50, 23);
            this.bindingNavigatorPositionItem.Text = "0";
            this.bindingNavigatorPositionItem.ToolTipText = "Current position";
            // 
            // bindingNavigatorSeparator1
            // 
            this.bindingNavigatorSeparator1.Name = "bindingNavigatorSeparator1";
            this.bindingNavigatorSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // bindingNavigatorMoveNextItem
            // 
            this.bindingNavigatorMoveNextItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.bindingNavigatorMoveNextItem.Image = ((System.Drawing.Image)(resources.GetObject("bindingNavigatorMoveNextItem.Image")));
            this.bindingNavigatorMoveNextItem.Name = "bindingNavigatorMoveNextItem";
            this.bindingNavigatorMoveNextItem.RightToLeftAutoMirrorImage = true;
            this.bindingNavigatorMoveNextItem.Size = new System.Drawing.Size(23, 22);
            this.bindingNavigatorMoveNextItem.Text = "Move next";
            // 
            // bindingNavigatorMoveLastItem
            // 
            this.bindingNavigatorMoveLastItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.bindingNavigatorMoveLastItem.Image = ((System.Drawing.Image)(resources.GetObject("bindingNavigatorMoveLastItem.Image")));
            this.bindingNavigatorMoveLastItem.Name = "bindingNavigatorMoveLastItem";
            this.bindingNavigatorMoveLastItem.RightToLeftAutoMirrorImage = true;
            this.bindingNavigatorMoveLastItem.Size = new System.Drawing.Size(23, 22);
            this.bindingNavigatorMoveLastItem.Text = "Move last";
            // 
            // bindingNavigatorSeparator2
            // 
            this.bindingNavigatorSeparator2.Name = "bindingNavigatorSeparator2";
            this.bindingNavigatorSeparator2.Size = new System.Drawing.Size(6, 25);
            // 
            // toolStripButton1
            // 
            this.toolStripButton1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripButton1.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton1.Image")));
            this.toolStripButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton1.Name = "toolStripButton1";
            this.toolStripButton1.Size = new System.Drawing.Size(127, 22);
            this.toolStripButton1.Text = "Edit Advertiser Names";
            this.toolStripButton1.Click += new System.EventHandler(this.toolStripButton1_Click);
            // 
            // campaignBindingNavigatorSaveItem
            // 
            this.campaignBindingNavigatorSaveItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.campaignBindingNavigatorSaveItem.Image = ((System.Drawing.Image)(resources.GetObject("campaignBindingNavigatorSaveItem.Image")));
            this.campaignBindingNavigatorSaveItem.Name = "campaignBindingNavigatorSaveItem";
            this.campaignBindingNavigatorSaveItem.Size = new System.Drawing.Size(23, 22);
            this.campaignBindingNavigatorSaveItem.Text = "Save Data";
            this.campaignBindingNavigatorSaveItem.Click += new System.EventHandler(this.campaignBindingNavigatorSaveItem_Click);
            // 
            // toolStripLabel1
            // 
            this.toolStripLabel1.Name = "toolStripLabel1";
            this.toolStripLabel1.Size = new System.Drawing.Size(52, 22);
            this.toolStripLabel1.Text = "               ";
            // 
            // toolStripLabel2
            // 
            this.toolStripLabel2.Name = "toolStripLabel2";
            this.toolStripLabel2.Size = new System.Drawing.Size(64, 22);
            this.toolStripLabel2.Text = "                   ";
            // 
            // campaignDataGridView
            // 
            this.campaignDataGridView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.campaignDataGridView.AutoGenerateColumns = false;
            this.campaignDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.campaignDataGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dataGridViewTextBoxColumn1,
            this.dataGridViewTextBoxColumn5,
            this.dataGridViewTextBoxColumn7,
            this.dataGridViewTextBoxColumn6,
            this.dataGridViewTextBoxColumn9});
            this.campaignDataGridView.DataSource = this.campaignBindingSource;
            this.campaignDataGridView.Location = new System.Drawing.Point(12, 61);
            this.campaignDataGridView.Name = "campaignDataGridView";
            this.campaignDataGridView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.campaignDataGridView.Size = new System.Drawing.Size(851, 226);
            this.campaignDataGridView.TabIndex = 1;
            // 
            // dataGridViewTextBoxColumn1
            // 
            this.dataGridViewTextBoxColumn1.DataPropertyName = "id";
            this.dataGridViewTextBoxColumn1.HeaderText = "id";
            this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
            this.dataGridViewTextBoxColumn1.ReadOnly = true;
            this.dataGridViewTextBoxColumn1.Visible = false;
            // 
            // dataGridViewTextBoxColumn5
            // 
            this.dataGridViewTextBoxColumn5.DataPropertyName = "advertiser_id";
            this.dataGridViewTextBoxColumn5.DataSource = this.advertiserBindingSource;
            this.dataGridViewTextBoxColumn5.DisplayMember = "name";
            this.dataGridViewTextBoxColumn5.HeaderText = "Advertiser";
            this.dataGridViewTextBoxColumn5.Name = "dataGridViewTextBoxColumn5";
            this.dataGridViewTextBoxColumn5.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridViewTextBoxColumn5.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.dataGridViewTextBoxColumn5.ValueMember = "id";
            this.dataGridViewTextBoxColumn5.Width = 300;
            // 
            // advertiserBindingSource
            // 
            this.advertiserBindingSource.DataMember = "Advertiser";
            this.advertiserBindingSource.DataSource = this.advertisersDataSet1;
            // 
            // dataGridViewTextBoxColumn7
            // 
            this.dataGridViewTextBoxColumn7.DataPropertyName = "campaign_name";
            this.dataGridViewTextBoxColumn7.HeaderText = "Campaign";
            this.dataGridViewTextBoxColumn7.Name = "dataGridViewTextBoxColumn7";
            this.dataGridViewTextBoxColumn7.ReadOnly = true;
            this.dataGridViewTextBoxColumn7.Width = 300;
            // 
            // dataGridViewTextBoxColumn6
            // 
            this.dataGridViewTextBoxColumn6.DataPropertyName = "pid";
            this.dataGridViewTextBoxColumn6.HeaderText = "PID";
            this.dataGridViewTextBoxColumn6.Name = "dataGridViewTextBoxColumn6";
            this.dataGridViewTextBoxColumn6.ReadOnly = true;
            // 
            // dataGridViewTextBoxColumn9
            // 
            this.dataGridViewTextBoxColumn9.DataPropertyName = "modified";
            this.dataGridViewTextBoxColumn9.HeaderText = "Modified";
            this.dataGridViewTextBoxColumn9.Name = "dataGridViewTextBoxColumn9";
            this.dataGridViewTextBoxColumn9.ReadOnly = true;
            // 
            // advertiserTableAdapter
            // 
            this.advertiserTableAdapter.ClearBeforeFill = true;
            // 
            // campaignBindingSource1
            // 
            this.campaignBindingSource1.DataMember = "Campaign";
            this.campaignBindingSource1.DataSource = this.advertisersDataSet1;
            // 
            // advertiserBindingSource1
            // 
            this.advertiserBindingSource1.DataMember = "Advertiser";
            this.advertiserBindingSource1.DataSource = this.advertisersDataSet1;
            // 
            // comboBox1
            // 
            this.comboBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.comboBox1.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.comboBox1.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.comboBox1.DataSource = this.campaignBindingSource;
            this.comboBox1.DisplayMember = "campaign_name";
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Location = new System.Drawing.Point(12, 34);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(839, 21);
            this.comboBox1.TabIndex = 2;
            this.comboBox1.ValueMember = "pid";
            // 
            // AdvertiserMapping
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(863, 287);
            this.Controls.Add(this.comboBox1);
            this.Controls.Add(this.campaignDataGridView);
            this.Controls.Add(this.campaignBindingNavigator);
            this.Name = "AdvertiserMapping";
            this.Text = "AdvertiserMapping";
            this.Load += new System.EventHandler(this.AdvertiserMapping_Load);
            ((System.ComponentModel.ISupportInitialize)(this.advertisersDataSet1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.campaignBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.campaignBindingNavigator)).EndInit();
            this.campaignBindingNavigator.ResumeLayout(false);
            this.campaignBindingNavigator.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.campaignDataGridView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.advertiserBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.campaignBindingSource1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.advertiserBindingSource1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Data.AdvertisersDataSet1 advertisersDataSet1;
        private System.Windows.Forms.BindingSource campaignBindingSource;
        private Data.AdvertisersDataSet1TableAdapters.CampaignTableAdapter campaignTableAdapter;
        private Data.AdvertisersDataSet1TableAdapters.TableAdapterManager tableAdapterManager;
        private System.Windows.Forms.BindingNavigator campaignBindingNavigator;
        private System.Windows.Forms.ToolStripButton bindingNavigatorAddNewItem;
        private System.Windows.Forms.ToolStripLabel bindingNavigatorCountItem;
        private System.Windows.Forms.ToolStripButton bindingNavigatorDeleteItem;
        private System.Windows.Forms.ToolStripButton bindingNavigatorMoveFirstItem;
        private System.Windows.Forms.ToolStripButton bindingNavigatorMovePreviousItem;
        private System.Windows.Forms.ToolStripSeparator bindingNavigatorSeparator;
        private System.Windows.Forms.ToolStripTextBox bindingNavigatorPositionItem;
        private System.Windows.Forms.ToolStripSeparator bindingNavigatorSeparator1;
        private System.Windows.Forms.ToolStripButton bindingNavigatorMoveNextItem;
        private System.Windows.Forms.ToolStripButton bindingNavigatorMoveLastItem;
        private System.Windows.Forms.ToolStripSeparator bindingNavigatorSeparator2;
        private System.Windows.Forms.ToolStripButton campaignBindingNavigatorSaveItem;
        private System.Windows.Forms.DataGridView campaignDataGridView;
        private System.Windows.Forms.BindingSource advertiserBindingSource;
        private Data.AdvertisersDataSet1TableAdapters.AdvertiserTableAdapter advertiserTableAdapter;
        private System.Windows.Forms.ToolStripButton toolStripButton1;
        private System.Windows.Forms.BindingSource campaignBindingSource1;
        private System.Windows.Forms.BindingSource advertiserBindingSource1;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewComboBoxColumn dataGridViewTextBoxColumn5;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn7;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn6;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn9;
        private System.Windows.Forms.ToolStripLabel toolStripLabel2;
        private System.Windows.Forms.ToolStripLabel toolStripLabel1;
    }
}