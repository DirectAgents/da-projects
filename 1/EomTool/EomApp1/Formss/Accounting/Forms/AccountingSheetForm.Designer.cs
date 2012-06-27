namespace EomApp1.Formss.Accounting.Forms
{
    partial class AccountingSheetForm
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
            this.accountingSheet1 = new EomApp1.Formss.Accounting.Controls.AccountingSheet();
            this.SuspendLayout();
            // 
            // accountingSheet1
            // 
            this.accountingSheet1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.accountingSheet1.Location = new System.Drawing.Point(0, 0);
            this.accountingSheet1.Name = "accountingSheet1";
            this.accountingSheet1.Size = new System.Drawing.Size(1120, 602);
            this.accountingSheet1.TabIndex = 0;
            // 
            // AccountingSheetForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1120, 602);
            this.Controls.Add(this.accountingSheet1);
            this.Name = "AccountingSheetForm";
            this.Text = "AccountingSheetForm";
            this.ResumeLayout(false);

        }

        #endregion

        private Controls.AccountingSheet accountingSheet1;
    }
}