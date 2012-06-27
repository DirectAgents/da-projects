namespace EomApp1.Formss.Affiliate.Forms
{
    partial class AffiliatesForm
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
            this.affiliates1 = new EomApp1.Formss.Affiliate.Controls.Affiliates();
            this.SuspendLayout();
            // 
            // affiliates1
            // 
            this.affiliates1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.affiliates1.Location = new System.Drawing.Point(0, 0);
            this.affiliates1.Name = "affiliates1";
            this.affiliates1.Size = new System.Drawing.Size(884, 462);
            this.affiliates1.TabIndex = 0;
            // 
            // AffiliatesForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(884, 462);
            this.Controls.Add(this.affiliates1);
            this.Name = "AffiliatesForm";
            this.ShowIcon = false;
            this.Text = "Affiliates";
            this.ResumeLayout(false);

        }

        #endregion

        private Controls.Affiliates affiliates1;
    }
}