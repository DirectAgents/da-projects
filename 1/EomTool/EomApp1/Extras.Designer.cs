namespace EomApp1
{
    partial class Extras
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Extras));
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.extraDataGridView = new System.Windows.Forms.DataGridView();
            this.source_id = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.sourceBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.dataSetForExtra1 = new EomApp1.data.DataSetForExtra();
            this.PublisherName = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.affiliateBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.Advertiser = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.advertiserBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.unitTypeBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.campaignBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.Campaign = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.CDNum = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.currencyBindingSource1 = new System.Windows.Forms.BindingSource(this.components);
            this.currencyBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.Rev = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.extraBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.panel1 = new System.Windows.Forms.Panel();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.extraTableAdapter = new EomApp1.data.DataSetForExtraTableAdapters.ExtraTableAdapter();
            this.tableAdapterManager = new EomApp1.data.DataSetForExtraTableAdapters.TableAdapterManager();
            this.extraBindingNavigator = new System.Windows.Forms.BindingNavigator(this.components);
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
            this.extraBindingNavigatorSaveItem = new System.Windows.Forms.ToolStripButton();
            this.sourceTableAdapter = new EomApp1.data.DataSetForExtraTableAdapters.SourceTableAdapter();
            this.fKExtraSourceBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.affiliateTableAdapter = new EomApp1.data.DataSetForExtraTableAdapters.AffiliateTableAdapter();
            this.unitTypeTableAdapter = new EomApp1.data.DataSetForExtraTableAdapters.UnitTypeTableAdapter();
            this.currencyTableAdapter = new EomApp1.data.DataSetForExtraTableAdapters.CurrencyTableAdapter();
            this.campaignTableAdapter = new EomApp1.data.DataSetForExtraTableAdapters.CampaignTableAdapter();
            this.advertiserTableAdapter = new EomApp1.data.DataSetForExtraTableAdapters.AdvertiserTableAdapter();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.extraDataGridView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.sourceBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataSetForExtra1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.affiliateBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.advertiserBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.unitTypeBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.campaignBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.currencyBindingSource1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.currencyBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.extraBindingSource)).BeginInit();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.extraBindingNavigator)).BeginInit();
            this.extraBindingNavigator.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.fKExtraSourceBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.extraDataGridView);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.panel1);
            this.splitContainer1.Size = new System.Drawing.Size(1319, 647);
            this.splitContainer1.SplitterDistance = 531;
            this.splitContainer1.TabIndex = 0;
            // 
            // extraDataGridView
            // 
            this.extraDataGridView.AutoGenerateColumns = false;
            this.extraDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.extraDataGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.source_id,
            this.PublisherName,
            this.Advertiser,
            this.Campaign,
            this.CDNum,
            this.Rev});
            this.extraDataGridView.DataSource = this.extraBindingSource;
            this.extraDataGridView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.extraDataGridView.Location = new System.Drawing.Point(0, 0);
            this.extraDataGridView.Name = "extraDataGridView";
            this.extraDataGridView.Size = new System.Drawing.Size(1319, 531);
            this.extraDataGridView.TabIndex = 0;
            this.extraDataGridView.CellValidated += new System.Windows.Forms.DataGridViewCellEventHandler(this.extraDataGridView_CellValidated);
            // 
            // source_id
            // 
            this.source_id.DataPropertyName = "source_id";
            this.source_id.DataSource = this.sourceBindingSource;
            this.source_id.DisplayMember = "source_name";
            this.source_id.HeaderText = "Source";
            this.source_id.Name = "source_id";
            this.source_id.ValueMember = "id";
            // 
            // sourceBindingSource
            // 
            this.sourceBindingSource.DataMember = "Source";
            this.sourceBindingSource.DataSource = this.dataSetForExtra1;
            // 
            // dataSetForExtra1
            // 
            this.dataSetForExtra1.DataSetName = "DataSetForExtra";
            this.dataSetForExtra1.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // PublisherName
            // 
            this.PublisherName.DataPropertyName = "affid";
            this.PublisherName.DataSource = this.affiliateBindingSource;
            this.PublisherName.DisplayMember = "company_name";
            this.PublisherName.DisplayStyle = System.Windows.Forms.DataGridViewComboBoxDisplayStyle.ComboBox;
            this.PublisherName.DisplayStyleForCurrentCellOnly = true;
            this.PublisherName.HeaderText = "Publisher";
            this.PublisherName.Name = "PublisherName";
            this.PublisherName.ValueMember = "affid";
            // 
            // affiliateBindingSource
            // 
            this.affiliateBindingSource.DataMember = "Affiliate";
            this.affiliateBindingSource.DataSource = this.dataSetForExtra1;
            // 
            // Advertiser
            // 
            this.Advertiser.DataPropertyName = "advertid";
            this.Advertiser.DataSource = this.advertiserBindingSource;
            this.Advertiser.DisplayMember = "advertiser_name";
            this.Advertiser.DisplayStyle = System.Windows.Forms.DataGridViewComboBoxDisplayStyle.ComboBox;
            this.Advertiser.DisplayStyleForCurrentCellOnly = true;
            this.Advertiser.HeaderText = "Advertiser";
            this.Advertiser.Name = "Advertiser";
            this.Advertiser.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.Advertiser.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.Advertiser.ValueMember = "id";
            // 
            // advertiserBindingSource
            // 
            this.advertiserBindingSource.DataMember = "Advertiser";
            this.advertiserBindingSource.DataSource = this.dataSetForExtra1;
            // 
            // unitTypeBindingSource
            // 
            this.unitTypeBindingSource.DataMember = "UnitType";
            this.unitTypeBindingSource.DataSource = this.dataSetForExtra1;
            // 
            // campaignBindingSource
            // 
            this.campaignBindingSource.DataMember = "Campaign";
            this.campaignBindingSource.DataSource = this.dataSetForExtra1;
            // 
            // Campaign
            // 
            this.Campaign.DataPropertyName = "pid";
            this.Campaign.DataSource = this.campaignBindingSource;
            this.Campaign.DisplayMember = "campaign_name";
            this.Campaign.DisplayStyle = System.Windows.Forms.DataGridViewComboBoxDisplayStyle.ComboBox;
            this.Campaign.DisplayStyleForCurrentCellOnly = true;
            this.Campaign.HeaderText = "Campaign";
            this.Campaign.Name = "Campaign";
            this.Campaign.ValueMember = "pid";
            // 
            // CDNum
            // 
            this.CDNum.DataPropertyName = "affid";
            this.CDNum.DataSource = this.affiliateBindingSource;
            this.CDNum.DisplayMember = "add_code";
            this.CDNum.DisplayStyle = System.Windows.Forms.DataGridViewComboBoxDisplayStyle.ComboBox;
            this.CDNum.DisplayStyleForCurrentCellOnly = true;
            this.CDNum.HeaderText = "CD Number";
            this.CDNum.Name = "CDNum";
            this.CDNum.ValueMember = "affid";
            // 
            // currencyBindingSource1
            // 
            this.currencyBindingSource1.DataMember = "Currency";
            this.currencyBindingSource1.DataSource = this.dataSetForExtra1;
            // 
            // currencyBindingSource
            // 
            this.currencyBindingSource.DataMember = "Currency";
            this.currencyBindingSource.DataSource = this.dataSetForExtra1;
            // 
            // Rev
            // 
            this.Rev.DataPropertyName = "Total Rev";
            this.Rev.HeaderText = "Total Rev";
            this.Rev.Name = "Rev";
            this.Rev.ReadOnly = true;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.comboBox1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1319, 235);
            this.panel1.TabIndex = 0;
            // 
            // comboBox1
            // 
            this.comboBox1.DataSource = this.sourceBindingSource;
            this.comboBox1.DisplayMember = "source_name";
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Location = new System.Drawing.Point(1207, 3);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(121, 21);
            this.comboBox1.TabIndex = 0;
            this.comboBox1.ValueMember = "id";
            // 
            // extraTableAdapter
            // 
            this.extraTableAdapter.ClearBeforeFill = true;
            // 
            // tableAdapterManager
            // 
            this.tableAdapterManager.AccountManagerTableAdapter = null;
            this.tableAdapterManager.AdManagerTableAdapter = null;
            this.tableAdapterManager.AdvertiserTableAdapter = null;
            this.tableAdapterManager.AffiliateTableAdapter = null;
            this.tableAdapterManager.BackupDataSetBeforeUpdate = false;
            this.tableAdapterManager.CampaignStatusTableAdapter = null;
            this.tableAdapterManager.CampaignTableAdapter = null;
            this.tableAdapterManager.CurrencyTableAdapter = null;
            this.tableAdapterManager.ExtraTableAdapter = this.extraTableAdapter;
            this.tableAdapterManager.MediaBuyerTableAdapter = null;
            this.tableAdapterManager.SourceTableAdapter = null;
            this.tableAdapterManager.UnitTypeTableAdapter = null;
            this.tableAdapterManager.UpdateOrder = EomApp1.data.DataSetForExtraTableAdapters.TableAdapterManager.UpdateOrderOption.InsertUpdateDelete;
            // 
            // extraBindingNavigator
            // 
            this.extraBindingNavigator.AddNewItem = this.bindingNavigatorAddNewItem;
            this.extraBindingNavigator.BindingSource = this.extraBindingSource;
            this.extraBindingNavigator.CountItem = this.bindingNavigatorCountItem;
            this.extraBindingNavigator.DeleteItem = this.bindingNavigatorDeleteItem;
            this.extraBindingNavigator.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.extraBindingNavigator.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.bindingNavigatorMoveFirstItem,
            this.bindingNavigatorMovePreviousItem,
            this.bindingNavigatorSeparator,
            this.bindingNavigatorPositionItem,
            this.bindingNavigatorCountItem,
            this.bindingNavigatorSeparator1,
            this.bindingNavigatorMoveNextItem,
            this.bindingNavigatorMoveLastItem,
            this.bindingNavigatorSeparator2,
            this.bindingNavigatorAddNewItem,
            this.bindingNavigatorDeleteItem,
            this.extraBindingNavigatorSaveItem});
            this.extraBindingNavigator.Location = new System.Drawing.Point(0, 622);
            this.extraBindingNavigator.MoveFirstItem = this.bindingNavigatorMoveFirstItem;
            this.extraBindingNavigator.MoveLastItem = this.bindingNavigatorMoveLastItem;
            this.extraBindingNavigator.MoveNextItem = this.bindingNavigatorMoveNextItem;
            this.extraBindingNavigator.MovePreviousItem = this.bindingNavigatorMovePreviousItem;
            this.extraBindingNavigator.Name = "extraBindingNavigator";
            this.extraBindingNavigator.PositionItem = this.bindingNavigatorPositionItem;
            this.extraBindingNavigator.Size = new System.Drawing.Size(1319, 25);
            this.extraBindingNavigator.TabIndex = 1;
            this.extraBindingNavigator.Text = "bindingNavigator1";
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
            // extraBindingNavigatorSaveItem
            // 
            this.extraBindingNavigatorSaveItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.extraBindingNavigatorSaveItem.Image = ((System.Drawing.Image)(resources.GetObject("extraBindingNavigatorSaveItem.Image")));
            this.extraBindingNavigatorSaveItem.Name = "extraBindingNavigatorSaveItem";
            this.extraBindingNavigatorSaveItem.Size = new System.Drawing.Size(23, 22);
            this.extraBindingNavigatorSaveItem.Text = "Save Data";
            this.extraBindingNavigatorSaveItem.Click += new System.EventHandler(this.extraBindingNavigatorSaveItem_Click);
            // 
            // sourceTableAdapter
            // 
            this.sourceTableAdapter.ClearBeforeFill = true;
            // 
            // fKExtraSourceBindingSource
            // 
            this.fKExtraSourceBindingSource.DataMember = "FK_Extra_Source";
            this.fKExtraSourceBindingSource.DataSource = this.sourceBindingSource;
            // 
            // affiliateTableAdapter
            // 
            this.affiliateTableAdapter.ClearBeforeFill = true;
            // 
            // unitTypeTableAdapter
            // 
            this.unitTypeTableAdapter.ClearBeforeFill = true;
            // 
            // currencyTableAdapter
            // 
            this.currencyTableAdapter.ClearBeforeFill = true;
            // 
            // campaignTableAdapter
            // 
            this.campaignTableAdapter.ClearBeforeFill = true;
            // 
            // advertiserTableAdapter
            // 
            this.advertiserTableAdapter.ClearBeforeFill = true;
            // 
            // Extras
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1319, 647);
            this.Controls.Add(this.extraBindingNavigator);
            this.Controls.Add(this.splitContainer1);
            this.Name = "Extras";
            this.ShowIcon = false;
            this.Text = "Extras";
            this.Load += new System.EventHandler(this.Extras_Load);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.extraDataGridView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.sourceBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataSetForExtra1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.affiliateBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.advertiserBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.unitTypeBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.campaignBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.currencyBindingSource1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.currencyBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.extraBindingSource)).EndInit();
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.extraBindingNavigator)).EndInit();
            this.extraBindingNavigator.ResumeLayout(false);
            this.extraBindingNavigator.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.fKExtraSourceBindingSource)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.BindingSource extraBindingSource;
        private data.DataSetForExtraTableAdapters.ExtraTableAdapter extraTableAdapter;
        private data.DataSetForExtraTableAdapters.TableAdapterManager tableAdapterManager;
        private System.Windows.Forms.BindingNavigator extraBindingNavigator;
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
        private System.Windows.Forms.ToolStripButton extraBindingNavigatorSaveItem;
        private System.Windows.Forms.DataGridView extraDataGridView;
        private data.DataSetForExtra dataSetForExtra1;
        private System.Windows.Forms.BindingSource sourceBindingSource;
        private data.DataSetForExtraTableAdapters.SourceTableAdapter sourceTableAdapter;
        private System.Windows.Forms.BindingSource fKExtraSourceBindingSource;
        private System.Windows.Forms.BindingSource affiliateBindingSource;
        private data.DataSetForExtraTableAdapters.AffiliateTableAdapter affiliateTableAdapter;
        private System.Windows.Forms.BindingSource unitTypeBindingSource;
        private data.DataSetForExtraTableAdapters.UnitTypeTableAdapter unitTypeTableAdapter;
        private System.Windows.Forms.BindingSource currencyBindingSource;
        private data.DataSetForExtraTableAdapters.CurrencyTableAdapter currencyTableAdapter;
        private System.Windows.Forms.BindingSource currencyBindingSource1;
        private System.Windows.Forms.BindingSource campaignBindingSource;
        private data.DataSetForExtraTableAdapters.CampaignTableAdapter campaignTableAdapter;
        private System.Windows.Forms.BindingSource advertiserBindingSource;
        private data.DataSetForExtraTableAdapters.AdvertiserTableAdapter advertiserTableAdapter;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.DataGridViewComboBoxColumn source_id;
        private System.Windows.Forms.DataGridViewComboBoxColumn PublisherName;
        private System.Windows.Forms.DataGridViewComboBoxColumn Advertiser;
        private System.Windows.Forms.DataGridViewComboBoxColumn Campaign;
        private System.Windows.Forms.DataGridViewComboBoxColumn CDNum;
        private System.Windows.Forms.DataGridViewTextBoxColumn Rev;
    }
}