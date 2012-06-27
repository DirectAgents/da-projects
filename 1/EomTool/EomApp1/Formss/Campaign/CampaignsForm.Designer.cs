namespace EomApp1.Formss.Campaign
{
    partial class CampaignsForm
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
            this.campaignsUC1 = new EomApp1.Formss.Campaign.CampaignsUC();
            this.SuspendLayout();
            // 
            // campaignsUC1
            // 
            this.campaignsUC1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.campaignsUC1.Location = new System.Drawing.Point(0, 0);
            this.campaignsUC1.Name = "campaignsUC1";
            this.campaignsUC1.Size = new System.Drawing.Size(1031, 511);
            this.campaignsUC1.TabIndex = 0;
            // 
            // CampaignsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1031, 511);
            this.Controls.Add(this.campaignsUC1);
            this.Name = "CampaignsForm";
            this.Text = "CampaignsForm";
            this.Load += new System.EventHandler(this.CampaignsForm_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private CampaignsUC campaignsUC1;

    }
}