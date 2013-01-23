namespace QuickBooks.UI.Controls
{
    partial class QuickBooksSynchControl
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
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.companyFileBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.targetsCheckBoxList = new System.Windows.Forms.CheckedListBox();
            this.goButton = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.companyFileBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // comboBox1
            // 
            this.comboBox1.DataSource = this.companyFileBindingSource;
            this.comboBox1.DisplayMember = "Name";
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Location = new System.Drawing.Point(3, 3);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(176, 21);
            this.comboBox1.TabIndex = 0;
            this.comboBox1.ValueMember = "Id";
            // 
            // companyFileBindingSource
            // 
            this.companyFileBindingSource.DataSource = typeof(QuickBooks.UI.Models.CompanyFile);
            // 
            // targetsCheckBoxList
            // 
            this.targetsCheckBoxList.CheckOnClick = true;
            this.targetsCheckBoxList.FormattingEnabled = true;
            this.targetsCheckBoxList.Items.AddRange(new object[] {
            "Customer",
            "Invoice",
            "Payment",
            "CreditMemo"});
            this.targetsCheckBoxList.Location = new System.Drawing.Point(3, 30);
            this.targetsCheckBoxList.Name = "targetsCheckBoxList";
            this.targetsCheckBoxList.Size = new System.Drawing.Size(176, 64);
            this.targetsCheckBoxList.TabIndex = 4;
            // 
            // goButton
            // 
            this.goButton.BackColor = System.Drawing.SystemColors.Control;
            this.goButton.Enabled = false;
            this.goButton.ForeColor = System.Drawing.Color.Black;
            this.goButton.Location = new System.Drawing.Point(3, 100);
            this.goButton.Name = "goButton";
            this.goButton.Size = new System.Drawing.Size(176, 34);
            this.goButton.TabIndex = 5;
            this.goButton.Text = "Synch QuickBooks";
            this.goButton.UseVisualStyleBackColor = false;
            this.goButton.Click += new System.EventHandler(this.goButton_Click);
            // 
            // QuickBooksSynchControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.goButton);
            this.Controls.Add(this.targetsCheckBoxList);
            this.Controls.Add(this.comboBox1);
            this.Name = "QuickBooksSynchControl";
            this.Size = new System.Drawing.Size(183, 138);
            ((System.ComponentModel.ISupportInitialize)(this.companyFileBindingSource)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.BindingSource companyFileBindingSource;
        private System.Windows.Forms.CheckedListBox targetsCheckBoxList;
        private System.Windows.Forms.Button goButton;
    }
}
