namespace EomApp1.Formss.Campaign
{
    partial class SearchControl
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
            this._searchLabel = new System.Windows.Forms.Label();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this._searchTextBox = new System.Windows.Forms.TextBox();
            this._cancelButton = new System.Windows.Forms.Button();
            this._accountManagerLink = new System.Windows.Forms.LinkLabel();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // _searchLabel
            // 
            this._searchLabel.AutoSize = true;
            this._searchLabel.Location = new System.Drawing.Point(0, 0);
            this._searchLabel.Margin = new System.Windows.Forms.Padding(0);
            this._searchLabel.Name = "_searchLabel";
            this._searchLabel.Size = new System.Drawing.Size(41, 13);
            this._searchLabel.TabIndex = 0;
            this._searchLabel.Text = "Search";
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 4;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 75F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 166F));
            this.tableLayoutPanel1.Controls.Add(this._searchLabel, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this._searchTextBox, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this._cancelButton, 2, 0);
            this.tableLayoutPanel1.Controls.Add(this._accountManagerLink, 3, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(300, 16);
            this.tableLayoutPanel1.TabIndex = 3;
            // 
            // _searchTextBox
            // 
            this._searchTextBox.BackColor = System.Drawing.Color.MistyRose;
            this._searchTextBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this._searchTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this._searchTextBox.Location = new System.Drawing.Point(41, 0);
            this._searchTextBox.Margin = new System.Windows.Forms.Padding(0);
            this._searchTextBox.Name = "_searchTextBox";
            this._searchTextBox.Size = new System.Drawing.Size(75, 13);
            this._searchTextBox.TabIndex = 1;
            this._searchTextBox.TextChanged += new System.EventHandler(this.SearchTextBox_TextChanged);
            this._searchTextBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this._searchTextBox_KeyDown);
            // 
            // _cancelButton
            // 
            this._cancelButton.BackgroundImage = global::EomApp1.Properties.Resources.button_cancel_32;
            this._cancelButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this._cancelButton.FlatAppearance.BorderSize = 0;
            this._cancelButton.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Silver;
            this._cancelButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this._cancelButton.Location = new System.Drawing.Point(119, 3);
            this._cancelButton.Name = "_cancelButton";
            this._cancelButton.Size = new System.Drawing.Size(12, 10);
            this._cancelButton.TabIndex = 2;
            this._cancelButton.UseVisualStyleBackColor = true;
            this._cancelButton.Visible = false;
            this._cancelButton.Click += new System.EventHandler(this.CancelSearchButton_Click);
            // 
            // _accountManagerLink
            // 
            this._accountManagerLink.AutoSize = true;
            this._accountManagerLink.Location = new System.Drawing.Point(137, 0);
            this._accountManagerLink.Name = "_accountManagerLink";
            this._accountManagerLink.Size = new System.Drawing.Size(42, 13);
            this._accountManagerLink.TabIndex = 3;
            this._accountManagerLink.TabStop = true;
            this._accountManagerLink.Text = "All AMs";
            this._accountManagerLink.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this._accountManagerLink_LinkClicked);
            // 
            // SearchControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tableLayoutPanel1);
            this.Margin = new System.Windows.Forms.Padding(0);
            this.Name = "SearchControl";
            this.Size = new System.Drawing.Size(300, 16);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label _searchLabel;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.TextBox _searchTextBox;
        private System.Windows.Forms.Button _cancelButton;
        private System.Windows.Forms.LinkLabel _accountManagerLink;
    }
}
