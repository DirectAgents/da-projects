namespace EomApp1.Formss.Advertiser
{
    partial class AdvertiserMapping2
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AdvertiserMapping2));
            this.advertisersDataSet1 = new EomApp1.Formss.Advertiser.Data.AdvertisersDataSet1();
            this.advertiserBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.advertiserTableAdapter = new EomApp1.Formss.Advertiser.Data.AdvertisersDataSet1TableAdapters.AdvertiserTableAdapter();
            this.tableAdapterManager = new EomApp1.Formss.Advertiser.Data.AdvertisersDataSet1TableAdapters.TableAdapterManager();
            this.advertiserBindingNavigator = new System.Windows.Forms.BindingNavigator(this.components);
            this.bindingNavigatorMoveFirstItem = new System.Windows.Forms.ToolStripButton();
            this.bindingNavigatorMovePreviousItem = new System.Windows.Forms.ToolStripButton();
            this.bindingNavigatorSeparator = new System.Windows.Forms.ToolStripSeparator();
            this.bindingNavigatorPositionItem = new System.Windows.Forms.ToolStripTextBox();
            this.bindingNavigatorCountItem = new System.Windows.Forms.ToolStripLabel();
            this.bindingNavigatorSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.bindingNavigatorMoveNextItem = new System.Windows.Forms.ToolStripButton();
            this.bindingNavigatorMoveLastItem = new System.Windows.Forms.ToolStripButton();
            this.bindingNavigatorSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.bindingNavigatorAddNewItem = new System.Windows.Forms.ToolStripButton();
            this.bindingNavigatorDeleteItem = new System.Windows.Forms.ToolStripButton();
            this.advertiserBindingNavigatorSaveItem = new System.Windows.Forms.ToolStripButton();
            this.advertiserComboBox = new System.Windows.Forms.ComboBox();
            this.campaignBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.campaignTableAdapter = new EomApp1.Formss.Advertiser.Data.AdvertisersDataSet1TableAdapters.CampaignTableAdapter();
            this.campaignComboBox = new System.Windows.Forms.ComboBox();
            this.fKCampaignAdvertiserBindingSource = new System.Windows.Forms.BindingSource(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.advertisersDataSet1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.advertiserBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.advertiserBindingNavigator)).BeginInit();
            this.advertiserBindingNavigator.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.campaignBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.fKCampaignAdvertiserBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // advertisersDataSet1
            // 
            this.advertisersDataSet1.DataSetName = "AdvertisersDataSet1";
            this.advertisersDataSet1.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // advertiserBindingSource
            // 
            this.advertiserBindingSource.DataMember = "Advertiser";
            this.advertiserBindingSource.DataSource = this.advertisersDataSet1;
            // 
            // advertiserTableAdapter
            // 
            this.advertiserTableAdapter.ClearBeforeFill = true;
            // 
            // tableAdapterManager
            // 
            this.tableAdapterManager.AdvertiserTableAdapter = this.advertiserTableAdapter;
            this.tableAdapterManager.BackupDataSetBeforeUpdate = false;
            this.tableAdapterManager.CampaignTableAdapter = this.campaignTableAdapter;
            this.tableAdapterManager.UpdateOrder = EomApp1.Formss.Advertiser.Data.AdvertisersDataSet1TableAdapters.TableAdapterManager.UpdateOrderOption.InsertUpdateDelete;
            // 
            // advertiserBindingNavigator
            // 
            this.advertiserBindingNavigator.AddNewItem = this.bindingNavigatorAddNewItem;
            this.advertiserBindingNavigator.BindingSource = this.advertiserBindingSource;
            this.advertiserBindingNavigator.CountItem = this.bindingNavigatorCountItem;
            this.advertiserBindingNavigator.DeleteItem = this.bindingNavigatorDeleteItem;
            this.advertiserBindingNavigator.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
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
            this.advertiserBindingNavigatorSaveItem});
            this.advertiserBindingNavigator.Location = new System.Drawing.Point(0, 0);
            this.advertiserBindingNavigator.MoveFirstItem = this.bindingNavigatorMoveFirstItem;
            this.advertiserBindingNavigator.MoveLastItem = this.bindingNavigatorMoveLastItem;
            this.advertiserBindingNavigator.MoveNextItem = this.bindingNavigatorMoveNextItem;
            this.advertiserBindingNavigator.MovePreviousItem = this.bindingNavigatorMovePreviousItem;
            this.advertiserBindingNavigator.Name = "advertiserBindingNavigator";
            this.advertiserBindingNavigator.PositionItem = this.bindingNavigatorPositionItem;
            this.advertiserBindingNavigator.Size = new System.Drawing.Size(1038, 25);
            this.advertiserBindingNavigator.TabIndex = 0;
            this.advertiserBindingNavigator.Text = "bindingNavigator1";
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
            // bindingNavigatorCountItem
            // 
            this.bindingNavigatorCountItem.Name = "bindingNavigatorCountItem";
            this.bindingNavigatorCountItem.Size = new System.Drawing.Size(35, 15);
            this.bindingNavigatorCountItem.Text = "of {0}";
            this.bindingNavigatorCountItem.ToolTipText = "Total number of items";
            // 
            // bindingNavigatorSeparator1
            // 
            this.bindingNavigatorSeparator1.Name = "bindingNavigatorSeparator";
            this.bindingNavigatorSeparator1.Size = new System.Drawing.Size(6, 6);
            // 
            // bindingNavigatorMoveNextItem
            // 
            this.bindingNavigatorMoveNextItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.bindingNavigatorMoveNextItem.Image = ((System.Drawing.Image)(resources.GetObject("bindingNavigatorMoveNextItem.Image")));
            this.bindingNavigatorMoveNextItem.Name = "bindingNavigatorMoveNextItem";
            this.bindingNavigatorMoveNextItem.RightToLeftAutoMirrorImage = true;
            this.bindingNavigatorMoveNextItem.Size = new System.Drawing.Size(23, 20);
            this.bindingNavigatorMoveNextItem.Text = "Move next";
            // 
            // bindingNavigatorMoveLastItem
            // 
            this.bindingNavigatorMoveLastItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.bindingNavigatorMoveLastItem.Image = ((System.Drawing.Image)(resources.GetObject("bindingNavigatorMoveLastItem.Image")));
            this.bindingNavigatorMoveLastItem.Name = "bindingNavigatorMoveLastItem";
            this.bindingNavigatorMoveLastItem.RightToLeftAutoMirrorImage = true;
            this.bindingNavigatorMoveLastItem.Size = new System.Drawing.Size(23, 20);
            this.bindingNavigatorMoveLastItem.Text = "Move last";
            // 
            // bindingNavigatorSeparator2
            // 
            this.bindingNavigatorSeparator2.Name = "bindingNavigatorSeparator";
            this.bindingNavigatorSeparator2.Size = new System.Drawing.Size(6, 6);
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
            this.bindingNavigatorDeleteItem.Size = new System.Drawing.Size(23, 20);
            this.bindingNavigatorDeleteItem.Text = "Delete";
            // 
            // advertiserBindingNavigatorSaveItem
            // 
            this.advertiserBindingNavigatorSaveItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.advertiserBindingNavigatorSaveItem.Image = ((System.Drawing.Image)(resources.GetObject("advertiserBindingNavigatorSaveItem.Image")));
            this.advertiserBindingNavigatorSaveItem.Name = "advertiserBindingNavigatorSaveItem";
            this.advertiserBindingNavigatorSaveItem.Size = new System.Drawing.Size(23, 23);
            this.advertiserBindingNavigatorSaveItem.Text = "Save Data";
            this.advertiserBindingNavigatorSaveItem.Click += new System.EventHandler(this.advertiserBindingNavigatorSaveItem_Click);
            // 
            // advertiserComboBox
            // 
            this.advertiserComboBox.DataSource = this.advertiserBindingSource;
            this.advertiserComboBox.DisplayMember = "name";
            this.advertiserComboBox.FormattingEnabled = true;
            this.advertiserComboBox.Location = new System.Drawing.Point(12, 28);
            this.advertiserComboBox.Name = "advertiserComboBox";
            this.advertiserComboBox.Size = new System.Drawing.Size(300, 21);
            this.advertiserComboBox.TabIndex = 1;
            this.advertiserComboBox.ValueMember = "id";
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
            // campaignComboBox
            // 
            this.campaignComboBox.DataSource = this.fKCampaignAdvertiserBindingSource;
            this.campaignComboBox.DisplayMember = "campaign_name";
            this.campaignComboBox.FormattingEnabled = true;
            this.campaignComboBox.Location = new System.Drawing.Point(318, 28);
            this.campaignComboBox.Name = "campaignComboBox";
            this.campaignComboBox.Size = new System.Drawing.Size(300, 21);
            this.campaignComboBox.TabIndex = 2;
            // 
            // fKCampaignAdvertiserBindingSource
            // 
            this.fKCampaignAdvertiserBindingSource.DataMember = "FK_Campaign_Advertiser";
            this.fKCampaignAdvertiserBindingSource.DataSource = this.advertiserBindingSource;
            // 
            // AdvertiserMapping2
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1038, 524);
            this.Controls.Add(this.campaignComboBox);
            this.Controls.Add(this.advertiserComboBox);
            this.Controls.Add(this.advertiserBindingNavigator);
            this.Name = "AdvertiserMapping2";
            this.Text = "AdvertiserMapping2";
            this.Load += new System.EventHandler(this.AdvertiserMapping2_Load);
            ((System.ComponentModel.ISupportInitialize)(this.advertisersDataSet1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.advertiserBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.advertiserBindingNavigator)).EndInit();
            this.advertiserBindingNavigator.ResumeLayout(false);
            this.advertiserBindingNavigator.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.campaignBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.fKCampaignAdvertiserBindingSource)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Data.AdvertisersDataSet1 advertisersDataSet1;
        private System.Windows.Forms.BindingSource advertiserBindingSource;
        private Data.AdvertisersDataSet1TableAdapters.AdvertiserTableAdapter advertiserTableAdapter;
        private Data.AdvertisersDataSet1TableAdapters.TableAdapterManager tableAdapterManager;
        private System.Windows.Forms.BindingNavigator advertiserBindingNavigator;
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
        private System.Windows.Forms.ToolStripButton advertiserBindingNavigatorSaveItem;
        private Data.AdvertisersDataSet1TableAdapters.CampaignTableAdapter campaignTableAdapter;
        private System.Windows.Forms.ComboBox advertiserComboBox;
        private System.Windows.Forms.BindingSource campaignBindingSource;
        private System.Windows.Forms.ComboBox campaignComboBox;
        private System.Windows.Forms.BindingSource fKCampaignAdvertiserBindingSource;
    }
}