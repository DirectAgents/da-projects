namespace EomApp1.Screens.Synch
{
    partial class SynchForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SynchForm));
            this.CampaignPrepareForStatsButton = new System.Windows.Forms.Button();
            this.bindingSource1 = new System.Windows.Forms.BindingSource(this.components);
            this.campaignPrepareForStatsBackgroundWorker = new System.ComponentModel.BackgroundWorker();
            this.richTextBox1 = new System.Windows.Forms.RichTextBox();
            this.fromDayTextBox = new System.Windows.Forms.TextBox();
            this.toDayTextBox = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this._loopEveryComboBox = new System.Windows.Forms.ComboBox();
            this.everyRadioButton = new System.Windows.Forms.RadioButton();
            this.continuouslyRadioButton = new System.Windows.Forms.RadioButton();
            this._updateAMADCheckbox = new System.Windows.Forms.CheckBox();
            this._loopEveryTextBox = new System.Windows.Forms.TextBox();
            this._updateCampaignsCheckbox = new System.Windows.Forms.CheckBox();
            this._loopCheckBox = new System.Windows.Forms.CheckBox();
            this._activeCampaignsCheckBox = new System.Windows.Forms.CheckBox();
            this._updateABCheckBox = new System.Windows.Forms.CheckBox();
            this._extractConversionsFromCakeInBatches = new System.Windows.Forms.CheckBox();
            this._groupItemsToFirstDayOfMonthCheckBox = new System.Windows.Forms.CheckBox();
            this.loggerBox1 = new Mainn.Controls.Logging.LoggerBox();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource1)).BeginInit();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            this.SuspendLayout();
            // 
            // CampaignPrepareForStatsButton
            // 
            this.CampaignPrepareForStatsButton.BackColor = System.Drawing.Color.LightGreen;
            this.CampaignPrepareForStatsButton.Location = new System.Drawing.Point(104, 41);
            this.CampaignPrepareForStatsButton.Name = "CampaignPrepareForStatsButton";
            this.CampaignPrepareForStatsButton.Size = new System.Drawing.Size(50, 50);
            this.CampaignPrepareForStatsButton.TabIndex = 1;
            this.CampaignPrepareForStatsButton.Text = "Go";
            this.CampaignPrepareForStatsButton.UseVisualStyleBackColor = false;
            this.CampaignPrepareForStatsButton.Click += new System.EventHandler(this.CampaignPrepareForStatsButton_Click);
            // 
            // campaignPrepareForStatsBackgroundWorker
            // 
            this.campaignPrepareForStatsBackgroundWorker.DoWork += new System.ComponentModel.DoWorkEventHandler(this.Synch);
            // 
            // richTextBox1
            // 
            this.richTextBox1.Location = new System.Drawing.Point(12, 12);
            this.richTextBox1.Name = "richTextBox1";
            this.richTextBox1.Size = new System.Drawing.Size(69, 95);
            this.richTextBox1.TabIndex = 4;
            this.richTextBox1.Text = "";
            // 
            // fromDayTextBox
            // 
            this.fromDayTextBox.Location = new System.Drawing.Point(89, 11);
            this.fromDayTextBox.Name = "fromDayTextBox";
            this.fromDayTextBox.Size = new System.Drawing.Size(27, 20);
            this.fromDayTextBox.TabIndex = 7;
            this.fromDayTextBox.Text = "1";
            // 
            // toDayTextBox
            // 
            this.toDayTextBox.Location = new System.Drawing.Point(145, 11);
            this.toDayTextBox.Name = "toDayTextBox";
            this.toDayTextBox.Size = new System.Drawing.Size(27, 20);
            this.toDayTextBox.TabIndex = 8;
            this.toDayTextBox.Text = "31";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(123, 14);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(16, 13);
            this.label1.TabIndex = 9;
            this.label1.Text = "to";
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this._loopEveryComboBox);
            this.groupBox1.Controls.Add(this.everyRadioButton);
            this.groupBox1.Controls.Add(this.continuouslyRadioButton);
            this.groupBox1.Controls.Add(this._updateAMADCheckbox);
            this.groupBox1.Controls.Add(this._loopEveryTextBox);
            this.groupBox1.Controls.Add(this._updateCampaignsCheckbox);
            this.groupBox1.Controls.Add(this._loopCheckBox);
            this.groupBox1.Controls.Add(this._activeCampaignsCheckBox);
            this.groupBox1.Controls.Add(this._updateABCheckBox);
            this.groupBox1.Location = new System.Drawing.Point(1000, 5);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(0, 138);
            this.groupBox1.TabIndex = 12;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Additional Options";
            // 
            // _loopEveryComboBox
            // 
            this._loopEveryComboBox.Enabled = false;
            this._loopEveryComboBox.FormattingEnabled = true;
            this._loopEveryComboBox.Items.AddRange(new object[] {
            "Minutes",
            "Hours",
            "Days"});
            this._loopEveryComboBox.Location = new System.Drawing.Point(241, 52);
            this._loopEveryComboBox.Name = "_loopEveryComboBox";
            this._loopEveryComboBox.Size = new System.Drawing.Size(84, 21);
            this._loopEveryComboBox.TabIndex = 16;
            // 
            // everyRadioButton
            // 
            this.everyRadioButton.AutoSize = true;
            this.everyRadioButton.Location = new System.Drawing.Point(153, 53);
            this.everyRadioButton.Name = "everyRadioButton";
            this.everyRadioButton.Size = new System.Drawing.Size(52, 17);
            this.everyRadioButton.TabIndex = 15;
            this.everyRadioButton.Text = "Every";
            this.everyRadioButton.UseVisualStyleBackColor = true;
            this.everyRadioButton.CheckedChanged += new System.EventHandler(this.IntervalCheckedChanged);
            // 
            // continuouslyRadioButton
            // 
            this.continuouslyRadioButton.AutoSize = true;
            this.continuouslyRadioButton.Checked = true;
            this.continuouslyRadioButton.Location = new System.Drawing.Point(62, 53);
            this.continuouslyRadioButton.Name = "continuouslyRadioButton";
            this.continuouslyRadioButton.Size = new System.Drawing.Size(82, 17);
            this.continuouslyRadioButton.TabIndex = 15;
            this.continuouslyRadioButton.TabStop = true;
            this.continuouslyRadioButton.Text = "Coninuously";
            this.continuouslyRadioButton.UseVisualStyleBackColor = true;
            this.continuouslyRadioButton.CheckedChanged += new System.EventHandler(this.IntervalCheckedChanged);
            // 
            // _updateAMADCheckbox
            // 
            this._updateAMADCheckbox.AutoSize = true;
            this._updateAMADCheckbox.Location = new System.Drawing.Point(6, 99);
            this._updateAMADCheckbox.Name = "_updateAMADCheckbox";
            this._updateAMADCheckbox.Size = new System.Drawing.Size(143, 17);
            this._updateAMADCheckbox.TabIndex = 14;
            this._updateAMADCheckbox.Text = "Update AM/AD mapping";
            this._updateAMADCheckbox.UseVisualStyleBackColor = true;
            // 
            // _loopEveryTextBox
            // 
            this._loopEveryTextBox.Enabled = false;
            this._loopEveryTextBox.Location = new System.Drawing.Point(208, 52);
            this._loopEveryTextBox.Name = "_loopEveryTextBox";
            this._loopEveryTextBox.Size = new System.Drawing.Size(27, 20);
            this._loopEveryTextBox.TabIndex = 7;
            this._loopEveryTextBox.Text = "1";
            // 
            // _updateCampaignsCheckbox
            // 
            this._updateCampaignsCheckbox.AutoSize = true;
            this._updateCampaignsCheckbox.Location = new System.Drawing.Point(6, 76);
            this._updateCampaignsCheckbox.Name = "_updateCampaignsCheckbox";
            this._updateCampaignsCheckbox.Size = new System.Drawing.Size(116, 17);
            this._updateCampaignsCheckbox.TabIndex = 14;
            this._updateCampaignsCheckbox.Text = "Update Campaigns";
            this._updateCampaignsCheckbox.UseVisualStyleBackColor = true;
            // 
            // _loopCheckBox
            // 
            this._loopCheckBox.AutoSize = true;
            this._loopCheckBox.Location = new System.Drawing.Point(6, 53);
            this._loopCheckBox.Name = "_loopCheckBox";
            this._loopCheckBox.Size = new System.Drawing.Size(50, 17);
            this._loopCheckBox.TabIndex = 14;
            this._loopCheckBox.Text = "Loop";
            this._loopCheckBox.UseVisualStyleBackColor = true;
            this._loopCheckBox.CheckedChanged += new System.EventHandler(this.LoopCheckedChanged);
            // 
            // _activeCampaignsCheckBox
            // 
            this._activeCampaignsCheckBox.AutoSize = true;
            this._activeCampaignsCheckBox.Location = new System.Drawing.Point(6, 30);
            this._activeCampaignsCheckBox.Name = "_activeCampaignsCheckBox";
            this._activeCampaignsCheckBox.Size = new System.Drawing.Size(157, 17);
            this._activeCampaignsCheckBox.TabIndex = 14;
            this._activeCampaignsCheckBox.Text = "Synch all Active Campaigns";
            this._activeCampaignsCheckBox.UseVisualStyleBackColor = true;
            // 
            // _updateABCheckBox
            // 
            this._updateABCheckBox.AutoSize = true;
            this._updateABCheckBox.Location = new System.Drawing.Point(6, 122);
            this._updateABCheckBox.Name = "_updateABCheckBox";
            this._updateABCheckBox.Size = new System.Drawing.Size(78, 17);
            this._updateABCheckBox.TabIndex = 14;
            this._updateABCheckBox.Text = "Update AB";
            this._updateABCheckBox.UseVisualStyleBackColor = true;
            // 
            // _extractConversionsFromCakeInBatches
            // 
            this._extractConversionsFromCakeInBatches.AutoSize = true;
            this._extractConversionsFromCakeInBatches.Location = new System.Drawing.Point(179, 41);
            this._extractConversionsFromCakeInBatches.Name = "_extractConversionsFromCakeInBatches";
            this._extractConversionsFromCakeInBatches.Size = new System.Drawing.Size(112, 17);
            this._extractConversionsFromCakeInBatches.TabIndex = 14;
            this._extractConversionsFromCakeInBatches.Text = "Extract in Batches";
            this._extractConversionsFromCakeInBatches.UseVisualStyleBackColor = true;
            // 
            // _groupItemsToFirstDayOfMonthCheckBox
            // 
            this._groupItemsToFirstDayOfMonthCheckBox.AutoSize = true;
            this._groupItemsToFirstDayOfMonthCheckBox.Location = new System.Drawing.Point(179, 64);
            this._groupItemsToFirstDayOfMonthCheckBox.Name = "_groupItemsToFirstDayOfMonthCheckBox";
            this._groupItemsToFirstDayOfMonthCheckBox.Size = new System.Drawing.Size(95, 17);
            this._groupItemsToFirstDayOfMonthCheckBox.TabIndex = 14;
            this._groupItemsToFirstDayOfMonthCheckBox.Text = "Regroup Items";
            this._groupItemsToFirstDayOfMonthCheckBox.UseVisualStyleBackColor = true;
            this._groupItemsToFirstDayOfMonthCheckBox.EnabledChanged += new System.EventHandler(this._groupItemsToFirstDayOfMonthCheckBox_EnabledChanged);
            // 
            // loggerBox1
            // 
            this.loggerBox1.Location = new System.Drawing.Point(12, 113);
            this.loggerBox1.Name = "loggerBox1";
            this.loggerBox1.ShowErrorMessages = true;
            this.loggerBox1.ShowLogMessages = true;
            this.loggerBox1.Size = new System.Drawing.Size(344, 274);
            this.loggerBox1.TabIndex = 14;
            this.loggerBox1.Load += new System.EventHandler(this.loggerBox1_Load);
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.Location = new System.Drawing.Point(286, 44);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(16, 16);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pictureBox1.TabIndex = 15;
            this.pictureBox1.TabStop = false;
            this.toolTip1.SetToolTip(this.pictureBox1, "Check this box if there are more than 100,000 conversions.");
            // 
            // pictureBox2
            // 
            this.pictureBox2.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox2.Image")));
            this.pictureBox2.Location = new System.Drawing.Point(269, 66);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(16, 16);
            this.pictureBox2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pictureBox2.TabIndex = 15;
            this.pictureBox2.TabStop = false;
            this.toolTip1.SetToolTip(this.pictureBox2, "In some cases when there are many affiliates, check this box to limit the number " +
        "of items created.");
            // 
            // SynchForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(372, 404);
            this.Controls.Add(this.pictureBox2);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.loggerBox1);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.toDayTextBox);
            this.Controls.Add(this.fromDayTextBox);
            this.Controls.Add(this._groupItemsToFirstDayOfMonthCheckBox);
            this.Controls.Add(this._extractConversionsFromCakeInBatches);
            this.Controls.Add(this.richTextBox1);
            this.Controls.Add(this.CampaignPrepareForStatsButton);
            this.MaximizeBox = false;
            this.Name = "SynchForm";
            this.ShowIcon = false;
            this.Text = "Synch Stats";
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource1)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button CampaignPrepareForStatsButton;
        private System.Windows.Forms.BindingSource bindingSource1;
        private System.ComponentModel.BackgroundWorker campaignPrepareForStatsBackgroundWorker;
        private System.Windows.Forms.RichTextBox richTextBox1;
        private System.Windows.Forms.TextBox fromDayTextBox;
        private System.Windows.Forms.TextBox toDayTextBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.CheckBox _updateABCheckBox;
        private System.Windows.Forms.CheckBox _activeCampaignsCheckBox;
        private System.Windows.Forms.CheckBox _loopCheckBox;
        private System.Windows.Forms.CheckBox _updateCampaignsCheckbox;
        private System.Windows.Forms.CheckBox _updateAMADCheckbox;
        private System.Windows.Forms.ComboBox _loopEveryComboBox;
        private System.Windows.Forms.RadioButton everyRadioButton;
        private System.Windows.Forms.RadioButton continuouslyRadioButton;
        private System.Windows.Forms.TextBox _loopEveryTextBox;
        private Mainn.Controls.Logging.LoggerBox loggerBox1;
        private System.Windows.Forms.CheckBox _groupItemsToFirstDayOfMonthCheckBox;
        private System.Windows.Forms.CheckBox _extractConversionsFromCakeInBatches;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.PictureBox pictureBox2;
        private System.Windows.Forms.ToolTip toolTip1;
    }
}