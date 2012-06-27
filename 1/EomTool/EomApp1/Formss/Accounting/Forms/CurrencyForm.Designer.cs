namespace EomApp1.Formss.Accounting.Forms
{
    partial class CurrencyForm
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
            this.currencies1 = new EomApp1.Formss.Accounting.Controls.Currencies();
            this.SuspendLayout();
            // 
            // currencies1
            // 
            this.currencies1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.currencies1.Location = new System.Drawing.Point(0, 0);
            this.currencies1.Name = "currencies1";
            this.currencies1.Size = new System.Drawing.Size(296, 195);
            this.currencies1.TabIndex = 0;
            // 
            // CurrencyForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(296, 195);
            this.Controls.Add(this.currencies1);
            this.Name = "CurrencyForm";
            this.ShowIcon = false;
            this.Text = "Currency Setup";
            this.Load += new System.EventHandler(this.CurrencyForm_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private Controls.Currencies currencies1;
    }
}