namespace QuickBooksService.Gui
{
    partial class QuickBooksServiceGuiForm
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
            this._usRadioButton = new System.Windows.Forms.RadioButton();
            this._intlRadioButton = new System.Windows.Forms.RadioButton();
            this._targetsCheckBoxList = new System.Windows.Forms.CheckedListBox();
            this._goButton = new System.Windows.Forms.Button();
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.backgroundWorker2 = new System.ComponentModel.BackgroundWorker();
            this.SuspendLayout();
            // 
            // _usRadioButton
            // 
            this._usRadioButton.AutoSize = true;
            this._usRadioButton.Location = new System.Drawing.Point(12, 12);
            this._usRadioButton.Name = "_usRadioButton";
            this._usRadioButton.Size = new System.Drawing.Size(40, 17);
            this._usRadioButton.TabIndex = 1;
            this._usRadioButton.Text = "US";
            this._usRadioButton.UseVisualStyleBackColor = true;
            this._usRadioButton.CheckedChanged += new System.EventHandler(this._CheckedChanged);
            // 
            // _intlRadioButton
            // 
            this._intlRadioButton.AutoSize = true;
            this._intlRadioButton.Location = new System.Drawing.Point(12, 36);
            this._intlRadioButton.Name = "_intlRadioButton";
            this._intlRadioButton.Size = new System.Drawing.Size(49, 17);
            this._intlRadioButton.TabIndex = 2;
            this._intlRadioButton.Text = "INTL";
            this._intlRadioButton.UseVisualStyleBackColor = true;
            this._intlRadioButton.CheckedChanged += new System.EventHandler(this._CheckedChanged);
            // 
            // _targetsCheckBoxList
            // 
            this._targetsCheckBoxList.CheckOnClick = true;
            this._targetsCheckBoxList.FormattingEnabled = true;
            this._targetsCheckBoxList.Items.AddRange(new object[] {
            "Customer",
            "Invoice",
            "Payment",
            "CreditMemo"});
            this._targetsCheckBoxList.Location = new System.Drawing.Point(67, 12);
            this._targetsCheckBoxList.Name = "_targetsCheckBoxList";
            this._targetsCheckBoxList.Size = new System.Drawing.Size(96, 64);
            this._targetsCheckBoxList.TabIndex = 3;
            // 
            // _goButton
            // 
            this._goButton.BackColor = System.Drawing.SystemColors.Control;
            this._goButton.Enabled = false;
            this._goButton.ForeColor = System.Drawing.Color.Black;
            this._goButton.Location = new System.Drawing.Point(169, 27);
            this._goButton.Name = "_goButton";
            this._goButton.Size = new System.Drawing.Size(116, 34);
            this._goButton.TabIndex = 4;
            this._goButton.Text = "Synch QuickBooks";
            this._goButton.UseVisualStyleBackColor = false;
            this._goButton.Click += new System.EventHandler(this._goButton_Click);
            // 
            // backgroundWorker1
            // 
            this.backgroundWorker1.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorker1_DoWork);
            // 
            // QuickBooksServiceGuiForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(297, 89);
            this.Controls.Add(this._goButton);
            this.Controls.Add(this._targetsCheckBoxList);
            this.Controls.Add(this._intlRadioButton);
            this.Controls.Add(this._usRadioButton);
            this.Name = "QuickBooksServiceGuiForm";
            this.ShowIcon = false;
            this.Text = "Quick Books Service";
            this.Load += new System.EventHandler(this.QuickBooksServiceGuiForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.RadioButton _usRadioButton;
        private System.Windows.Forms.RadioButton _intlRadioButton;
        private System.Windows.Forms.CheckedListBox _targetsCheckBoxList;
        private System.Windows.Forms.Button _goButton;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private System.ComponentModel.BackgroundWorker backgroundWorker2;
    }
}

